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
	
	[Test]
    public IEnumerator swapCamera()
    {
        var cameraObj = new GameObject();
        var camera = cameraObj.AddComponent<Camera>();

        var frontCameraObject = new GameObject();
        var frontCamera = frontCameraObject.AddComponent<Camera>();
        frontCamera.tag = "MainCamera";

        var backCameraObject = new GameObject();
        var backCamera = backCameraObject.AddComponent<Camera>();

        yield return null;

        Assert.IsTrue(frontCamera.enabled);
        Assert.IsFalse(backCamera.enabled);

        Input.simulateTouchCount = 1;
        Input.simulateMouseWithTouches = true;
        Input.ResetInputAxes();
        yield return new WaitForSeconds(0.1f);

        // Verify that the back camera is now enabled
        Assert.IsFalse(frontCamera.enabled);
        Assert.IsTrue(backCamera.enabled);
    }
}