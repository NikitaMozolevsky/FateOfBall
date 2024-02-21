

using TMPro;
using UnityEngine;

public class ScoreService
{
    
    private static ScoreService _instance;
    
    // Позиция в которой видно кол-во очков.
    private Vector2 currentSchoreShowPosition = new(-200, -100);
    // Позиция в которой не видно кол-во очков.
    private Vector2 currentSchoreHidePosition = new(-200, 150);
    // Позиция в которой видно кол-во очков.
    private Vector2 yourSchoreShowPosition = new(0, 0);
    // Позиция в которой не видно кол-во очков.
    private Vector2 yourSchoreHidePosition = new(-750, 0);
    
    public int currentSchoreCount = 0;
    private int[] scores = new int[5];

    private static string PP_1PLACE = "1place";
    private static string PP_2PLACE = "2place";
    private static string PP_3PLACE = "3place";
    private static string PP_4PLACE = "4place";
    private static string PP_5PLACE = "5place";

    private ScoreService()
    {
    }
    
    public static ScoreService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScoreService();
            }
            return _instance;
        }
    }

    // Добавляет очки к текущему значению при падении платформы если идет игра.
    public void AddSchore()
    {
        if (GameService.playCondition)
        {
            currentSchoreCount++;
        }
    }

    // Сбрасывает счетчик при рестарте.
    public void ResetCurrentCounter()
    {
        currentSchoreCount = 0;
    }

    public void UpdateCurrentSchoreCount(TextMeshProUGUI schoreCount)
    {
        schoreCount.text = currentSchoreCount.ToString();
    }

    // Устанавливает текущее кол-во очков при поражении.
    public void SetYourSchoreCount(TextMeshProUGUI tmp)
    {
        tmp.text = currentSchoreCount.ToString();
    }

    // Прячет текущее число очков.
    public void ShowCurrentSchore(TextMeshProUGUI tmp)
    {
        GameController.instance.StartCoroutine(RepresentationButton.MoveElementPosition
            (tmp.gameObject, currentSchoreShowPosition, CanvasManager.instance.showElementCurve));
    }

    // Прячет текущее число очков.
    public void HideCurrentSchore(TextMeshProUGUI tmp)
    {
        GameController.instance.StartCoroutine(RepresentationButton.MoveElementPosition
            (tmp.gameObject, currentSchoreHidePosition, CanvasManager.instance.hideElementCurve));
    }

    // Прячет конечное число очков.
    public void ShowYourSchore(Transform tmp)
    {
        GameController.instance.StartCoroutine(RepresentationButton.MoveElementPosition
            (tmp.gameObject, yourSchoreShowPosition, CanvasManager.instance.showElementCurve));
    }

    // Прячет конечное число очков.
    public void HideYourSchore(Transform tmp)
    {
        GameController.instance.StartCoroutine(RepresentationButton.MoveElementPosition
            (tmp.gameObject, yourSchoreHidePosition, CanvasManager.instance.hideElementCurve));
    }

    public void SetNewScore()
    {
        int insertIndex = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            if (currentSchoreCount > scores[i])
            {
                insertIndex = i;
                break;
            }
        }
        
        // Если число меньше всех в массиве, оно не вставляется
        if (insertIndex == -1)
        {
            return;
        }
        
        // Сдвиг чисел вниз для освобождения места
        for (int i = scores.Length - 1; i > insertIndex; i--)
        {
            scores[i] = scores[i - 1];
        }

        // Вставка нового числа
        scores[insertIndex] = currentSchoreCount;
    }
    
    public void GetScorePrefs()
    {
         scores[0] = PlayerPrefs.GetInt(PP_1PLACE);
         scores[1] = PlayerPrefs.GetInt(PP_2PLACE);
         scores[2] = PlayerPrefs.GetInt(PP_3PLACE);
         scores[3] = PlayerPrefs.GetInt(PP_4PLACE);
         scores[4] = PlayerPrefs.GetInt(PP_5PLACE);
    }
    
    public void SetScorePrefs()
    {
        PlayerPrefs.SetInt(PP_1PLACE, scores[0]);
        PlayerPrefs.SetInt(PP_2PLACE, scores[1]);
        PlayerPrefs.SetInt(PP_3PLACE, scores[2]);
        PlayerPrefs.SetInt(PP_4PLACE, scores[3]);
        PlayerPrefs.SetInt(PP_5PLACE, scores[4]);
    }

    public void SetTextToRecords
        (TextMeshProUGUI firstPlaceText, 
            TextMeshProUGUI secondPlaceText, 
            TextMeshProUGUI thirdPlaceText, 
            TextMeshProUGUI fourthPlaceText, 
            TextMeshProUGUI fifthPlaceText)
    {
        
        firstPlaceText.text = scores[0].ToString();
        secondPlaceText.text = scores[1].ToString();
        thirdPlaceText.text = scores[2].ToString();
        fourthPlaceText.text = scores[3].ToString();
        fifthPlaceText.text = scores[4].ToString();
    }
}