using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Unknown,
        Menu,
        Dialogue,
        Circuit,
        Test
    }

    static GameManager s_instance;
    static GameManager Instance { get { Init(); return s_instance; } }
    public static GameState gameState = GameState.Unknown;

    CircuitManager _circuit = new CircuitManager();
    InputManager _input = new InputManager();
    UIManager _ui = new UIManager();
    ResourceManager _resource = new ResourceManager();
    
    public static CircuitManager Circuit { get { return Instance._circuit; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static InputManager Input {get {return Instance._input;}}
    public static UIManager UI { get { return Instance._ui; }}
    
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

        gameState = GameState.Circuit;
    }

    public static void Clear()
    {
        Circuit.Clear();
    }
}
