using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public RawImage photoDisplay;
	private Camera frontCamera;
    private Camera backCamera;

    private bool isUsingFrontCamera = true;
	
	void Start()
    {
        frontCamera = Camera.main;
        backCamera = Camera.allCameras[1];
        backCamera.enabled = false;
    }

    private Texture2D photoTexture;

    // Captures a photo from the camera.
    public void TakePhoto()
    {
  
        int width = Screen.width;
        int height = Screen.height;
        photoTexture = new Texture2D(width, height, TextureFormat.RGB24, false);

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        camera.targetTexture = renderTexture;
        camera.Render();
        RenderTexture.active = renderTexture;
        photoTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        photoTexture.Apply();

        photoDisplay.texture = photoTexture;
    }

    // Saves the photo.
    public void SavePhoto()
    {
        // Convert the photo texture to a byte array.
        byte[] bytes = photoTexture.EncodeToJPG();

        string filename = string.Format("{0}/photo_{1}.jpg", Application.persistentDataPath, System.DateTime.Now.ToString("yyyyMMdd_HHmmss"));

        File.WriteAllBytes(filename, bytes);
    }

    void swapCamera()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            isUsingFrontCamera = !isUsingFrontCamera;
            frontCamera.enabled = isUsingFrontCamera;
            backCamera.enabled = !isUsingFrontCamera;
        }
    }
}
