

using System;
using UnityEngine;

public class RepresentationSoundButton : MonoBehaviour
{

    public Vector2 showSoundButtonPosition = new Vector2(-100, -280);
    public Vector2 hideSoundButtonPosition = new Vector2(100, -280);

    private void SubscribeEvents()
    {
        ActionPlayButton.onPlay += HideSoundButton;
        ActionPauseButton.onPauseGame += SetShowPositionSoundButton;
        ActionContinueButton.onContinueGame += SetHidePositionSoundButton;
        GameController.onLose += ShowSoundButton;
    }

    private void UnsubscribeEvents()
    {
        ActionPlayButton.onPlay -= HideSoundButton;
        ActionPauseButton.onPauseGame -= SetShowPositionSoundButton;
        ActionContinueButton.onContinueGame -= SetHidePositionSoundButton;
        GameController.onLose -= ShowSoundButton;
    }
    
    private void Start()
    {
        SubscribeEvents();
    }
    
    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    private void HideSoundButton()
    {
        StartCoroutine(RepresentationButton.MoveElementPosition
            (gameObject, hideSoundButtonPosition, CanvasManager.instance.hideElementCurve));

    }

    private void ShowSoundButton()
    {
        StartCoroutine(RepresentationButton.MoveElementPosition
            (gameObject, showSoundButtonPosition, CanvasManager.instance.showElementCurve));
    }

    private void SetShowPositionSoundButton()
    {
        RepresentationButton.ChangeElementPosition(gameObject, showSoundButtonPosition);
    }

    private void SetHidePositionSoundButton()
    {
        RepresentationButton.ChangeElementPosition(gameObject, hideSoundButtonPosition);
    }
    
}