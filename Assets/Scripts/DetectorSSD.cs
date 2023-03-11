using System;
using UnityEngine;
using Unity.Barracuda;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class Detectorssd2 : MonoBehaviour, Detector
{
    public NNModel modelFile;
    public TextAsset labelsFile;

    private const int IMG_M = 0;
    private const float IMG_STD = 255.0F;

    public string INPUT_NAME;
    public string OUTPUT_NAME;

    private const int _image_size = 416;
    public int IMG_SIZE { get => _image_size; }

    public float MINIMUM_CONFIDENCE;

    private IWorker worker;

    public const int ROW_COUNT = 13;
    public const int COL_COUNT = 13;
    public const int BOXES_PER_CELL = 5;
    public const int BOX_INFO_FEATURE_COUNT = 5;

    public int CLASS_COUNT;

    public const float CELL_WIDTH = 32;
    public const float CELL_HEIGHT = 32;
    private string[] labels;

    private float[] anchors = new float[]
    {
        1.08F, 1.19F, 3.42F, 4.41F, 6.63F, 11.38F, 9.42F, 5.11F, 16.62F, 10.52F
    };


    public void Start()
    {
        this.labels = Regex.Split(this.labelsFile.text, "\n|\r|\r\n")
            .Where(s => !String.IsNullOrEmpty(s)).ToArray();
        var model = ModelLoader.Load(this.modelFile);
        this.worker = GraphicsWorker.GetWorker(model);
    }


    public IEnumerator Detect(Color32[] picture, System.Action<IList<BoundingBox>> callback)
    {
        using (var tensor = TransformInput(picture, IMG_SIZE, IMG_SIZE))
        {
            var inputs = new Dictionary<string, Tensor>();
            inputs.Add(INPUT_NAME, tensor);
            yield return StartCoroutine(worker.StartManualSchedule(inputs));
            var output = worker.PeekOutput(OUTPUT_NAME);
            Debug.Log("Output: " + output);
            callback(boxes);
        }
    }


    public static Tensor TransformInput(Color32[] pic, int width, int height)
    {
        float[] floatValues = new float[width * height * 3];

        for (int i = 0; i < pic.Length; ++i)
        {
            var color = pic[i];

            floatValues[i * 3 + 0] = (color.r - IMG_M) / IMG_STD;
            floatValues[i * 3 + 1] = (color.g - IMG_M) / IMG_STD;
            floatValues[i * 3 + 2] = (color.b - IMG_M) / IMG_STD;
        }

        return new Tensor(1, height, width, 3, floatValues);
    }

    private float Sigmoid(float value)
    {
        var k = (float)Math.Exp(value);

        return k / (1.0f + k);
    }


    private float[] Softmax(float[] values)
    {
        var maxVal = values.Max();
        var exp = values.Select(v => Math.Exp(v - maxVal));
        var sumExp = exp.Sum();

        return exp.Select(v => (float)(v / sumExp)).ToArray();
    }

    private float IntersectionOverUnion(Rect boundingBoxA, Rect boundingBoxB)
    {
        var areaA = boundingBoxA.width * boundingBoxA.height;

        if (areaA <= 0)
            return 0;

        var areaB = boundingBoxB.width * boundingBoxB.height;

        if (areaB <= 0)
            return 0;

        var minX = Math.Max(boundingBoxA.xMin, boundingBoxB.xMin);
        var minY = Math.Max(boundingBoxA.yMin, boundingBoxB.yMin);
        var maxX = Math.Min(boundingBoxA.xMax, boundingBoxB.xMax);
        var maxY = Math.Min(boundingBoxA.yMax, boundingBoxB.yMax);

        var intersectionArea = Math.Max(maxY - minY, 0) * Math.Max(maxX - minX, 0);

        return intersectionArea / (areaA + areaB - intersectionArea);
    }

}
