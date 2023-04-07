using UnityEngine;
using UnityEngine.UI;

public class VideoButton : MonoBehaviour
{
    public Button button;
    public Text lengthText;
    public Camera camera;
    public float maxDistance = 100f;

    private bool isMeasuring = false;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (!isMeasuring)
        {
            isMeasuring = true;
            button.GetComponentInChildren<Text>().text = "Finish Measuring";
            lengthText.text = "Measuring camera length...";
        }
        else
        {
            isMeasuring = false;
            button.GetComponentInChildren<Text>().text = "Measure Camera Length";
            float length = CalculateLength();
            if (length > 0)
            {
                lengthText.text = "Camera Length: " + length.ToString("F2") + " units";
            }
            else
            {
                lengthText.text = "Error: Could not measure camera length";
            }
        }
    }

    private float CalculateLength()
    {
        float length = 0f;
        RaycastHit hit;
        if (Physics.Raycast(startPos, camera.transform.forward, out hit, maxDistance))
        {
            float startDist = Vector3.Distance(startPos, hit.point);
            if (Physics.Raycast(endPos, camera.transform.forward, out hit, maxDistance))
            {
                float endDist = Vector3.Distance(endPos, hit.point);
                length = Mathf.Abs(endDist - startDist);
            }
        }
        return length;
    }

    private void Update()
    {
        if (isMeasuring)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = camera.ScreenToWorldPoint(Input.mousePosition + camera.transform.forward * 10f);
                startPos.z = 0f;
            }
            if (Input.GetMouseButton(0))
            {
                endPos = camera.ScreenToWorldPoint(Input.mousePosition + camera.transform.forward * 10f);
                endPos.z = 0f;
            }
        }
    }
}
