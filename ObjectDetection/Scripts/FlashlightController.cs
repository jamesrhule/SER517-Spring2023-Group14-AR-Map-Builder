using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashlightController : MonoBehaviour {

    private bool isFlashOn = false;
    private Camera mainCamera;

    void Start () {
        mainCamera = Camera.main;
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
            Debug.Log("No camera permission granted.");
            return;
        }
    }

    public void ToggleFlashlight () {
        if (!isFlashOn) {
            mainCamera.GetComponent<Light>().enabled = true;
            isFlashOn = true;
        } else {
            mainCamera.GetComponent<Light>().enabled = false;
            isFlashOn = false;
        }
    }
}
