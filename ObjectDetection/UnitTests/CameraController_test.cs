public class CameraControllerTests
{
    [Test]
    public void TestTakePhoto()
    {
        var cameraObj = new GameObject();
        var camera = cameraObj.AddComponent<Camera>();
        var photoDisplayObj = new GameObject();
        var photoDisplay = photoDisplayObj.AddComponent<RawImage>();
        var cameraController = cameraObj.AddComponent<CameraController>();
        cameraController.camera = camera;
        cameraController.photoDisplay = photoDisplay;

        cameraController.TakePhoto();
        Assert.IsNotNull(cameraController.photoDisplay.texture);
    }
}