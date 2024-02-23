

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RepresentationButton
{
    private static RepresentationButton _instance;

    // Время за которое кнопка проячется и показывается.
    public const float DURATION_OF_BUTTON_MOVEMENT = 1f;
    
    private RepresentationButton()
    {
    }

    // Статический метод для получения экземпляра синглтона
    public static RepresentationButton instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RepresentationButton();
            }
            return _instance;
        }
    }

    // Двигает элемент на канвасе.
    public static IEnumerator MoveElementPosition
        (GameObject element, Vector2 targetPosition, AnimationCurve curve)
    {
        float elapsed = 0f;
        RectTransform buttonRectTransform = element.GetComponent<RectTransform>();
        Vector2 startPosition = buttonRectTransform.anchoredPosition;

        while (elapsed < DURATION_OF_BUTTON_MOVEMENT)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / DURATION_OF_BUTTON_MOVEMENT);
            float curveT = curve.Evaluate(t);

            buttonRectTransform.anchoredPosition = Vector2.Lerp
                (startPosition, targetPosition, curveT);
            yield return null;
        }

        buttonRectTransform.anchoredPosition = targetPosition;
    }
    
    // Мгновенно перемещает элемент на Canvas.
    public static void ChangeElementPosition(GameObject element, Vector2 targetPosition)
    {
        RectTransform buttonRectTransform = element.GetComponent<RectTransform>();
        buttonRectTransform.anchoredPosition = targetPosition;
    }
    
    // Метод включения и отключения кнопки.
    public void ButtonRendering(GameObject button, bool showButton)
    {
        button.GetComponent<Image>().enabled = showButton;
        button.GetComponent<Button>().enabled = showButton;
        button.transform.GetChild(0).gameObject.SetActive(showButton);
    }

    // Временное отключение действия кнопки. Дабы не мешала игре.
    public IEnumerator TemporarilyDeactivateButton(GameObject button)
    {
        // Получаем RectTransform для объектов
        RectTransform buttonRect = button.GetComponent<RectTransform>();

        // Запоминаем исходные позиции
        int originalButtonIndex = buttonRect.GetSiblingIndex();

        // Перемещаем кнопку вниз
        buttonRect.SetAsFirstSibling();

        // Ждем заданное время
        yield return new WaitForSeconds(DURATION_OF_BUTTON_MOVEMENT);

        // Возвращаем кнопку на исходное место
        buttonRect.SetSiblingIndex(originalButtonIndex);
    }
}