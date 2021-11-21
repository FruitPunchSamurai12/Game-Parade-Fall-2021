using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameStateMachine : MonoBehaviour
{
    public static event Action<IState> OnGameStateChanged;

    public static GameStateMachine Instance { get; private set; }
    private StateMachine _stateMachine;

    [SerializeField] int _numberOfExits = 3;
    public int CurrentExit { get; private set; }

    public Type CurrentStateType => _stateMachine.CurrentState.GetType();

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        CurrentExit = -1;
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
        var menu = new Menu();
        var loading = new LoadLevel();
        var play = new Play();
        var pause = new Pause();

        _stateMachine.SetState(menu);

        _stateMachine.AddTransition(menu, loading, () => PlayButton.LevelToLoad != null);
        _stateMachine.AddTransition(loading, play, loading.Finished);
       // _stateMachine.AddTransition(play, pause, () => Input.GetKeyDown(KeyCode.Escape));
       // _stateMachine.AddTransition(pause, play, () => Input.GetKeyDown(KeyCode.Escape));
        _stateMachine.AddTransition(pause, menu, () => BackToMenuButton.Pressed);
        _stateMachine.AddTransition(play, menu, () => BackToMenuButton.Pressed);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void SetNewExit()
    {
        int newExit = 0;
        do
        {
            newExit = UnityEngine.Random.Range(0, _numberOfExits);
        } while (newExit == CurrentExit);
        CurrentExit = newExit;
    }
}

public class Menu : IState
{
    public void OnEnter()
    {
        PlayButton.LevelToLoad = null;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}

public class Play : IState
{
    public void OnEnter()
    {
    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}

public class LoadLevel : IState
{
    public bool Finished() => _operations.TrueForAll(t => t.isDone);
    private List<AsyncOperation> _operations = new List<AsyncOperation>();
    public void OnEnter()
    {
        GameStateMachine.Instance.SetNewExit();
       _operations.Add(SceneManager.LoadSceneAsync(PlayButton.LevelToLoad));
       _operations.Add(SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive));
    }

    public void OnExit()
    {
        _operations.Clear();
    }

    public void Tick()
    {

    }
}

public class Pause : IState
{
    public static bool Active { get; private set; }
    public void OnEnter()
    {
        Time.timeScale = 0f;
        Active = true;
    }

    public void OnExit()
    {
        Time.timeScale = 1f;
        Active = false;
    }

    public void Tick()
    {

    }
}

