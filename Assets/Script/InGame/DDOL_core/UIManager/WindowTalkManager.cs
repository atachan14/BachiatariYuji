using System.Collections;
using UnityEngine;
using TMPro;

public class WindowTalkManager : SingletonMonoBehaviour<WindowTalkManager>
{
    [SerializeField] private GameObject btmWindow;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private GameObject nextCursor;
    [SerializeField] private int maxVisibleLines = 3;
    [SerializeField] private float charDelay = 0.05f; // �b���Œ���
    [SerializeField] private float fastDelay = 0.0f;  // �����莞��1f or ���\��

    private WindowTalkData currentData;
    private int textIndex;
    private bool isFastForward = false;


    // ===== Public API =====
    public void ShowUnder(WindowTalkData data)
    {
        currentData = data;
        StopAllCoroutines();
        StartCoroutine(PlayTalkRoutine());
    }

    // ===== Driver =====
    private IEnumerator PlayTalkRoutine()
    {
        EnterUnderTalkMode();

        for (textIndex = 0; textIndex < currentData.text.Length; textIndex++)
        {
            yield return StartCoroutine(PlayOneElement(currentData.text[textIndex]));

            // �v�f�̏I���Ń����e���|�҂� �� �y�[�W�N���A
            yield return WaitNextPress();
            SetText("");
        }

        ExitUnderTalkMode();

        if (currentData.nextEvent != null)
            currentData.nextEvent.DoEvent();
    }

    private IEnumerator PlayOneElement(string fullText)
    {
        isFastForward = false;

        foreach (char c in fullText)
        {
            // �����ǉ��i�I�[�o�[���͎����ő҂����X�N���[���j
            yield return TryAppendChar(c);

            // �������Ƃ̃f�B���C�i������Ή��j
            yield return WaitForCharDelay(charDelay);
        }

    }

    // 1�����ǉ����čs�I�[�o�[����
    private IEnumerator TryAppendChar(char c)
    {
        AppendChar(c);
        tmp.ForceMeshUpdate();

        // �s���I�[�o�[�H
        if (tmp.textInfo.lineCount > maxVisibleLines)
        {
            // ���O�̒ǉ���߂�
            tmp.text = tmp.text.Substring(0, tmp.text.Length - 1);
            tmp.ForceMeshUpdate();

            // ���͑҂�
            yield return WaitNextPress();

            // �X�N���[�����Ă��當�����Ēǉ�
            ScrollOffTopLine();
            AppendChar(c);
        }
    }

    // �b���ҋ@�������t���`�F�b�N���đ�����
    private IEnumerator WaitForCharDelay(float delay)
    {
        float timer = 0f;
        while (timer < delay)
        {
            // ������Ă��瑁����ɐ؂�ւ�
            if (InputReceiver.Instance.NextTalk)
                isFastForward = true;

            float dt = Time.deltaTime;
            if (isFastForward)
                dt *= 10f; // �����葬�x�����i������1f�ł�OK�j

            timer += dt;
            yield return null;
        }
    }



    // ===== Helpers =====
    private void EnterUnderTalkMode()
    {
        btmWindow.SetActive(true);
        SetText("");
        InputReceiver.Instance.SwitchMode(InputMode.UnderTalk);
    }

    private void ExitUnderTalkMode()
    {
        btmWindow.SetActive(false);
        InputReceiver.Instance.SwitchMode(SceneSetuper.Instance.GetInputMode());
    }

    private void SetText(string s)
    {
        tmp.text = s;
        tmp.ForceMeshUpdate();
    }

    private void AppendChar(char c)
    {
        tmp.text += c;
        tmp.ForceMeshUpdate();
    }

    private IEnumerator WaitNextPress()
    {
        // ���}�[�N��\��
        nextCursor.SetActive(true);

        // ���͑҂�
        yield return new WaitUntil(() => InputReceiver.Instance.NextTalk);

        // ���}�[�N���\��
        nextCursor.SetActive(false);

        // �������t���O�����Z�b�g
        InputReceiver.Instance.NextTalk = false;
        isFastForward = false;
    }



    // �擪�́g�����Ă���1�s�h������؂藎�Ƃ��ăX�N���[��
    private void ScrollOffTopLine()
    {
        var ti = tmp.textInfo;
        if (ti.lineCount == 0) return;

        var l0 = ti.lineInfo[0];
        int cutStart = l0.firstCharacterIndex;           // ���Ԃ� 0
        int cutEndInclusive = l0.lastCharacterIndex;     // �擪�s�̍Ō�̉�����
        int cutLen = cutEndInclusive - cutStart + 1;

        string t = tmp.text;

        // ���̕��������s�Ȃ炻����ꏏ�ɗ��Ƃ��i�����ڏ�̉��s/�������s�ǂ���ł����Ȃ��j
        int nextIdx = cutEndInclusive + 1;
        if (nextIdx < t.Length && t[nextIdx] == '\n')
            cutLen++;

        string rest = t.Substring(cutStart + cutLen);
        // �擪�ɉ��s���c���Ă���|��
        rest = rest.TrimStart('\n', '\r');

        SetText(rest);
    }
}
