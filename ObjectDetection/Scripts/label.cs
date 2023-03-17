using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.DnnModule;
using OpenCVForUnity.UnityUtils;

public class YOLOObjectDetector : MonoBehaviour
{
    public Text outputText;
    public string[] classes;
    public float confThreshold = 0.5f;
    public float nmsThreshold = 0.4f;
    public int inpWidth = 416;
    public int inpHeight = 416;
    public string modelConfiguration;
    public string modelWeights;

    private Net net;
    private List<string> outNames;
    private List<int> outLayers;

    private void Start()
    {
        // Load the YOLO model
        net = Dnn.readNetFromDarknet(modelConfiguration, modelWeights);
        outNames = net.getUnconnectedOutLayersNames();
        outLayers = new List<int>();
        foreach (string name in outNames)
        {
            outLayers.Add(net.getLayerId(name));
        }
    }

    private void Update()
    {
        // Capture an image from the camera
        Mat image = gameObject.GetComponent<Camera>().GetImage();

        // Create a 4D blob from the image
        Size inpSize = new Size(inpWidth, inpHeight);
        Mat blob = Dnn.blobFromImage(image, 1.0 / 255, inpSize, new Scalar(0, 0, 0), true, false);

        // Pass the blob through the YOLO network
        net.setInput(blob);
        List<Mat> outs = new List<Mat>();
        net.forward(outs, outNames);

        // Decode the output of the YOLO network
        List<int> classIds = new List<int>();
        List<float> confidences = new List<float>();
        List<Rect2d> boxes = new List<Rect2d>();
        for (int i = 0; i < outs.Count; ++i)
        {
            Mat output = outs[i];
            for (int j = 0; j < output.rows(); ++j)
            {
                float[] data = output.row(j).get(0, 0);
                int classId = -1;
                float confidence = 0;
                Rect2d box = new Rect2d();
                Dnn.NMSBoxes(new List<Rect2d> { box }, new List<float> { confidence }, confThreshold, nmsThreshold);
                if (classId >= 0 && confidence >= confThreshold)
                {
                    classIds.Add(classId);
                    confidences.Add(confidence);
                    boxes.Add(box);
                }
            }
        }

        // Label the detected objects in the image
        for (int i = 0; i < boxes.Count; ++i)
        {
            Rect2d box = boxes[i];
            int classId = classIds[i];
            float confidence = confidences[i];
            string label = classes[classId];
            outputText.text += label + " " + confidence + "\n";
            Imgproc.putText(image, label, new Point(box.x, box.y), Imgproc.FONT_HERSHEY_SIMPLEX, 1, new Scalar(255, 255, 255), 2);
            Imgproc.rectangle(image, box, new Scalar(255, 0, 0), 2);
        }

        // Show the image in the camera
        gameObject.GetComponent<Camera>().ShowImage(image);
    }
}
