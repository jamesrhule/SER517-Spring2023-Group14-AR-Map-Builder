using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using System.IO;

public class RecordVideo : MonoBehaviour
{
    public Button recordButton; // the button to start recording
    public VideoPlayer videoPlayer; // the video player component to preview the recorded video
    public string videoSavePath; // the path to save the recorded video

    private bool isRecording = false; // flag to indicate if recording is in progress

    void Start()
    {
        recordButton.onClick.AddListener(OnRecordButtonPressed); // add a listener to the record button
    }

    void Update()
    {
        if (isRecording)
        {
            // check if the record button is still being pressed
            if (!recordButton.IsPressed())
            {
                // stop recording when the button is released
                StopRecording();
            }
        }
    }

    void OnRecordButtonPressed()
    {
        // start recording when the button is long-pressed
        StartCoroutine(StartRecording());
    }

    IEnumerator StartRecording()
    {
        isRecording = true;

        // create a new video capture object
        var videoCapture = new UnityEngine.XR.ARFoundation.ARVideoCapture();

        // set the recording mode to high quality
        var videoCaptureConfig = UnityEngine.XR.ARFoundation.ARVideoCaptureConfig.Create();
        videoCaptureConfig.videoQuality = UnityEngine.XR.ARFoundation.ARVideoQuality.HighQuality;

        // start the video capture session
        var videoCaptureStarted = videoCapture.StartRecording(videoCaptureConfig);

        // wait until the video capture session has started
        while (!videoCaptureStarted)
        {
            yield return null;
        }

        // wait until the record button is released
        while (recordButton.IsPressed())
        {
            yield return null;
        }

        // stop the video capture session
        var videoCaptureStopped = videoCapture.StopRecording();

        // wait until the video capture session has stopped
        while (!videoCaptureStopped)
        {
            yield return null;
        }

        isRecording = false;

        // save the recorded video to the specified path
        var videoFilePath = Path.Combine(Application.persistentDataPath, videoSavePath);
        File.Move(videoCapture.outputFilepath, videoFilePath);

        // preview the recorded video
        videoPlayer.url = videoFilePath;
        videoPlayer.Play();
    }

    void StopRecording()
    {
        isRecording = false;
    }
}
