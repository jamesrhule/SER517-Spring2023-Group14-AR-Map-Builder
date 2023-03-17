[TestFixture]
public class YoloAccuracyTests
{
    private YoloV3 detector;

    [SetUp]
    public void SetUp()
    {
        // Load YOLO model
        var model = ModelLoader.Load("YOLOv3.onnx");

        // Initialize the YOLO detector
        detector = new YoloV3(model);

        // Set hyperparameters
        detector.ConfidenceThreshold = 0.5f;
        detector.NmsThreshold = 0.5f;
    }

    [Test]
    public void TestObjectDetectionAccuracy()
    {
        // Load test image and ground truth
        var image = LoadImage("test_image.jpg");
        var expectedDetections = LoadGroundTruth("test_image_ground_truth.txt");

        // Detect objects in the input image
        var actualDetections = detector.Detect(image);

        // Compare the actual and expected detections
        Assert.AreEqual(expectedDetections.Count, actualDetections.Count);
        for (int i = 0; i < expectedDetections.Count; i++)
        {
            var expected = expectedDetections[i];
            var actual = actualDetections[i];

            Assert.AreEqual(expected.Label, actual.Label);
            Assert.AreEqual(expected.Confidence, actual.Confidence, 0.01f);
            Assert.AreEqual(expected.X, actual.X, 5);
            Assert.AreEqual(expected.Y, actual.Y, 5);
            Assert.AreEqual(expected.Width, actual.Width, 5);
            Assert.AreEqual(expected.Height, actual.Height, 5);
        }
    }

    private List<Detection> LoadGroundTruth(string groundTruthPath)
    {
        // Load ground truth from a text file
        var groundTruth = new List<Detection>();
        var lines = File.ReadAllLines(groundTruthPath);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            var label = parts[0];
            var x = float.Parse(parts[1]);
            var y = float.Parse(parts[2]);
            var width = float.Parse(parts[3]);
            var height = float.Parse(parts[4]);
            var confidence = float.Parse(parts[5]);
            groundTruth.Add(new Detection(label, x, y, width, height, confidence));
        }
        return groundTruth;
    }
}
