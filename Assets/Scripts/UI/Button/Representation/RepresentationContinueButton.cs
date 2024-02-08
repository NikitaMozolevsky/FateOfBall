using UnityEngine;

public class RepresentationContinueButton : MonoBehaviour
{
    private void OnEnable()
    {
        ActionPauseButton.onPauseGame += ShowContinueButton;
        ActionContinueButton.onContinueGame += HideCountinueButton;
    }

    private void OnDisable()
    {
        ActionPauseButton.onPauseGame += ShowContinueButton;
        ActionContinueButton.onContinueGame += HideCountinueButton;
    }

    private void ShowContinueButton()
    {
        RepresentationButtonAction.instance.ButtonRendering(gameObject, true);
    }

    private void HideCountinueButton()
    {
        RepresentationButtonAction.instance.ButtonRendering(gameObject, false);
    }
}