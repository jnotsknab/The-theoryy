//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// Records the positions and rotations of a gameobject every frame.
///// </summary>
//public class TransformRecorder : MonoBehaviour
//{
//    private List<Vector3> mRecordedPositions = new List<Vector3>();
//    private List<Quaternion> mRecordedRotations = new List<Quaternion>();

//    private void OnEnable()
//    {
//        PlaybackController.Instance.RegisterRecorder(this);
//    }

//    private void OnDisable()
//    {
//        PlaybackController.Instance.UnregisterRecorder(this);
//    }

//    private void Update()
//    {   
//        Record();
//    }

//    private void Record()
//    {
//        mRecordedPositions.Add(transform.position);
//        mRecordedRotations.Add(transform.rotation);
//    }

//    public void RestoreFrame(int frame)
//    {   
//        //Recorded positions count should never be more than the frame
//        Debug.Assert(mRecordedRotations.Count > frame);
//        transform.position = mRecordedPositions[frame];
//        transform.rotation = mRecordedRotations[frame];

//    }

//}
