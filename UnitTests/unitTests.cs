using NUnit.Framework;

public class YOLODetectionTests
{
    //Test if the YOLO model is loaded correctly:

    [Test]
    public void TestYOLOModelLoaded()
    {
        var yoloDetection = new YOLODetection();
        yoloDetection.LoadModel("yolo_model.pb");
        Assert.IsNotNull(yoloDetection.Model);
    }

    //Test if the YOLO model correctly detects the object within its detection threshold:
    [Test]
    public void TestYOLOObjectDetected()
    {
        var yoloDetection = new YOLODetection();
        yoloDetection.LoadModel("yolo_model.pb");
        var inputImage = new Texture2D(128, 128);
        var targetObject = new GameObject();
        targetObject.AddComponent<MeshRenderer>();
        var renderer = targetObject.GetComponent<MeshRenderer>();
        renderer.material.color = Color.red;
        var targetTexture = renderer.material.mainTexture = inputImage;
        yoloDetection.DetectionThreshold = 0.5f;
        Assert.IsTrue(yoloDetection.DetectObject(targetTexture));
    }

    //Test if the YOLO model correctly does not detect the object below its detection threshold:
    [Test]
    public void TestYOLOObjectNotDetected()
    {
        var yoloDetection = new YOLODetection();
        yoloDetection.LoadModel("yolo_model.pb");
        var inputImage = new Texture2D(128, 128);
        var targetObject = new GameObject();
        targetObject.AddComponent<MeshRenderer>();
        var renderer = targetObject.GetComponent<MeshRenderer>();
        renderer.material.color = Color.red;
        var targetTexture = renderer.material.mainTexture = inputImage;
        yoloDetection.DetectionThreshold = 0.9f;
        Assert.IsFalse(yoloDetection.DetectObject(targetTexture));
    }


}