using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    private bool isFlashlightOn = false;
    private Light flashlight;

    void Start()
    {
        Debug.Log("Flashlight script started.....");
        flashlight = GetComponent<Light>();
        flashlight.enabled = false;

        Button flashlightButton = GetComponent<Button>();
        flashlightButton.onClick.AddListener(ToggleFlashlight);
    }

    void ToggleFlashlight()
    {
        Debug.Log("flashhhhhhhhhhhhhh.....");
        isFlashlightOn = !isFlashlightOn;

        if (isFlashlightOn)
        {
            flashlight.enabled = true;
        }
        else
        {
            flashlight.enabled = false;
        }
    }
}
