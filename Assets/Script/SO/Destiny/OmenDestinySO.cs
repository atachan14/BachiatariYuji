using UnityEngine;
public enum OmenType
{
    NoneOmen, FloorOmen, WallOmen, BeyondOmen
}

[CreateAssetMenu(fileName = "OmenDestinySO", menuName = "DestinySO/OmenDestinySO")]
public class OmenDestinySO : DestinySO
{
    public OmenType omenType;
}
