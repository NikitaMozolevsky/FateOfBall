

using System.Collections;
using UnityEngine;

public class CameraService
{
    private static CameraService _instance;
    private CameraController cameraController = CameraController.instance;
    
    private float currentHue = 0;
    private float RGBChangeInterval = 0.01f;

    public static CameraService instance
    {
        get
        {
            if (_instance == null)
            {
                
                _instance = new CameraService();
            }
            return _instance;
        }
    }

    public IEnumerator ColorChanger()
    {
        foreach (Color targetColor in cameraController.colors)
        {
            float t = 0f;
            Color startColor = Camera.main.backgroundColor;

            while (t < 1f && CameraController.colorChangerActive)
            {
                t += Time.deltaTime / cameraController.transitionDuration;
                Camera.main.backgroundColor = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }

            // Ждем перед следующим цветом
            yield return new WaitForSeconds(1f);
        }
    }
}