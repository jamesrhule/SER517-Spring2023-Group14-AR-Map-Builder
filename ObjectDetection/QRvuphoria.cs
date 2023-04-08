using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QRCodeReader : MonoBehaviour
{
    public RawImage cameraScreen;
    private WebCamTexture camTexture;
    private IBarcodeReader barcodeReader;

    void Start()
    {
        // Initialize camera texture
        camTexture = new WebCamTexture();
        cameraScreen.texture = camTexture;
        camTexture.Play();

        // Initialize barcode reader
        barcodeReader = new BarcodeReader();
    }

    void Update()
    {
        // Capture camera input
        Texture2D camTex = new Texture2D(camTexture.width, camTexture.height);
        camTex.SetPixels(camTexture.GetPixels());
        camTex.Apply();

        // Process QR code
        var result = barcodeReader.Decode(camTex.GetPixels32(),
            camTex.width, camTex.height);

        if (result != null)
        {
            Debug.Log("QR Code Detected: " + result.Text);

            // Display QR code information on plane object
            TextMesh textMesh = GetComponent<TextMesh>();
            textMesh.text = result.Text;
        }
    }
}
