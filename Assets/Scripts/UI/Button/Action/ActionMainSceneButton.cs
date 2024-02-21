

using UnityEngine;
using UnityEngine.Events;

public class ActionMainSceneButton : MonoBehaviour
{
    public static UnityAction<Camera> onBackToMainScene;

    public void Back()
    {
        onBackToMainScene?.Invoke(CameraController.instance.mainCamera);
    }
}