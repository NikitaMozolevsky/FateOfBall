


using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Util
{

    private static Util _instance;
    private Unity.Mathematics.Random random = new();
    
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
    }/*

    public static void RandomDelay(float seconds)
    {
        Delay(Random.Range(0f, seconds));
    }

    public static async void Delay(float seconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds));
    }*/

}
