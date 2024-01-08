

using System;
using UnityEngine;

public class SaveLoadColor 
{

    private static SaveLoadColor _instance;
    
    public static SaveLoadColor instance
    {
        get
        {
            if (_instance == null)
            {
                
                _instance = new SaveLoadColor();
            }
            return _instance;
        }
    }

    public void SaveColor(ColorData color)
    {
        string json = JsonUtility.ToJson(color);
        PlayerPrefs.SetString("BackgroundColor", json);
        PlayerPrefs.Save();
    }

    public ColorData LoadColor()
    {
        string json = PlayerPrefs.GetString("BackgroundColor", "");
        ColorData loadColor;
        if (json == "")
        {
            return null;
        }
        loadColor = JsonUtility.FromJson<ColorData>(json);
        return loadColor;
    }
}