using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Unknown,
        Menu,
        Dialogue,
        Circuit,
    }

    private static GameManager s_instance;
    public static GameManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<GameManager>();
                if (s_instance == null)
                {
                    GameObject gameManager = new GameObject("@GameManager");
                    s_instance = gameManager.AddComponent<GameManager>();
                    DontDestroyOnLoad(gameManager);
                }
            }
            return s_instance;
        }
    }
    public static Define.Scene currScene = Define.Scene.Menu;

    CircuitManager _circuit;
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
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize manager instances
            _circuit = new CircuitManager();
            _input = new InputManager();
            _ui = new UIManager();
            _resource = new ResourceManager();
            _sound = new SoundManager();
            _stage = new StageManager();
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currScene = Define.Scene.Menu;
        Circuit.Init();
        Debug.Log($"CurrScene: {currScene}");
    }

    void Update()
    {
        _input.OnUpdate();
    }

    public static void Clear()
    {
        Circuit.Clear();
    }

    public static void SceneChange(Define.Scene scene = Define.Scene.Menu, int stageNum = 0)
    {
        if (scene == Define.Scene.Menu)
        {
            SceneManager.LoadScene(0);
            currScene = Define.Scene.Menu;
        }
        else
        {
            Clear();
            SceneManager.LoadScene((int)scene);
            currScene = (Define.Scene)stageNum;
        }
    }

    public static void BgmSoundChange(Define.Scene scene)
    {
        if (scene == currScene) return;

        if ((int)currScene > 0 && (int)scene > 0) return;

        if (scene == Define.Scene.Menu)
        {
            Sound.Play("Sounds/Bgm/MainBgm");
        }
        else
        {
            Sound.Play("Sounds/Bgm/CircuitBgm");
        }
    }
}
