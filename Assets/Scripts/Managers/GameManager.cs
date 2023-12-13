using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static Action<int, Vector3> OnClick;
    // Singleton
    public static GameManager Instance { get; private set; }

    public float AutoSaveInSec = 60f;
    private float _currentSecs = 0f;

    public GameObject CanvasMenu;
    public GameObject CanvasGame;

    public ImprovementsManager RefImprovementsManager;
    public CanvasImprovementsManager RefCanvasManager;
    public GameObject RefCanvasMenu;
    public Button RefHackerInteraction;

    public AudioSource AudioClickController;
    public List<AudioClip> ClickClips = new List<AudioClip>();
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
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SaveGame();
        }
    }

    private void OnEnable()
    {
        _updateCoroutine = Timing.RunCoroutine(MyUpdate());
    }

    public void StartGame()
    {
        CanvasMenu.SetActive(false);
        var data = LoadGame();
        if(data == false)
        {
            var task = RefImprovementsManager.DefaultInit();
            task.Wait();
        }
        
        CanvasGame.SetActive(true);
    }

    public IEnumerator<float> MyUpdate()
    {
        while (true)
        {
            PlayerResources += _fractionGeneratedResources;
            //it checks if it's available to buy perks
            RefImprovementsManager.CheckChangeStateImprovements(PlayerResources);
            //RefImprovementsManager.CheckUnlockInfoImprovements(PlayerResources);
            if(_currentSecs >= AutoSaveInSec)
            {
                SaveGame();
                _currentSecs = 0f;
            }
            _currentSecs += FractionOfSeconds;
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
        ActiveSoundClick();
        OnClick?.Invoke((int)_generatedPerClickResources.CodeLines, Input.mousePosition);
    }

    public void ActiveSoundClick()
    {
        int index = UnityEngine.Random.Range(0, ClickClips.Capacity);
        AudioClickController.clip = ClickClips[index];
        AudioClickController.Play();
    }

    public void ReduceResources(Resources resources) => PlayerResources -= resources;

    public void MenuOptions()
    {
        if (RefCanvasMenu.activeSelf) //Deactivate
        {
            
            Resume();
            RefCanvasMenu.SetActive(false);
            RefHackerInteraction.enabled = true;
        }
        else //Activate
        {
            
            Pause();
            RefCanvasMenu.SetActive(true);
            RefHackerInteraction.enabled = false;
        }
    }

    public void CallSaveGame()
    {
        _currentSecs = 0f; // To avoid call it again while it is running
        SaveGame();
    }
    public Task SaveGame()
    {
        SaveData data = new SaveData();
        data.ImprovementsAvailable = RefImprovementsManager.ImprovementsAvailable;
        data.ImprovementsBlocked = RefImprovementsManager.ImprovementsBlocked;
        data.PlayerResources = PlayerResources;

        int length = RefImprovementsManager.ImprovementsAvailable.Count;
        for (int i = 0; i < length; ++i)
        {
            data.PerksLvl.Add(RefImprovementsManager.ImprovementsAvailable[i].ImprovementLevel);
        }
        
        SaveManager.SaveGameState(data);
#if UNITY_EDITOR
        Debug.Log("GUARDADO");
#endif
        return Task.CompletedTask;
    }

    public bool LoadGame()
    {
        SaveData data = new SaveData();
        data = SaveManager.LoadGameState();
        if (data == null)
        {
#if UNITY_EDITOR
            Debug.Log("NO CARGO DATOS");
#endif
            return false;
        }

        SetData(data).Wait();

        return true;
    }

    public Task SetData(SaveData data)
    {
        RefImprovementsManager.ImprovementsAvailable.Clear();
        RefImprovementsManager.ImprovementsBlocked.Clear();

        RefImprovementsManager.ImprovementsAvailable = data.ImprovementsAvailable;
        RefImprovementsManager.ImprovementsBlocked = data.ImprovementsBlocked;

        PlayerResources = data.PlayerResources;
        int length = RefImprovementsManager.ImprovementsAvailable.Count;
        for (int i = 0; i < length; ++i)
        {
            RefImprovementsManager.ImprovementsAvailable[i].SetLevelFromSavedData(data.PerksLvl[i]);
        }

        UpdateResourcesPerClick();
        UpdateResources();

        RefCanvasManager.UpdatePerks(RefImprovementsManager.ImprovementsAvailable);
#if UNITY_EDITOR
        Debug.Log("CARGO DATOS!!!");
#endif
        return Task.CompletedTask;
    }

  
    public void Pause() => Timing.PauseCoroutines(_updateCoroutine);

    public void Resume() => Timing.ResumeCoroutines(_updateCoroutine);

    public void ExitGame() => Application.Quit();
}
