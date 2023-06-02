using UnityEngine;

[SerializeField]
public enum PlayerRotationState
{
    left,
    right
}

[SerializeField]
public enum PlatformState
{
    simple,
    move,
    drop
}
[SerializeField]
public enum DropperState
{
    start,
    startSwitch,
    switching,
    waveing,
    drop
}

[SerializeField]
public enum BlockState
{
    simple,
    fly
}