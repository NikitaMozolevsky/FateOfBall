using UnityEngine;

public class RepresentationContinueButton : MonoBehaviour
{
    private void SubscribeEvents()
    {
        ActionPauseButton.onPauseGame += ShowContinueButton;
        ActionContinueButton.onContinueGame += HideCountinueButton;
    }

    private void UnsubscribeEvents()
    {
        ActionPauseButton.onPauseGame += ShowContinueButton;
        ActionContinueButton.onContinueGame += HideCountinueButton;
    }
    
    private void Start()
    {
        SubscribeEvents();
    }
    
    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    private void ShowContinueButton()
    {
        RepresentationButton.instance.ButtonRendering(gameObject, true);
    }

    private void HideCountinueButton()
    {
        RepresentationButton.instance.ButtonRendering(gameObject, false);
    }
}