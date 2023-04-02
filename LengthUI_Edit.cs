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

        // Set the input fields to default values
        minutesInput.text = "0";
        secondsInput.text = "0";
    }

    void DisplayVideoLength()
    {
        // Parse the minutes and seconds input
        int minutes = int.Parse(minutesInput.text);
        int seconds = int.Parse(secondsInput.text);

        // Check for invalid input values
        if (minutes < 0 || seconds < 0 || seconds > 59)
        {
            buttonText.text = "Invalid Input";
            return;
        }

        // Calculate the total length in seconds
        int totalSeconds = (minutes * 60) + seconds;

        // Set the button text to display the length
        TimeSpan length = TimeSpan.FromSeconds(totalSeconds);
        buttonText.text = "Video Length: " + length.ToString(@"mm\:ss");

        // Set the input fields to the parsed values
        minutesInput.text = minutes.ToString();
        secondsInput.text = seconds.ToString();
    }
}
