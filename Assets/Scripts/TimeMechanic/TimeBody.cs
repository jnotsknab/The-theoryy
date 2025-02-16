using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{   
    private bool isRewinding = false;
    private Rigidbody rb;

    private List<TimeTransformData> pointsInTime;

    public float recordTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        pointsInTime = new List<TimeTransformData>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
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
