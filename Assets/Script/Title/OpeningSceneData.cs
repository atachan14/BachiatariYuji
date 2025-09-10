using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneData : SceneData
{
    
    protected override void Start()
    {
    }
    private void Update()
    {
        if(Input.anyKeyDown)
            SceneChanger.Instance.TransitionTo(SceneName.House2F, DoorName.Start);
    }
}
