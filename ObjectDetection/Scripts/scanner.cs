using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QRCodeScanner : MonoBehaviour
{
    private WebCamTexture camTexture;
    private Rect screenRect;

    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.Play();
    }

    void OnGUI()
    {

        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);


        IBarcodeReader reader = new BarcodeReader();
        Result result = reader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);

        // display the decoded text
        if (result != null)
        {
            GUI.Label(new Rect(10, Screen.height - 50, 500, 50), "QR Code Text: " + result.Text);
        }
    }

    void OnDisable()
    {
        if (camTexture != null)
        {
            camTexture.Stop();
        }
    }
}
