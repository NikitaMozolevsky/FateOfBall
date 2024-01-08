


using UnityEngine;
using Random = System.Random;

public class Util
{

    private static Util _instance;
    private Random random = new();
    
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
        return random.Next(2) == 0;
    }

}
