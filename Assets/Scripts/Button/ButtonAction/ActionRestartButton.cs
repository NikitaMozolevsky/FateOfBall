using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ActionRestartButton : MonoBehaviour
{
    // Вызывается при нажатии.
    public static UnityAction onRestartGame;
    // Вызывается (поменять на onNewGame)
    public static UnityAction afterRestartGame;
    
    // Метод отвечает за выполнение action после нажатия рестарта
    // и после перемещения камеры.
    public void Restart()
    {
        StartCoroutine(RestartIEnumerator());
    }

    private IEnumerator RestartIEnumerator()
    {
        onRestartGame?.Invoke();

        yield return new WaitForSeconds(CameraService.TIME_TO_SET_CAMERA_POSITION);
        // Вызов после перемещения камеры.
        afterRestartGame?.Invoke();
    }
    
}