


using UnityEngine;

public class Util
{

    private static Util _thisInstance;
    
    public static Util _instance
    {
        get
        {
            if (_thisInstance == null)
            {
                _thisInstance = new Util();
            }

            return _thisInstance;
        }
    }

}
