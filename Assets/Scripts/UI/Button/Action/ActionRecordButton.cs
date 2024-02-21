

using UnityEngine;
using UnityEngine.Events;

public class ActionRecordButton : MonoBehaviour
{
    public static UnityAction<Camera> onShowRecords;

    public void ShowRecords()
    {
        onShowRecords?.Invoke(CameraController.instance.recordsCamera);
    }
}