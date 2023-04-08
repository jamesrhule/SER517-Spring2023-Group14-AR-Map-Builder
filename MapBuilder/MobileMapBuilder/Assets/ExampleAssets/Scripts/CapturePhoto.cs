using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class CapturePhoto : MonoBehaviour
{

    private Texture2D texture;
    private string imagePath;

    void Start()
    {
        Debug.Log("Script started... ");
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    public void OnClickCapture()
    {
        Debug.Log("Button clicked");

        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        imagePath = Path.Combine(Application.persistentDataPath, "photo.png");
        File.WriteAllBytes(imagePath, bytes);

        Debug.Log("Screenshot captured");

    }

}
