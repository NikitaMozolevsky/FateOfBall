

using UnityEngine;

public class RepresentationRestartButton : MonoBehaviour
{
    private Vector2 targetShowRestartPosition;
    private Vector2 targetHideRestartPosition;
    private RectTransform buttonRectTransform;
    
    public AnimationCurve hideCurve;
    public AnimationCurve showCurve;
    
    private float targetShowXPosition = 330f; // Позиция видимости кнопки Restart
    private float targetHideXPosition = 500f; // Позиция сокрытости кнопки Restart
    
    private void OnEnable()
    {
        ActionRestartButton.onRestartGame += OnRestart;
        GameController.onLose += OnLose;
        
    }
    private void OnDisable()
    {
        
        ActionRestartButton.onRestartGame -= OnRestart;
        GameController.onLose -= OnLose;

    }
    
    private void Start()
    {
        buttonRectTransform = GetComponent<RectTransform>();
        targetShowRestartPosition = new Vector2
            (targetShowXPosition, buttonRectTransform.anchoredPosition.y);
        targetHideRestartPosition = new Vector2
            (targetHideXPosition, buttonRectTransform.anchoredPosition.y);
    }
    
    private void OnRestart()
    { // Скрывает кнопку restart за канвас
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
            (gameObject, targetHideRestartPosition, showCurve));
    }
    
    private void OnLose()
    { // Показывает кнопку restart из-за канваса
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
            (gameObject, targetShowRestartPosition, showCurve));
    }
    
}