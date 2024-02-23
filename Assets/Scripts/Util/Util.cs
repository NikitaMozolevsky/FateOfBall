


using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
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
    
    // Метод для поиска объектов с определенным компонентом
    public static void FindObjectsWithComponent<T>() where T : Component
    {
        // Найти все объекты в сцене
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();

        // Список для хранения объектов с определенным компонентом
        List<GameObject> objectsWithComponent = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Проверить, есть ли у объекта компонент типа T
            T component = obj.GetComponent<T>();

            if (component != null)
            {
                // Объект содержит нужный компонент, добавить его в список
                objectsWithComponent.Add(obj);
            }
        }

        if (objectsWithComponent.Count == 0)
        {
            Debug.Log($"Object with {typeof(T).Name} is MISTAKEN");
        }

        // Теперь objectsWithComponent содержит все объекты с нужным компонентом
        foreach (GameObject obj in objectsWithComponent)
        {
            Debug.Log($"Object with {typeof(T).Name}: {obj.name}");
        }
    }
}
