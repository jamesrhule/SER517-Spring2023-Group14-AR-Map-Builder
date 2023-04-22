using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GalleryTest
{
    [UnityTest]
    public IEnumerator GalleryOpen()
    {
        // Create a new GameObject with the GalleryOpener script attached
        GameObject galleryOpenerObject = new GameObject();
        GalleryOpener galleryOpener = galleryOpenerObject.AddComponent<GalleryOpener>();

        // Create a new Button component and assign it to the galleryButton variable in the GalleryOpener script
        Button button = galleryOpenerObject.AddComponent<Button>();
        galleryOpener.galleryButton = button;

        // Call the Start() function on the GalleryOpener script
        galleryOpener.Start();

        // Simulate a click on the galleryButton
        button.onClick.Invoke();

        // Wait for the gallery to open
        yield return new WaitForSecondsRealtime(3);

        // Check if the gallery is open by checking if the device is busy picking media
        bool isGalleryOpen = NativeGallery.IsMediaPickerBusy();

        // Assert that the gallery is open
        Assert.IsTrue(isGalleryOpen);
    }
}
