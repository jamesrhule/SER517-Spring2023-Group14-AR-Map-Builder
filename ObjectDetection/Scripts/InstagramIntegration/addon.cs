using UnityEngine;
using System.IO;

public class ShareToInstagram : MonoBehaviour
{
    public void ShareImageToInstagram(Texture2D texture)
    {
        byte[] pngData = texture.EncodeToPNG();
        string filePath = Path.Combine(Application.temporaryCachePath, "sharedImage.png");
        File.WriteAllBytes(filePath, pngData);

        
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "image/*");

        
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", filePath);
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

        
        intentObject.Call<AndroidJavaObject>("setPackage", "com.instagram.android");

        
        AndroidJavaObject activityObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activityObject.Call("startActivity", intentObject);
    }
}
