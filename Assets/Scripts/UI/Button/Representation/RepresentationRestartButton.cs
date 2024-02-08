

using UnityEngine;

public class RepresentationRestartButton : MonoBehaviour
{
    private GameService GameService = GameService.instance;
    
    private RectTransform buttonRectTransform;
    private Vector2 targetShowRestartPosition;
    private Vector2 targetHideRestartPosition;

    public AnimationCurve hideCurve;
    public AnimationCurve showCurve;
    
    private float targetShowXPosition = -100f; // Позиция видимости кнопки Restart
    private float targetHideXPosition = 100f; // Позиция сокрытости кнопки Restart
    
    private void OnEnable()
    {
        ActionRestartButton.onRestartGame += HideRestartButton;
        GameService.onLose += ShowRestartButton;
        
    }
    private void OnDisable()
    {
        
        ActionRestartButton.onRestartGame -= HideRestartButton;
        GameService.onLose -= ShowRestartButton;

    }
    
    private void Start()
    {
        buttonRectTransform = GetComponent<RectTransform>();
        targetShowRestartPosition = new Vector2
            (targetShowXPosition, buttonRectTransform.anchoredPosition.y);
        targetHideRestartPosition = new Vector2
            (targetHideXPosition, buttonRectTransform.anchoredPosition.y);
    }

    private void ShowRestartButton()
    { // Показывает кнопку restart из-за канваса.
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
            (gameObject, targetShowRestartPosition, showCurve));
    }
    
    // Сокрытие кнопки Restart при нажатии.
    // Временное отключение.
    private void HideRestartButton()
    { 
        // Отключение кнопки на 1 секунду, что бы не мешала игре.
        StartCoroutine(RepresentationButtonAction.instance.TemporarilyDeactivateButton
            (gameObject));
        // Скрывает кнопку restart за канвас.
        StartCoroutine(RepresentationButtonAction.instance.ChangeButtonPosition
            (gameObject, targetHideRestartPosition, showCurve));
    }
}