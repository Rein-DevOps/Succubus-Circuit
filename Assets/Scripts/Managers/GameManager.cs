using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Define.Scene currScene;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                {
                    GameObject gameManager = new GameObject("@GameManager");
                    _instance = gameManager.AddComponent<GameManager>();
                    DontDestroyOnLoad(gameManager);
                }
            }
            return _instance;
        }
    }

    CircuitManager _circuit = new CircuitManager();
    InputManager _input; 
    UIManager _ui;
    ResourceManager _resource;
    SoundManager _sound;
    StageManager _stage;

    public static CircuitManager Circuit { get { return Instance._circuit; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static InputManager Input { get { return Instance._input; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static StageManager Stage { get { return Instance._stage; } }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize manager instances
            _circuit = new CircuitManager();
            _input = new InputManager();
            _ui = new UIManager();
            _resource = new ResourceManager();
            _sound = new SoundManager();
            _stage = new StageManager();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currScene = (Define.Scene) SceneManager.GetActiveScene().buildIndex;
        // Circuit.Init();
        Debug.Log($"CurrScene: {currScene}");
    }

    void Update()
    {
        _input.OnUpdate();
    }

    public static void Clear()
    {
        // Circuit.Clear();
    }

    public static void LoadScene(Define.Scene scene, int stageNum = 0)
    {
        if (scene == Define.Scene.Menu)
        {
            SceneManager.LoadScene((int) scene);
            currScene = Define.Scene.Menu;
        }
        else
        {
            Clear();
            SceneManager.LoadScene((int) scene);
            currScene = (Define.Scene)stageNum;
        }
    }
}
