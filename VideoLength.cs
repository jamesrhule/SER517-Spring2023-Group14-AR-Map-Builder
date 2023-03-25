using UnityEngine;
using UnityEngine.UI;

public class VideoCapture : MonoBehaviour
{
    public Dropdown videoLengthDropdown;
    public int videoLength;

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
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
