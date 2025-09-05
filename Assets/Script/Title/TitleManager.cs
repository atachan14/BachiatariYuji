using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TitleMode
{
    TitleMenu,
    LanguageMenu,
    AchieveMenu,
    StartGame,
    Wait
}

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField] private TitleMenuController titleMenu;
    [SerializeField] private LanguageMenuController languageMenu;
    [SerializeField] private AchieveMenuController achieveMenu;
    [SerializeField] private BaseNode titleStartNode;
    [SerializeField] private BaseNode gameStartNode; 

    public TitleMode currentMode = TitleMode.TitleMenu;
    public static event System.Action OnTitleMenu;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        StartCoroutine(InvokeTitleMenuAfterDelay());
    }

    private IEnumerator InvokeTitleMenuAfterDelay()
    {
        yield return null;
        OnTitleMenu?.Invoke();
        yield return null;
        titleStartNode.PlayNode();
        Debug.Log("inbod");
    }

    public void SwitchMode(TitleMode nextMode)
    {
        currentMode = nextMode;

        // 新しいモードを表示
        switch (nextMode)
        {
            case TitleMode.StartGame:
                StartCoroutine(PlayStartSequence());
                break;
            case TitleMode.TitleMenu:
                titleMenu.Show();
                break;
            case TitleMode.LanguageMenu:
                languageMenu.Show();
                break;
            case TitleMode.AchieveMenu:
                achieveMenu.Show();
                break;
            default:
                break;
        }
    }

    private IEnumerator PlayStartSequence()
    {
        yield return YujiAwake();
        yield return CamZoomOut();
        gameStartNode.PlayNode();
        Destroy(gameObject);

        // ゲーム開始時の演出用（現状は空）
    }
    IEnumerator YujiAwake()
    {
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator CamZoomOut()
    {
        Transform camTrans = CameraController.Instance.transform;

        Vector3 startPos = camTrans.position;
        Vector3 targetPos = new Vector3(0f, 0f, camTrans.position.z); // XYをゼロにする想定
        float startSize = Camera.main.orthographicSize;
        float targetSize = 5f;

        float duration = 2f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            camTrans.position = Vector3.Lerp(startPos, targetPos, t);
            Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        camTrans.position = targetPos;
        Camera.main.orthographicSize = targetSize;
    }







}
