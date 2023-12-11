using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<int, Vector3> OnClick;
    // Singleton
    public static GameManager Instance { get; private set; }

    public ImprovementsManager RefImprovementsManager;
    public CanvasImprovementsManager RefCanvasManager;

    [Range(0f, 1f)]
    public float FractionOfSeconds = 0.1f; // 1f / 10f - 10 times per second
    private Resources _fractionGeneratedResources = new Resources(0, 0);
    [SerializeField]
    private Resources _generatedPerClickResources = new Resources(1,0);

    private CoroutineHandle _updateCoroutine;
    private Resources _playerResources = new Resources();
    private Resources _generatedResources = new Resources();
    public Resources PlayerResources
    {
        get => _playerResources;
        private set
        {
            _playerResources = value;
            RefCanvasManager.UpdateCurrentResources(_playerResources);
        }
    }

    public Resources GeneratedResources // per secods
    {
        get => _generatedResources;
        private set
        {
            _generatedResources = value;
            RefCanvasManager.UpdateGenerateResources(_generatedResources);

        }
    }
    
    private void Awake()
    {
        //PlayerResources = new Resources(0, 0);
        _fractionGeneratedResources = new Resources(0, 0);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerResources.Bitcoin += 100;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            GeneratedResources.Bitcoin += 10;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerResources.CodeLines += 100;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GeneratedResources.CodeLines += 10;
        }
    }

    private void OnEnable()
    {
        _updateCoroutine = Timing.RunCoroutine(MyUpdate());
    }

    private void Start()
    {
        //Load game
        // or default init
        //RefImprovementsManager.DefaultInit();
    }

    public IEnumerator<float> MyUpdate()
    {
        while (true)
        {
            PlayerResources += _fractionGeneratedResources;

            //it checks if it's available to buy perks
            RefImprovementsManager.CheckChangeStateImprovements(PlayerResources);
            //RefImprovementsManager.CheckUnlockInfoImprovements(PlayerResources);

            yield return Timing.WaitForSeconds(FractionOfSeconds);
        }
    }

    public void CheckToBuy(ImprovementController perk)
    {
        RefImprovementsManager.IncreasedImprovement(perk, PlayerResources);
    }

    public void UpdateResourcesPerClick()
    {
        _generatedPerClickResources = RefImprovementsManager.GeneratedPerClickResources();
    }

    public void UpdateResources()
    {
        GeneratedResources = RefImprovementsManager.GeneratedResources();
        _fractionGeneratedResources = GeneratedResources * FractionOfSeconds;
    }

    public void ClickToHacker()
    {
        PlayerResources += _generatedPerClickResources;
        // Call feedback
        //Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        //var screenPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if(Physics.Raycast(screenPosition, out var hit))
            //OnClick?.Invoke((int)_generatedPerClickResources.CodeLines, hit.point);

        OnClick?.Invoke((int)_generatedPerClickResources.CodeLines, Input.mousePosition);


    }

    public void ReduceResources(Resources resources) => PlayerResources -= resources;

    public void Pause() => Timing.PauseCoroutines(_updateCoroutine);

    public void Resume() => Timing.ResumeCoroutines(_updateCoroutine);
}
