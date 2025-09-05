using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneViewMode { TopDown, SideScroll }


public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
{
    public DoorName SpawnDoorName { get; private set; }

    public void TransitionTo(SceneName targetScene, DoorName targetDoor)
    {
        SpawnDoorName = targetDoor;
        SceneManager.LoadScene(targetScene.ToString());
    }

}
