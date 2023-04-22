using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MapsTest
{
    [UnityTest]
    public IEnumerator MapsOpen()
    {
        // Define the location to be searched in Google Maps
        string location = "San Francisco, CA";

        // Define the Google Maps URL for the location
        string mapsURL = "https://www.google.com/maps/search/?api=1&query=" + location;

        // Open the maps URL in the default web browser
        Application.OpenURL(mapsURL);

        // Wait for the URL to load in the web browser
        yield return new WaitForSecondsRealtime(3);

        // Get the active window title of the web browser
        string activeWindowTitle = NativeMethods.GetActiveWindowTitle();

        // Assert that the active window title contains the location name
        Assert.IsTrue(activeWindowTitle.Contains(location));
    }
}

public static class NativeMethods
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

    public static string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }

        return null;
    }
}
