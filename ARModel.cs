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
            // Object is being tracked
            // Do something, such as show a 3D model
            // You can use the transform property to position and rotate the model
            transform.position = mTrackableBehaviour.transform.position;
            transform.rotation = mTrackableBehaviour.transform.rotation;
        }
        else
        {
            // Object is no longer being tracked
            // Do something else, such as hide the model
        }
    }
}
