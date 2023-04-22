using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GalleryOpener : MonoBehaviour
{
    public Button galleryButton;

    void Start()
    {
        // Add a listener to the galleryButton to open the gallery when it is clicked
        galleryButton.onClick.AddListener(OpenGallery);
    }

    void OpenGallery()
    {
        // Check if the device supports picking images from the gallery
        if (!NativeGallery.IsMediaPickerBusy())
        {
            // Open the gallery to pick an image
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                // Do something with the picked image
                StartCoroutine(LoadImage(path));
            }, "Pick an image", "image/*");
        }
    }

    IEnumerator LoadImage(string path)
    {
        // Load the picked image into a texture
        var www = new WWW("file://" + path);
        yield return www;

        // Create a sprite from the loaded texture and set it as the image on a UI Image component
        var texture = www.texture;
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        GetComponent<Image>().sprite = sprite;
    }
}
