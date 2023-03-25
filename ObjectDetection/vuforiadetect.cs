using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MyObjectBehavior : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            // Object has been detected, add behavior here:
            Debug.Log("Object detected!");
        }
        else
        {
            // Object is no longer being tracked, remove behavior here:
            Debug.Log("Object lost!");
        }
    }
}
