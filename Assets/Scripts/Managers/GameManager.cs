using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    public ImprovementsManager RefImprovementsManager;
    //public CanvasManager RefCanvasManager;

    [Range(0f, 1f)]
    public float FractionOfSeconds = 0.1f; // 1f / 10f - 10 times per second
    private Resources _fractionGeneratedResources = new Resources(0,0);

    private CoroutineHandle _updateCoroutine;
    private Resources _playerResources;
    private Resources _generatedResources;
    public Resources PlayerResources
    {
        get => _playerResources;
        private set
        {
            _playerResources = value;
            // TODO : Update Canvas Showing resources
        }
    }

    public Resources GeneratedResources // per secods
    {
        get => _generatedResources;
        private set
        {
            _generatedResources = value;
            // TODO : Update Canvas Showing resources generated
        }
    }
    
    private void Awake()
    {
        PlayerResources = new Resources(0, 0);
        _fractionGeneratedResources = new Resources(0, 0);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

    }

    private void OnEnable()
    {
        _updateCoroutine = Timing.RunCoroutine(MyUpdate());
    }

    public IEnumerator<float> MyUpdate()
    {
        while (true)
        {
            PlayerResources += _fractionGeneratedResources;

            //it checks if it's available to buy perks
            RefImprovementsManager.CheckChangeStateImprovements(PlayerResources);
            RefImprovementsManager.CheckUnlockInfoImprovements(PlayerResources);

            yield return Timing.WaitForSeconds(FractionOfSeconds);
        }
    }

    public void CheckToBuy(ImprovementController perk)
    {
        RefImprovementsManager.IncreasedImprovement(perk, PlayerResources);
    }

    public void UpdateResources()
    {
        GeneratedResources = RefImprovementsManager.GeneratedResources();
        _fractionGeneratedResources = GeneratedResources * FractionOfSeconds;
    }

    public void ReduceResources(Resources resources) => PlayerResources -= resources;

    public void Pause() => Timing.PauseCoroutines(_updateCoroutine);

    public void Resume() => Timing.ResumeCoroutines(_updateCoroutine);
}
