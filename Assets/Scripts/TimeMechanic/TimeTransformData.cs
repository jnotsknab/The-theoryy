
using UnityEngine;

public class TimeTransformData
{
    public Vector3 pos;
    public Quaternion rot;

    public TimeTransformData (Vector3 _pos, Quaternion _rot)
    {
        pos = _pos; rot = _rot;
    }
}
