

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RepresentationButtonAction : MonoBehaviour
{
    public static RepresentationButtonAction instance { get; private set; }
    
    public float desiredDuration;
    
    private void Awake()
    {
        CreateSingleton();
    }
    
    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    public IEnumerator ChangeButtonPosition
        (GameObject button, Vector2 targetPosition, AnimationCurve curve)
    {
        float elapsed = 0f;
        RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
        Vector2 startPosition = buttonRectTransform.anchoredPosition;

        while (elapsed < desiredDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / desiredDuration);
            float curveT = curve.Evaluate(t);

            buttonRectTransform.anchoredPosition = Vector2.Lerp
                (startPosition, targetPosition, curveT);
            yield return null;
        }

        buttonRectTransform.anchoredPosition = targetPosition;
    }
    
    public void ButtonRendering(GameObject button, bool showButton)
    {
        button.GetComponent<Image>().enabled = showButton;
        button.GetComponent<Button>().enabled = showButton;
        button.transform.GetChild(0).gameObject.SetActive(showButton);
    }

}