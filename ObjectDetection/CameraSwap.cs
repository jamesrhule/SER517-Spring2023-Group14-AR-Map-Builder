using UnityEngine;
using UnityEngine.UI;

public class CameraSwap : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;
    public Button switchButton;

    void Start()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        switchButton.onClick.AddListener(SwapCameras);
    }

    void SwapCameras()
    {
        mainCamera.enabled = !mainCamera.enabled;
        secondaryCamera.enabled = !secondaryCamera.enabled;
    }
}
