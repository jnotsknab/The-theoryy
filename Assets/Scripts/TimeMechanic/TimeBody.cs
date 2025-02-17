using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is only a rudimentary implementation of the time system i want, will refine soon.
public class TimeBody : MonoBehaviour
{   
    private bool isRewinding = false;
    private Rigidbody rb;

    private List<TimeTransformData> pointsInTime;

    public float recordTime = 5f;
    void Start()
    {
        pointsInTime = new List<TimeTransformData>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isRewinding)
        {
            StartRewind();
        }
        else
        {
            StopRewind();
        }
    }

    private void Rewind()
    {   
        if (pointsInTime.Count > 0)
        {   
            TimeTransformData pointInTime = pointsInTime[0];
            transform.position = pointInTime.pos;
            transform.rotation = pointInTime.rot;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
        
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    /// <summary>
    /// Records positions in time by adding them to time transform data.
    /// </summary>
    private void Record()
    {
        if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new TimeTransformData(transform.position, transform.rotation));
    }
    public void StartRewind()
    {   
        isRewinding = true;
        rb.isKinematic = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }


}
