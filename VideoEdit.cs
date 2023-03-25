using UnityEngine;
using UnityEngine.UI;

public class VideoCapture : MonoBehaviour
{
    // Public variables
    public Dropdown videoLengthDropdown;
    public Button startRecordingButton;
    public Button stopRecordingButton;
    public int videoLength;

    // Private variables
    private bool isRecording = false;
    private float recordingTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Add options to the dropdown menu
        videoLengthDropdown.options.Clear();
        videoLengthDropdown.options.Add(new Dropdown.OptionData("15 seconds"));
        videoLengthDropdown.options.Add(new Dropdown.OptionData("30 seconds"));
        videoLengthDropdown.options.Add(new Dropdown.OptionData("60 seconds"));
        videoLengthDropdown.value = 0;
        videoLengthDropdown.RefreshShownValue();

        // Set the initial video length to 15 seconds
        videoLength = 15;

        // Add listeners to the buttons
        startRecordingButton.onClick.AddListener(StartRecording);
        stopRecordingButton.onClick.AddListener(StopRecording);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecording)
        {
            // Increment the recording timer
            recordingTimer += Time.deltaTime;

            // Check if the recording timer has reached the specified length
            if (recordingTimer >= videoLength)
            {
                StopRecording();
            }
        }
    }

    // Start recording button click event
    void StartRecording()
    {
        // Set the recording flag to true
        isRecording = true;

        // Disable the start recording button and enable the stop recording button
        startRecordingButton.interactable = false;
        stopRecordingButton.interactable = true;

        // Get the selected value from the dropdown menu
        if (videoLengthDropdown.value == 0)
        {
            videoLength = 15;
        }
        else if (videoLengthDropdown.value == 1)
        {
            videoLength = 30;
        }
        else if (videoLengthDropdown.value == 2)
        {
            videoLength = 60;
        }

        // Start recording the video
        Debug.Log("Started recording " + videoLength + "-second video");
    }

    // Stop recording button click event
    void StopRecording()
    {
        // Set the recording flag to false and reset the recording timer
        isRecording = false;
        recordingTimer = 0.0f;

        // Disable the stop recording button and enable the start recording button
        stopRecordingButton.interactable = false;
        startRecordingButton.interactable = true;

        // Stop recording the video
        Debug.Log("Stopped recording video");
    }
}
