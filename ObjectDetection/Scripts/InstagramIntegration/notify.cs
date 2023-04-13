using UnityEngine;

public class VideoUploadNotification : MonoBehaviour
{
    public GameObject notificationPanel;

    private bool isUploading = false; 

    public void StartUpload()
    {
        isUploading = true;
    }

    public void FinishUpload()
    {
        isUploading = false;
        notificationPanel.SetActive(true);
    }

    private void Update()
    {
        if (isUploading)
        {
            if (/* video upload is complete */)
            {
                FinishUpload();
            }
        }
    }
}
