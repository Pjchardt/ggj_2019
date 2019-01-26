using UnityEngine;
using System.Collections;
using System.IO;

public class CaptureMonoscopicScreenshot : MonoBehaviour
{
    public int superSize;
    Camera thisCamera;
    public RenderTexture targetTexture;

    void Awake()
    {
        thisCamera = GetComponent<Camera>();
    }

	void Update ()
    {
        if (Input.GetButton("Snapshot"))
        {
            thisCamera.targetTexture = targetTexture;
            float savedFOV = thisCamera.fieldOfView;
            thisCamera.fieldOfView = 60f;
            thisCamera.Render();
            RenderTexture.active = targetTexture;

            Texture2D screenshot = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(Rect.MinMaxRect(0, 0, screenshot.width, screenshot.height), 0, 0, false);
            
            byte[] bytes = screenshot.EncodeToPNG();
            Object.Destroy(screenshot);

            File.WriteAllBytes(Application.dataPath + "/Screenshot" + System.DateTime.Now.ToFileTime() + ".png", bytes);

            thisCamera.fieldOfView = savedFOV;
            thisCamera.targetTexture = null;
        }
	}

    /*public void CaptureScreenshot()
    {
        Application.CaptureScreenshot(Application.dataPath + "/Screenshot" + System.DateTime.Now.ToFileTime() + ".png", 4);
    }*/
}
