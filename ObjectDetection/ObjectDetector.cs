using UnityEngine;
using OpenCvSharp;

public class ObjectDetector : MonoBehaviour
{
    // Load the object detection model
    private CvSVM detector = new CvSVM();
    private Mat model = CvSVM.load("detector.xml");
    detector.load(model.fileName);

    // Read the input image or video
    private WebCamTexture webcamTexture;
    private Mat rgbaMat;

    void Start()
    {
        // Start the webcam
        webcamTexture = new WebCamTexture();
        webcamTexture.Play();

        // Initialize the rgbaMat with the webcam resolution
        rgbaMat = new Mat(webcamTexture.height, webcamTexture.width, CvType.CV_8UC4);
    }

    void Update()
    {
        // Update the rgbaMat with the latest webcam frame
        Utils.webCamTextureToMat(webcamTexture, rgbaMat);

        // Run the object detection
        Mat objects = new Mat();
        detector.predict(rgbaMat, objects, false);

        // Draw the detection results
        for (int i = 0; i < objects.rows(); i++)
        {
            int x = (int)objects.get(i, 0)[0];
            int y = (int)objects.get(i, 1)[0];
            int width = (int)objects.get(i, 2)[0];
            int height = (int)objects.get(i, 3)[0];
            Imgproc.rectangle(rgbaMat, new Point(x, y), new Point(x + width, y + height), new Scalar(255, 0, 0), 2);
        }

        // Display the results
        Texture2D texture = new Texture2D(rgbaMat.cols(), rgbaMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(rgbaMat, texture);
        GetComponent<Renderer>().material.mainTexture = texture;
    }

    void OnDestroy()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
            webcamTexture = null;
        }
    }
}
