using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is only a rudimentary implementation of the time system i want, will refine soon.
public class TimeBody : MonoBehaviour
{
    public TimefluxLogic timefluxLogic;
    private bool isRewinding = false;
    private Rigidbody rb;

    private List<TimeTransformData> pointsInTime;

    public CameraDistortion cameraDistortion;
    public float recordTime = 5f;
    private int rewindSpeedMultiplier = 2;

    

    void Start()
    {
        pointsInTime = new List<TimeTransformData>();
        rb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        if (isRewinding)
        {
            DepleteBattery();
            StartRewind();
        }
        else
        {
            StopRewind();
        }
    }

    private void Rewind()
    {
        int iterations = Mathf.Min(rewindSpeedMultiplier, pointsInTime.Count);

        for (int i = 0; i < iterations; i++)
        {
            TimeTransformData pointInTime = pointsInTime[0];
            transform.position = pointInTime.pos;
            transform.rotation = pointInTime.rot;
            pointsInTime.RemoveAt(0);
        }

        if (pointsInTime.Count <= 0)
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
        //Ensures we cant rewind once charge is 0
        if (timefluxLogic.batteryCharge > 0)
        {
            isRewinding = true;
            rb.isKinematic = true;
        }
        else
        {
            timefluxLogic.batteryCharge = 0;
            Debug.Log("Timeflux battery is depleted with a value of : " +  timefluxLogic.batteryCharge);
            StopRewind();
        }
        
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }

    private void DepleteBattery()
    {
        float usageThisFrame = timefluxLogic.drainRate * Time.deltaTime;
        timefluxLogic.batteryCharge -= usageThisFrame;
        Debug.Log("Charge remaining: " + timefluxLogic.batteryCharge);
    }


}
