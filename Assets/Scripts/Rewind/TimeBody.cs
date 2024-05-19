using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    [SerializeField] private int RecordTime = 5;
    [SerializeField] TextMeshProUGUI Count;

    private bool isRewinding = false;
    private List<PointInTime> point = new List<PointInTime>();

    private Rigidbody rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartRewind();
        }

        if (Input.GetKeyUp(KeyCode.E))
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
            Count.text = (point.Count / 50).ToString();
        }
    }

    private void Rewind()
    {
        if (point.Count > 0)
        {
            PointInTime pointinTime = point[0];
            transform.position = pointinTime.position;
            transform.rotation = pointinTime.rotation;
            point.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    private void Record()
    {
        if (point.Count > Mathf.Round( RecordTime / Time.fixedDeltaTime))
        {
            point.RemoveAt(point.Count - 1);
        }
        
        point.Insert(0, new PointInTime(transform.position, transform.rotation));
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
