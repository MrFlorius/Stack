using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Events
    
    public event Action OnRestart;
    public event Action OnBlockPlaced;
    public event Action OnBlockFailed;

    #endregion
    
    public StackChip InitialCube;
    
    [Header("Objects to spawn")]
    public List<StackChip> Prefabs;
    
    [Header("StackChips' position restriction")] 
    public Bounds bounds;

    [Header("Grid cell size for StackCips' snapping")]
    public Vector3 grid;

    [Header("Popup animation on new game")]
    public AnimationCurve PopupAnimation;
    
    
    [HideInInspector] public int Score;
    [HideInInspector] public List<StackChip> Spawned;

    #region Getters

    public StackChip CurrentChip => Spawned.Last();
    public StackChip LastChip => Spawned[Spawned.Count - 2];
    public bool ReadyForNewGame => !Spawned.Any();

    #endregion

    private int _prefabIdx = 1;
    private Camera _camera;
    private Vector3 _initialCamPos;
    private GameState _state = GameState.Playing;
    
    private enum GameState
    {
        Playing,
        GameOver,
        PreparingForTheNewGame
    }
    
    private IEnumerator Restart(bool withAnimation = true)
    {
        _state = GameState.PreparingForTheNewGame;
        if (!ReadyForNewGame)
        {
            Colors.UpdateColors();

            foreach (var chip in Spawned)
            {
                chip.Drop();
            }

            yield return new WaitUntil(() => ReadyForNewGame);

            Spawned.Clear();
        }

        Spawned.Add(Instantiate(InitialCube));
        Spawned.Add(Instantiate(Prefabs[0]));
        
        foreach (var chip in Spawned)
        {
            chip.Renderer.material.color = Colors.GetColor();
        }

        if(withAnimation){
            #region Animation
        
        var initialPosition = Spawned.Select(chip => chip.transform.position).ToList();
        
        _camera.transform.position = _initialCamPos;
        _prefabIdx = 1;
        
        var journey = 0f;
        var duration = PopupAnimation[PopupAnimation.keys.Length - 1].time;

        while (journey <= duration)
        {
            journey += Time.deltaTime;

            for (var i = 0; i < Spawned.Count; i++)
                Spawned[i].transform.position = initialPosition[i] + Vector3.up * PopupAnimation.Evaluate(journey);
            yield return null;
        }
        
        #endregion
        }
        _state = GameState.Playing;
    }

    
    #region Unity Callbacks

    private void OnValidate()
    {
        bounds = bounds.GetXZ();
        //раскомментировать для ограничения значения правого кейфрейма анимации нулем 
        // var v = PopupAnimation[PopupAnimation.length - 1];
        // v.value = 0;
        // PopupAnimation.MoveKey(PopupAnimation.length - 1, v);
    }

    private void Awake()
    {
        Application.targetFrameRate = 120;
        
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    private void Start()
    {
        _camera = Camera.main;
        _initialCamPos = _camera.transform.position;
        
        
        OnBlockPlaced += Spawn;
        OnBlockPlaced += () => { Score += 1; };
        
        OnBlockFailed += () => { _state = GameState.GameOver; };

        OnRestart?.Invoke();
        StartCoroutine(Restart(false));
        OnRestart += () => { StartCoroutine(Restart()); };
    }
    
    private void Update()
    {
        switch (_state)
        {
            case GameState.Playing:
                if (Input.GetMouseButtonDown(0))
                    if (CurrentChip.Stop() != StackChip.Hit.NoHit)
                        OnBlockPlaced?.Invoke();
                    else
                        OnBlockFailed?.Invoke();
                break;
            
            case GameState.GameOver:
                if (Input.GetMouseButtonDown(0))
                    OnRestart?.Invoke();
                break;
            case GameState.PreparingForTheNewGame:
                
                break;
        }
    }

    #endregion

    private void Spawn()
    {
        var c = Instantiate(Prefabs[_prefabIdx]);

        _prefabIdx += 1;
        if(_prefabIdx > Prefabs.Count - 1) _prefabIdx = 0;
        
        Spawned.Add(c);

        c.transform.localScale = LastChip.transform.localScale;

        var yShift = LastChip.Collider.bounds.extents.y * 2;

        var oldPos = LastChip.transform.position;
        var newPos = c.transform.position;
        
        c.transform.position = new Vector3(newPos.x + oldPos.x, yShift + oldPos.y, newPos.z + oldPos.z);

        if (Spawned.Count > 10)
        {
            _camera.transform.position += Vector3.up * yShift;
        }

        c.Renderer.material.color = Colors.GetColor();
    }
}
