using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// TitleManager
public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField] BaseNode titleMenuStartNode;
    [SerializeField] BaseNode gameStartNode;
    public MenuBase currentMenu;
    public TitleMenuController titleMenu;
    public LanguageMenuController languageMenu;
    public AchieveMenuController achieveMenu;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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
        if (scene.name == SceneName.House2F.ToString())
        {
            StartCoroutine(StartTitle());
        }
    }

    private IEnumerator StartTitle()
    {
        yield return null; // 1�t���[���҂�
        InputReceiver.Instance.SwitchMode(InputMode.Dialog); // Title�p�ɏ㏑��
        titleMenuStartNode.PlayNode();
        ShowTitleMenu();
    }

    public void ShowTitleMenu()
    {
        SwitchMenu(titleMenu);
    }

    public void ShowLanguageMenu()
    {
        Debug.Log("language");
        SwitchMenu(languageMenu);
    }

    public void ShowArchiveMenu()
    {
        SwitchMenu(achieveMenu);
    }

    private void SwitchMenu(MenuBase nextMenu)
    {
        if (currentMenu != null)
            currentMenu.Hide();

        currentMenu = nextMenu;
        currentMenu.Show();
    }

    public void StartGame()
    {
        gameStartNode.PlayNode();
        // �Q�[���J�n���o�Ăяo��
    }

    public void ContinueGame()
    {
        Debug.Log("Continue Game!");
    }
}
