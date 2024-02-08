using UnityEngine;
using UnityEngine.Events;

public class SphereController : MonoBehaviour
{
    // Единственный экземпляр синглтона
    public static SphereController instance { get; private set; }
    
    private SphereService sphereService = SphereService.instance;
    private GameService gameService = GameService.instance;
    private PlatformRemoveService prs = PlatformRemoveService.instance;
    private PlatformGenerationService pgs = PlatformGenerationService.instance;
    
    // Вызывается при каждом столкновении с шаром.
    public static UnityAction<Collision> onBallCollision;
    // Вызывается при первом столкновении с шаром.
    public static UnityAction onFirstBallCollision;
    
    public GameObject sphere;
    public Transform sphereStartPosition;
    // Звук касания.
    public AudioClip clickSound;

    private SphereController()
    {
    }
    
    private void CreateSingleton() // Создание экземпляра
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }
    
    private void OnEnable()
    {
        ActionRestartButton.afterRestartGame += SetSphereStartPosition;
        // Остановка и запуск времени.
        /*onFirstBallCollision += gameService.StopTime;
        ActionPlayButton.onPlay += gameService.ContinueTime;*/
    }

    private void OnDisable()
    {
        ActionRestartButton.afterRestartGame -= SetSphereStartPosition;
        /*onFirstBallCollision -= gameService.StopTime;
        ActionPlayButton.onPlay -= gameService.ContinueTime;*/
    }

    private void Awake()
    {
        CreateSingleton();
    }

    private void Update()
    {
        sphereService.ScreenTouchManager();
    }

    private void FixedUpdate() // Для плавности движения
    {
        MoveSphere();
    }
    
    // Столкновение шара с платформой.
    private void OnCollisionEnter(Collision collision)
    {
        sphereService.OnCollisionEnter(collision);
    }
    
    private void MoveSphere()
    {
        sphereService.MoveSphere(sphere);
    }
    
    // Возвращает шар на стартовую позицию с которой он падает.
    private void SetSphereStartPosition()
    {
        sphereService.SetSphereStartPosition(sphere, sphereStartPosition);
    }
}
