using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitchTest
{
    [UnityTest]
    public IEnumerator CameraSwitches()
    {
        // Load the scene that contains the camera and the script that switches the camera
        SceneManager.LoadScene("MyScene");

        // Wait for the scene to load
        yield return null;

        // Find the camera GameObject in the scene
        GameObject cameraObject = GameObject.Find("Camera");

        // Get the initial active state of the camera
        bool initialActiveState = cameraObject.activeSelf;

        // Find the script that switches the camera
        CameraSwitch cameraSwitch = FindObjectOfType<CameraSwitch>();

        // Call the function that switches the camera
        cameraSwitch.SwitchCamera();

        // Get the new active state of the camera
        bool newActiveState = cameraObject.activeSelf;

        // Assert that the active state of the camera has changed
        Assert.AreNotEqual(initialActiveState, newActiveState);
    }
}
