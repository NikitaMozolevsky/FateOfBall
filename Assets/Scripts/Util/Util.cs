


using Random = UnityEngine.Random;

public class Util
{

    private static Util _instance;
    
    public static Util instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Util();
            }

            return _instance;
        }
    }

    public bool GetRandomBool()
    {
        return Random.Range(0, 2) == 0;
    }

    public float GetRandomFloatInRange(float number)
    {
        return Random.Range(0f, number);
    }

}
