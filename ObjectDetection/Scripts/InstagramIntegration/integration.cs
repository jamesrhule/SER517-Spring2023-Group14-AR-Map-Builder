using UnityEngine;
using System.Collections;

public class CameraCapture : MonoBehaviour {

    public GameObject camera;
    public Texture2D image;

    void Start() {
        camera = GetComponent<Camera>();
        image = new Texture2D(camera.Screen.width, camera.Screen.height);

        camera.CaptureImage(image);
    }
}