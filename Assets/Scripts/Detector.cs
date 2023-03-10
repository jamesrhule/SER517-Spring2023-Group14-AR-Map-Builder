using System;
using Barracuda;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class Detector : MonoBehaviour
{
    public NNModel modelFile;
    public TextAsset labelsFile;


    // for .nn model
    // private static int IMAGE_MEAN = 0;
    // private static float IMAGE_STD = 255f;
    // private const string INPUT_NAME = "input_1";
    // private const string OUTPUT_NAME = "conv2d_9/BiasAdd";


    // for .onnx model
    private const int IMAGE_MEAN = 0;
    private const float IMAGE_STD = 1f;
    private const string INPUT_NAME = "image";
    private const string OUTPUT_NAME = "grid";

    public const int IMAGE_SIZE = 416;
    
    // Minimum detection confidence to track a detection.
    private const float MINIMUM_CONFIDENCE = 0.3f;

    private IWorker worker;


    public const int ROW_COUNT = 13;
    public const int COL_COUNT = 13;
    public const int BOXES_PER_CELL = 5;
    public const int BOX_INFO_FEATURE_COUNT = 5;
    public const int CLASS_COUNT = 20;
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
        this.worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }


    // public IEnumerator Detect(Color32[] picture, System.Action<IList<BoundingBox>> callback)
    // {
    //     using (var tensor = TransformInput(picture, IMAGE_SIZE, IMAGE_SIZE))
    //     {
    //         var inputs = new Dictionary<string, Tensor>();
    //         inputs.Add(INPUT_NAME, tensor);
    //         yield return StartCoroutine(worker.ExecuteAsync(inputs));

    //         var output = worker.PeekOutput(OUTPUT_NAME);
    //         var results = ParseOutputs(output);
    //         var boxes = FilterBoundingBoxes(results, 5, MINIMUM_CONFIDENCE);

    //         callback(boxes);
    //     }
    // }
    public IList Detect(Texture2D texture, int numResultsPerClass = 1, float threshold = 0.2f, int angle = 0, Flip flip = Flip.NONE)
    {
        var shape = new TFShape(1, _inputWidth, _inputHeight, 3);
        var input = graph[_inputName][0];
        TFTensor inputTensor = null;

        if (input.OutputType == TFDataType.Float)
        {
            float[] imgData = Utils.DecodeTexture(texture, _inputWidth, _inputHeight,
                                                  _inputMean, _inputStd, angle, flip);
            inputTensor = TFTensor.FromBuffer(shape, imgData, 0, imgData.Length);
        }
        else if (input.OutputType == TFDataType.UInt8)
        {
            byte[] imgData = Utils.DecodeTexture(texture, _inputWidth, _inputHeight, angle, flip);
            inputTensor = TFTensor.FromBuffer(shape, imgData, 0, imgData.Length);
        }
        else
        {
            throw new Exception($"Input date type {input.OutputType} is not supported.");
        }

        var runner = session.GetRunner();
        runner.AddInput(input, inputTensor);

        IList results;

        if (_detectionModel == DetectionModels.SSD)
        {
            results = ParseSSD(runner, threshold, numResultsPerClass);
        }
        else
        {
            results = ParseYOLO(runner, threshold, numResultsPerClass);
        }

        inputTensor.Dispose();

        return results;
    }

    private IList ParseSSD(TFSession.Runner runner, float threshold, int numResultsPerClass)
    {
        runner.Fetch(graph["detection_boxes"][0],
                     graph["detection_classes"][0],
                     graph["detection_scores"][0],
                     graph["num_detections"][0]);

        var outputs = runner.Run();

        var boxes = outputs[0].GetValue() as float[,,];
        var classes = outputs[1].GetValue() as float[,];
        var scores = outputs[2].GetValue() as float[,];
        var num_detections = outputs[3].GetValue() as float[];

        foreach (var o in outputs) o.Dispose();

        var results = new List<Dictionary<string, object>>();
        var counters = new Dictionary<string, int>();

        for (int i = 0; i < (int)num_detections[0]; i++)
        {
            if (scores[0, i] < threshold) continue;

            string detectedClass = labels[(int)classes[0, i]];

            if (counters.ContainsKey(detectedClass))
            {
                if (counters[detectedClass] >= numResultsPerClass) continue;
                counters[detectedClass] += 1;
            }
            else
            {
                counters.Add(detectedClass, 1);
            }

            float ymin = Math.Max(0, boxes[0, i, 0]);
            float xmin = Math.Max(0, boxes[0, i, 1]);
            float ymax = boxes[0, i, 2];
            float xmax = boxes[0, i, 3];

            var rect = new Dictionary<string, float>
                {
                    { "x", xmin },
                    { "y", ymin },
                    { "w", Math.Min(1 - xmin, xmax - xmin) },
                    { "h", Math.Min(1 - ymin, ymax - ymin) }
                };

            var result = new Dictionary<string, object>
                {
                    { "rect", rect },
                    { "confidenceInClass", scores[0, i] },
                    { "detectedClass", detectedClass }
                };

            results.Add(result);
        }

        //Utils.Log(results);

        return results;
    }

    public static Tensor TransformInput(Color32[] pic, int width, int height)
    {
        float[] floatValues = new float[width * height * 3];

        for (int i = 0; i < pic.Length; ++i)
        {
            var color = pic[i];

            floatValues[i * 3 + 0] = (color.r - IMAGE_MEAN) / IMAGE_STD;
            floatValues[i * 3 + 1] = (color.g - IMAGE_MEAN) / IMAGE_STD;
            floatValues[i * 3 + 2] = (color.b - IMAGE_MEAN) / IMAGE_STD;
        }

        return new Tensor(1, height, width, 3, floatValues);
    }


    private IList<BoundingBox> ParseOutputs(Tensor yoloModelOutput, float threshold = .3F)
    {
        var boxes = new List<BoundingBox>();

        for (int cy = 0; cy < COL_COUNT; cy++)
        {
            for (int cx = 0; cx < ROW_COUNT; cx++)
            {
                for (int box = 0; box < BOXES_PER_CELL; box++)
                {
                    var channel = (box * (CLASS_COUNT + BOX_INFO_FEATURE_COUNT));
                    var bbd = ExtractBoundingBoxDimensions(yoloModelOutput, cx, cy, channel);
                    float confidence = GetConfidence(yoloModelOutput, cx, cy, channel);

                    if (confidence < threshold)
                    {
                        continue;
                    }

                    float[] predictedClasses = ExtractClasses(yoloModelOutput, cx, cy, channel);
                    var (topResultIndex, topResultScore) = GetTopResult(predictedClasses);
                    var topScore = topResultScore * confidence;

                    if (topScore < threshold)
                    {
                        continue;
                    }

                    var mappedBoundingBox = MapBoundingBoxToCell(cx, cy, box, bbd);
                    boxes.Add(new BoundingBox
                    {
                        Dimensions = new BoundingBoxDimensions
                        {
                            X = (mappedBoundingBox.X - mappedBoundingBox.Width / 2),
                            Y = (mappedBoundingBox.Y - mappedBoundingBox.Height / 2),
                            Width = mappedBoundingBox.Width,
                            Height = mappedBoundingBox.Height,
                        },
                        Confidence = topScore,
                        Label = labels[topResultIndex]
                    });
                }
            }
        }

        return boxes;
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


    private BoundingBoxDimensions ExtractBoundingBoxDimensions(Tensor modelOutput, int x, int y, int channel)
    {
        return new BoundingBoxDimensions
        {
            X = modelOutput[0, x, y, channel],
            Y = modelOutput[0, x, y, channel + 1],
            Width = modelOutput[0, x, y, channel + 2],
            Height = modelOutput[0, x, y, channel + 3]
        };
    }


    private float GetConfidence(Tensor modelOutput, int x, int y, int channel)
    {
        return Sigmoid(modelOutput[0, x, y, channel + 4]);
    }


    private CellDimensions MapBoundingBoxToCell(int x, int y, int box, BoundingBoxDimensions boxDimensions)
    {
        return new CellDimensions
        {
            X = ((float)y + Sigmoid(boxDimensions.X)) * CELL_WIDTH,
            Y = ((float)x + Sigmoid(boxDimensions.Y)) * CELL_HEIGHT,
            Width = (float)Math.Exp(boxDimensions.Width) * CELL_WIDTH * anchors[box * 2],
            Height = (float)Math.Exp(boxDimensions.Height) * CELL_HEIGHT * anchors[box * 2 + 1],
        };
    }


    public float[] ExtractClasses(Tensor modelOutput, int x, int y, int channel)
    {
        float[] predictedClasses = new float[CLASS_COUNT];
        int predictedClassOffset = channel + BOX_INFO_FEATURE_COUNT;

        for (int predictedClass = 0; predictedClass < CLASS_COUNT; predictedClass++)
        {
            predictedClasses[predictedClass] = modelOutput[0, x, y, predictedClass + predictedClassOffset];
        }

        return Softmax(predictedClasses);
    }


    private ValueTuple<int, float> GetTopResult(float[] predictedClasses)
    {
        return predictedClasses
            .Select((predictedClass, index) => (Index: index, Value: predictedClass))
            .OrderByDescending(result => result.Value)
            .First();
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


    private IList<BoundingBox> FilterBoundingBoxes(IList<BoundingBox> boxes, int limit, float threshold)
    {
        var activeCount = boxes.Count;
        var isActiveBoxes = new bool[boxes.Count];

        for (int i = 0; i < isActiveBoxes.Length; i++)
        {
            isActiveBoxes[i] = true;
        }

        var sortedBoxes = boxes.Select((b, i) => new { Box = b, Index = i })
                .OrderByDescending(b => b.Box.Confidence)
                .ToList();

        var results = new List<BoundingBox>();

        for (int i = 0; i < boxes.Count; i++)
        {
            if (isActiveBoxes[i])
            {
                var boxA = sortedBoxes[i].Box;
                results.Add(boxA);

                if (results.Count >= limit)
                    break;

                for (var j = i + 1; j < boxes.Count; j++)
                {
                    if (isActiveBoxes[j])
                    {
                        var boxB = sortedBoxes[j].Box;

                        if (IntersectionOverUnion(boxA.Rect, boxB.Rect) > threshold)
                        {
                            isActiveBoxes[j] = false;
                            activeCount--;

                            if (activeCount <= 0)
                                break;
                        }
                    }
                }

                if (activeCount <= 0)
                    break;
            }
        }

        return results;
    }
}


public class DimensionsBase
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Height { get; set; }
    public float Width { get; set; }
}


public class BoundingBoxDimensions : DimensionsBase { }

class CellDimensions : DimensionsBase { }


public class BoundingBox
{
    public BoundingBoxDimensions Dimensions { get; set; }

    public string Label { get; set; }

    public float Confidence { get; set; }

    public Rect Rect
    {
        get { return new Rect(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height); }
    }

    public override string ToString()
    {
        return $"{Label}:{Confidence}, {Dimensions.X}:{Dimensions.Y} - {Dimensions.Width}:{Dimensions.Height}";
    }
}