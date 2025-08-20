using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneViewMode { TopDown, SideScroll }


public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
{
    public DoorName SpawnDoorName { get; private set; }

    //private SceneViewMode mode = SceneViewMode.TopDown;
    //public SceneViewMode ViewMode
    //{
    //    get => mode;
    //    set
    //    {
    //        if (mode != value)
    //        {
    //            mode = value;
    //            OnModeChanged?.Invoke(mode);
    //        }
    //    }
    //}

    //public event Action<SceneViewMode> OnModeChanged;

    public void TransitionTo(SceneName targetScene, DoorName targetDoor)
    {
        SpawnDoorName = targetDoor;
        Debug.Log($"Sceneêÿë÷: {targetScene}, targetDoor: {targetDoor},SpawnDoorName:{SpawnDoorName}");
        SceneManager.LoadScene(targetScene.ToString());
    }

}
