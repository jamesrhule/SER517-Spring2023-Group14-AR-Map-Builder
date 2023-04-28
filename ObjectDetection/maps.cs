using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {

    public Button mapButton;

    void Start() {
        mapButton.onClick.AddListener(OpenMap);
    }

    void OpenMap() {
        string url = "https://www.google.com/maps/search/?api=1&query=" + Input.location.lastData.latitude + "," + Input.location.lastData.longitude;
        Application.OpenURL(url);
    }
}
