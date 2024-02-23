

using System;
using UnityEngine;
using Random = System.Random;

public class SoundController : MonoBehaviour
{
    public static SoundController instance { get; private set; }
    
    // Для воспроизведения звука.
    public AudioSource audioSource;
    // Звук движения кнопки.
    public AudioClip moveButtonSound;
    // Звук касания.
    public AudioClip changeDirectionSound;
    // Звук перехода к рекордам.
    public AudioClip toRecordsSound;
    // Звук перехода к главному меню
    public AudioClip toMainMenuSound;
    // Звуки поражения 2.
    public AudioClip firstLoseSound;
    public AudioClip secondLoseSound;
    // Звук нажатия на кнопку Play.
    public AudioClip playSound;
    // Звук удара о землю.
    public AudioClip knockingGroundSound;
    

    private SoundController()
    {
    }
    
    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }
    
    private void SubscribeEvents()
    {
        SphereController.onFirstBallCollision += MoveButtonSound;
        SphereController.onChangeSphereDirection += ChangeDirectionSound;
        ActionRecordButton.onShowRecords += RecordsSound;
        ActionMainSceneButton.onBackToMainScene += ToMainMenuSound;
        GameController.onLose += GameOverSound;
        ActionPlayButton.onPlay += PlayButtonSound;
        SphereController.onFirstBallCollision += KnockingGroundSound;
    }
    
    private void UnsubscribeEvents()
    {
        SphereController.onFirstBallCollision -= MoveButtonSound;
        SphereController.onChangeSphereDirection -= ChangeDirectionSound;
        ActionRecordButton.onShowRecords -= RecordsSound;
        ActionMainSceneButton.onBackToMainScene -= ToMainMenuSound;
        GameController.onLose -= GameOverSound;
        ActionPlayButton.onPlay -= PlayButtonSound;
        SphereController.onFirstBallCollision -= KnockingGroundSound;
    }

    private void Start()
    {
        CreateSingleton();
        SubscribeEvents();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    private void MoveButtonSound()
    {
        SoundService.PlaySound(moveButtonSound);
    }

    private void ChangeDirectionSound()
    {
        SoundService.PlaySound(changeDirectionSound);
    }

    private void RecordsSound(Camera arg0) //(заглушка)
    {
        SoundService.PlaySound(toRecordsSound);
    }

    private void ToMainMenuSound(Camera arg0) //(заглушка)
    {
        SoundService.PlaySound(toMainMenuSound);
    }

    private void GameOverSound()
    {
        SoundService.PlayRandomSound(firstLoseSound, secondLoseSound);
    }

    private void PlayButtonSound()
    {
        SoundService.PlaySound(playSound);
    }

    private void KnockingGroundSound()
    {
        SoundService.PlaySound(knockingGroundSound);
    }
}