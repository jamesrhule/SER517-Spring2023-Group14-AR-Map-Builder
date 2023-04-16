using UnityEngine;
using Vuforia;

public class ObjectTargetScript : MonoBehaviour, ITrackableEventHandler
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
        if (newStatus == TrackableBehaviour.Status.DETECTED || 
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            // Object is detected or tracked
            OnTrackingFound();
        }
        else
        {
            // Object is lost
            OnTrackingLost();
        }
    }
    
    private void OnTrackingFound()
    {
        // Code to execute when object is detected or tracked
        Debug.Log("Object found");
    }
    
    private void OnTrackingLost()
    {
        // Code to execute when object is lost
        Debug.Log("Object lost");
    }
}
