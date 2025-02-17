
using UnityEngine;

/// <summary>
/// Holds the transform data for objects that have a time body (ie can be paused, slowed, rewound, etc.)
/// </summary>
public class TimeTransformData
{
    public Vector3 pos;
    public Quaternion rot;

    public TimeTransformData (Vector3 _pos, Quaternion _rot)
    {
        pos = _pos; rot = _rot;
    }
}
