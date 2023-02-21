using TensorFlow;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private TFSession session;
    private TFGraph graph;
    private TFSession.Runner runner;

    void Start()
    {
        graph = new TFGraph();
        var model = File.ReadAllBytes(Application.dataPath + "/yolov3.pb");
        graph.Import(model, "");
        session = new TFSession(graph);
        runner = session.GetRunner();
    }
}
