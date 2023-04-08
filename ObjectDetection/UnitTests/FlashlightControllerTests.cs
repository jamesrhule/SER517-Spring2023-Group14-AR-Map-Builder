using NUnit.Framework;

public class FlashlightControllerTests
{
	//test for flash turn on
    public void FlashTurnsOn()
    {
        bool initialFlashState =false;
        testCamera.GetComponent<Flash>().ToggleFlash();
        bool flashStateAfterTurnedOn = testCamera.GetComponent<Flash>().ToggleFlashlight();
        Assert.IsTrue(flashStateAfterTurnedOn);
    }

	//test for flash turn off
	public void FlashTurnsOff()
    {
		bool initialFlashState = false;
        testCamera.GetComponent<Flash>().ToggleFlashlight();
        bool flashStateAfterTurnedOn = testCamera.GetComponent<Flash>().isFlashOn;
        testCamera.GetComponent<Flash>().ToggleFlashlight();
        bool flashStateAfterTurnedOff = testCamera.GetComponent<Flash>().isFlashOn;
        Assert.IsFalse(flashStateAfterTurnedOff);
    }

}