

public class LevelService
{
    private static LevelService _instance;

    private float[] sphereSpeedArray = { 0.2f, 0.2f };
    
    private LevelService()
    {
    }

    public static LevelService instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LevelService();
            }
            return _instance;
        }
    }
    
    public const int FIRST_LVL_SCORES = 100;
    public const int SECOND_LVL_SCORES = 200;
    public const int THIRD_LVL_SCORES = 300;
    public const int FOURTH_LVL_SCORES = 400;
    public const int FIFTH_LVL_SCORES = 500;
    
    public void LevelManager()
    {
        int currentScoreCount = ScoreService.instance.currentSchoreCount;

        if (currentScoreCount >= 0 && currentScoreCount <= FIRST_LVL_SCORES)
        {
            SetFirstLevel();
        }
        if (currentScoreCount >= FIRST_LVL_SCORES && currentScoreCount <= SECOND_LVL_SCORES)
        {
            SetSecondLevel();
        }
        if (currentScoreCount >= SECOND_LVL_SCORES && currentScoreCount <= THIRD_LVL_SCORES)
        {
            SetThirdLevel();
        }
        if (currentScoreCount >= THIRD_LVL_SCORES && currentScoreCount <= FOURTH_LVL_SCORES)
        {
            SetFourthLevel();
        }
        if (currentScoreCount >= FOURTH_LVL_SCORES && currentScoreCount <= FIFTH_LVL_SCORES)
        {
            SetFifthLevel();
        }
    }

    private void SetLevelPace(float sphereSpeed = 0.2f, float timeToRisePlatform = 1.5f, float timeToFallPlatform = 0.2f)
    {
        
    }

    private void SetFirstLevel()
    {
        
    }

    private void SetSecondLevel()
    {
        
    }

    private void SetThirdLevel()
    {
        
    }

    private void SetFourthLevel()
    {
        
    }

    private void SetFifthLevel()
    {
        
    }
}