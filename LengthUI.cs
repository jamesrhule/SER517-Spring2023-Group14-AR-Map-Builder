using UnityEngine;
using UnityEngine.UI;
using System;

public class VideoLengthInputButton : MonoBehaviour
{
    public InputField minutesInput;
    public InputField secondsInput;
    public Text buttonText;

    void Start()
    {
        // Add a listener to the button
        Button button = GetComponent<Button>();
        button.onClick.AddListener(DisplayVideoLength);
    }

    void DisplayVideoLength()
    {
        // Parse the minutes and seconds input
        int minutes = int.Parse(minutesInput.text);
        int seconds = int.Parse(secondsInput.text);

        // Calculate the total length in seconds
        int totalSeconds = (minutes * 60) + seconds;

        // Set the button text to display the length
        TimeSpan length = TimeSpan.FromSeconds(totalSeconds);
        buttonText.text = "Video Length: " + length.ToString(@"mm\:ss");
    }
}
