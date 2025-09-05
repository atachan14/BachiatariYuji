using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforTitleBoot : SceneData
{
    
    protected override void Start()
    {
    }
    private void Update()
    {
        if(Input.anyKeyDown)
        SceneManager.LoadScene(SceneName.House2F.ToString());
    }
}
