public class InstagramController : MonoBehaviour {
    public InstagramApiWrapper instagramApiWrapper;

    void UploadToInstagram() {
        instagramApiWrapper.Authorize((bool success) => {
            if (success) {
                // Upload the photo to Instagram
                byte[] imageData = ScreenCapture.CaptureScreenshotAsTexture().EncodeToPNG();
                instagramApiWrapper.UploadPhoto(imageData, "My Unity Camera App", "", (InstagramMedia media) => {
                    Debug.Log("Photo uploaded to Instagram: " + media.Link);
                });
            } else {
                Debug.LogError("Failed to authenticate with Instagram");
            }
        });
    }
}
