using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlRedirection : MonoBehaviour
{
 
	//TODO: update the URL string
	public void redirectURL()
	{
		Application.OpenURL("http://www.google.com");
	}
}
