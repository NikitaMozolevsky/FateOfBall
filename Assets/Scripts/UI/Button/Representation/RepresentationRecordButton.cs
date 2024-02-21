

using UnityEngine;

public class RepresentationRecordButton : MonoBehaviour
{
    public Vector2 showRecordButtonPosition = new Vector2(-100, -100);
    public Vector2 hideRecordButtonPosition = new Vector2(100, -100);

    private void SubscribeEvents()
    {
        ActionPlayButton.onPlay += HideRecordButton;
        ActionPauseButton.onPauseGame += SetShowPositionRecordButton;
        ActionContinueButton.onContinueGame += SetHidePositionRecordButton;
        GameController.onLose += ShowRecordButton;
    }

    private void UnsubscribeEvents()
    {
        ActionPlayButton.onPlay -= HideRecordButton;
        ActionPauseButton.onPauseGame -= SetShowPositionRecordButton;
        ActionContinueButton.onContinueGame -= SetHidePositionRecordButton;
        GameController.onLose -= ShowRecordButton;
    }
    
    private void Start()
    {
        SubscribeEvents();
    }
    
    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    private void HideRecordButton()
    {
        StartCoroutine(RepresentationButton.MoveElementPosition
            (gameObject, hideRecordButtonPosition, CanvasManager.instance.hideElementCurve));

    }

    private void ShowRecordButton()
    {
        StartCoroutine(RepresentationButton.MoveElementPosition
            (gameObject, showRecordButtonPosition, CanvasManager.instance.showElementCurve));
    }

    private void SetShowPositionRecordButton()
    {
        RepresentationButton.ChangeElementPosition(gameObject, showRecordButtonPosition);
    }

    private void SetHidePositionRecordButton()
    {
        RepresentationButton.ChangeElementPosition(gameObject, hideRecordButtonPosition);
    }
}