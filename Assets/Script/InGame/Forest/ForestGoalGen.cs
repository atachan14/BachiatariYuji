using UnityEngine;

public class ForestGoalGen : SingletonMonoBehaviour<ForestGoalGen>
{
    [field: SerializeField] public Transform GoalDoor { get; set; }
    [field: SerializeField] public Transform SpawnPos { get; set; }
    public void Generate()
    {
    }
}