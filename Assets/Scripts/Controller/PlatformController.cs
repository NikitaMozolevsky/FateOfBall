using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformController : MonoBehaviour
{
    public static PlatformController instance { get; private set; }
    private PlatformRemoveService prs = PlatformRemoveService.instance;
    private PlatformGenerationService pgs = PlatformGenerationService.instance;
    
    // Платформа начала падение.
    public static UnityAction onDropPlatform;

    // Плоатформы которые поднимаются.
    //public List<GameObject> raisingPlatformList = new(); // Очередь из платформ
    // Платформы которые подняты.
    public List<GameObject> raisedPlatformList = new(); // Очередь из платформ
    // Плоатформы которые сброшены.
    public List<GameObject> droppedPlatformList = new(); // Очередь из платформ
    
    [NonSerialized] public GameObject firstPlatform;
    public Transform parentTransform;
    public AnimationCurve raiseCurve;
    public AnimationCurve dropCurve;

    // Точка спавна новой платформы, постоянно перемещается.
    // Будет -2 по z т.к. должна переместится на новую точку.
    [NonSerialized] public Transform currentPlatformGenerationPoint;
    // Точка от которой начианется генерация платформ в начале игры.
    public Transform startPlatformGenerationPoint;
    // Префаб платформы.
    public GameObject originalPlatform;

    // Позиции пеервых 2 платформ, ктороые всегда на одном месте
    private Vector3 firstPlatformVector;
    private Vector3 secondPlatformVector;

    private PlatformController()
    {
    } // Для Singleton
    
    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void SubscribeEvents()
    {
        GameController.onStartGame += pgs.CreateCurrentGenerationPoint;
        GameController.onStartGame += GenerateFirstPlatforms;
        ActionPlayButton.onPlay += RemoveFirstPlatform;
        SphereController.onBallCollision += CollisionWithPlatform;
        onDropPlatform += GenerateAndRaisePlatform;
        GameController.onLose += DropAllRaisedPlatforms;
        GameController.onLose += AssignAllRaisedPlatfornsAsDropped;
        GameController.afterRestartGame += ClearDroppedList;
        GameController.afterRestartGame += pgs.SetGenerationPointToStartPosition;;
        GameController.afterRestartGame += GenerateFirstPlatforms;
    }

    private void UnsubscribeEvents()
    {
        GameController.onStartGame -= pgs.CreateCurrentGenerationPoint;
        GameController.onStartGame -= GenerateFirstPlatforms;
        ActionPlayButton.onPlay -= RemoveFirstPlatform;
        SphereController.onBallCollision -= CollisionWithPlatform;
        onDropPlatform -= GenerateAndRaisePlatform;
        GameController.onLose -= DropAllRaisedPlatforms;
        GameController.onLose -= AssignAllRaisedPlatfornsAsDropped;
        GameController.afterRestartGame -= ClearDroppedList;
        GameController.afterRestartGame -= pgs.SetGenerationPointToStartPosition;
        GameController.afterRestartGame -= GenerateFirstPlatforms;
    }

    private void Awake()
    {
        CreateSingleton();
        SubscribeEvents();
    }

    private void Start()
    {
        // Генерирует первые платформы, которые поддерживают постоянное кол-во.
        GenerateFirstPlatforms();
    }

    private void Update()
    {
        InvisibleDroppedPlatformDestroyer();
    }

    private void OnApplicationQuit()
    {
        UnsubscribeEvents();
    }

    public GameObject CreatePlatformObject(Vector3 spawnPosition)
    {
        return pgs.CreatePlatformObject(spawnPosition, originalPlatform, parentTransform);
    }

    // Убирает первую платформу.
    private void RemoveFirstPlatform()
    {
        prs.RemovePlatformManually(firstPlatform);
    }

    // Метод генерирует и поднимает одну платформу.
    private void GenerateAndRaisePlatform()
    {
        pgs.GenerateAndRaisePlatform(raisedPlatformList);
    }

    // Вызывается при поражении. Роняет все поднятые платформы и которые видны.
    // Принимает в качестве параметра список с поднимаемыми и поднятыми платформами.
    // Уроненые платформа уже должны падать.
    // Роняет с рандомной задержкой используя IEnumerator.
    private void DropAllRaisedPlatforms()
    {
        prs.DropAllRaisedPlatfromsWithRandomDelay(raisedPlatformList);
    }

    // Вызывается при поражении, после фактического сброса всех платформ.
    // Перемещает все платформы в список сброшенных для возможности их удаления.
    private void AssignAllRaisedPlatfornsAsDropped()
    {
        prs.AssignAllRaisedPlatfornsAsDropped(raisedPlatformList, droppedPlatformList);
    }

    private void CollisionWithPlatform(Collision collision)
    {
        prs.CollisionWithPlatform(collision, raisedPlatformList, droppedPlatformList);
    }

    // Генерирует первые платформы, которые поддерживают постоянное кол-во.
    private void GenerateFirstPlatforms()
    {
        StartCoroutine(pgs.GenerateFirstPlatforms(raisedPlatformList));
    }

    private void InvisibleDroppedPlatformDestroyer()
    {
        prs.InvisibleDroppedPlatformDestroyer(droppedPlatformList);
    }

    private void ClearDroppedList()
    {
        prs.ClearDroppedList(droppedPlatformList);
    }
} 