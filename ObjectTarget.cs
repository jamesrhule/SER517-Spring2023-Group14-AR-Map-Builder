using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ObjectTargeting : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;

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
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)
        {
            // Object is being tracked, activate it
            gameObject.SetActive(true);
        }
        else
        {
            // Object is not being tracked, deactivate it
            gameObject.SetActive(false);
        }
    }
}
