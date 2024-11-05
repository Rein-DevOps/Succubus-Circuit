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

    static GameManager s_instance;
    static GameManager Instance { get { Init(); return s_instance; } }
    public static GameState gameState = GameState.Unknown;

    CircuitManager _circuit = new CircuitManager();
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    
    UIManager _ui = new UIManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    StageManager _stage = new StageManager();
    
    public static CircuitManager Circuit { get { return Instance._circuit; } }
    public static DataManager Data { get { return Instance._data; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static InputManager Input {get {return Instance._input;}}
    public static UIManager UI { get { return Instance._ui; }}
    public static SoundManager Sound { get { return Instance._sound; }}
    public static StageManager Stage { get { return Instance._stage; }}

    
    void Start()
    {
        Init();
        Debug.Log($"gameState: {gameState}");
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject gameManager = GameObject.Find("@GameManager");

            if (gameManager == null)
            {
                gameManager  = new GameObject {name = "@GameManager"};
                gameManager.AddComponent<GameManager>();
            }
            DontDestroyOnLoad(gameManager);
            s_instance = gameManager.GetComponent<GameManager>();
            gameState = GameState.Menu;
        }
    }

    public static void Clear()
    {
        Circuit.Clear();
    }

    public static void SceneChange(Define.Scene scene = Define.Scene.Unknown, int stageNum = 0)
    {
        if (scene == Define.Scene.Menu)
        {
            SceneManager.LoadScene(0);
        }

        if (scene == Define.Scene.Game)
        {
            Stage.LoadStage(stageNum);
        }
    }

    public static void BgmSoundChange()
    {

    }
}
