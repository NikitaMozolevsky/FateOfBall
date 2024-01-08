using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/*public class PlatformGeneratorController : MonoBehaviour
{
    public static PlatformGeneratorController instance { get; private set; }
    private PlatformGeneratorService platformGeneratorService = PlatformGeneratorService.instance;
    public static Action onPlatformsGenerated;
    
    
    // Точка спавна новой платформы, постоянно перемещается.
    // Будет -2 по z т.к. должна переместится на новую точку.
    public Transform currentGenerationPoint;
    public GameObject originalPlatform; // Префаб платформы.
    
    // Позиции пеервых 2 платформ, ктороые всегда на одном месте
    private Vector3 firstPlatformVector;
    private Vector3 secondPlatformVector;
    
    
    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        CreateSingleton();
        PlatformGeneratorService.instance.PreparePlatformGenerator();
        PlatformGeneratorService.instance.EnablePlatformGeneration();
    }

    private void OnEnable()
    {
        ActionRestartButton.onRestartGame += PlatformGeneratorService.instance.PreparePlatformGenerator;
        ActionRestartButton.onRestartGame += PlatformGeneratorService.instance.EnablePlatformGeneration;
        GameController.onLose += PlatformGeneratorService.instance.DisablePlatformGeneration;
    }

    private void OnDisable()
    {
        ActionRestartButton.onRestartGame -= PlatformGeneratorService.instance.PreparePlatformGenerator;
        ActionRestartButton.onRestartGame -= PlatformGeneratorService.instance.EnablePlatformGeneration;
        GameController.onLose -= PlatformGeneratorService.instance.DisablePlatformGeneration;
    }

    private void Update()
    { // Постоянное управление поднятием о опусканием платформы.
        PlatformGeneratorService.instance.PlatformQueueManager();
    }
    
    public GameObject CreatePlatformObject(Vector3 spawnPosition)
    {
        GameObject newPlatform = Instantiate(originalPlatform, spawnPosition, Quaternion.identity);
        return newPlatform;
    }
    
    public void DeleteAllPlatforms()
    {
        foreach (var platform in platformGeneratorService.platformList)
        {
            Destroy(platform);
        }
    }
}*/
