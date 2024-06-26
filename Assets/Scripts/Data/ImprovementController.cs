using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ImprovementController", menuName = "Hackerman/Create Scriptable Improvement Controller")]
[Serializable]
public class ImprovementController : ScriptableObject
{
    public static Action OnCheckUnblockNextPerk;
    public static Action<ImprovementController> OnCheckUnlockInfoPerk;
    public static Action<ImprovementController> OnUpdateInfo;

    [InlineEditor(InlineEditorModes.FullEditor)]
    public ImprovementsData Data;
    
    [BoxGroup("Current Info Perk")]
    [ReadOnly] public int ImprovementLevel = 0;
    [BoxGroup("Current Info Perk")]
    [ReadOnly] public Resources Cost;
    [BoxGroup("Current Info Perk")]
    [ReadOnly] public Resources GenerateResources;

    [SerializeField]
    private bool _isUnlocked = false;
    public bool IsUnlocked
    {
        get => _isUnlocked;
        private set
        {
            IsUnlocked = value;
            if (value == true)
            {
                // it's hearing Canvas Manager
                OnCheckUnlockInfoPerk?.Invoke(this);
            }
        }
    }

    private void OnValidate()
    {
        if(Data != null)
            Init();
    }

    public void Init()
    {
        Cost = new Resources();
        GenerateResources = new Resources();
        Cost.CodeLines = Data.PriceValue.CodeLines;
        Cost.Bitcoin = Data.PriceValue.Bitcoin;
        ImprovementLevel = 0;
    }

    // Do I have enough resources to get it?
    public bool IsAvailable(Resources amount)
    {
        return
            amount.Bitcoin >= Cost.Bitcoin   &&
            amount.CodeLines >= Cost.CodeLines;
    }

    // Is info blocked?
    public bool CheckUnlockInfo(Resources amount)
    {
        bool EnoughBitcoin = amount.Bitcoin >= (Data.PriceValue.Bitcoin * Data.PercentageToUnlock);
        bool EnoughCodeLines = amount.CodeLines >= (Data.PriceValue.CodeLines * Data.PercentageToUnlock);

        return IsUnlocked = EnoughBitcoin && EnoughCodeLines;
    }


    public void IncreasedLevel()
    {
        GameManager.Instance.ReduceResources(Cost);
        ++ImprovementLevel;
        //GenerateResources = Data.GeneratedResources * ImprovementLevel;
        Cost.CodeLines += (int)(Cost.CodeLines * Data.IncreaseByLevel);
        Cost.Bitcoin += (int)(Cost.Bitcoin * Data.IncreaseByLevel);


        if (ImprovementLevel == 1) OnCheckUnblockNextPerk?.Invoke();

        // Just called at lvl 1
        if (Data.JustInLvl1) Data.SpecialEffect?.Invoke();
        // Called everytime
        else Data.SpecialEffect?.Invoke();

        if (Data.IsGeneratePerClick)
        {
            GenerateResources = Data.GeneratedResourcesPerClick * ImprovementLevel;
            GameManager.Instance.UpdateResourcesPerClick();
        }
        else
        {
            GenerateResources = Data.GeneratedResources * ImprovementLevel;
            GameManager.Instance.UpdateResources();
        }
        OnUpdateInfo?.Invoke(this);
        //TODO : Call to update info
        //TODO : Update perks
    }

    public void SetLevelFromSavedData(int lvlToReach)
    {
        ImprovementLevel = lvlToReach;
        Cost.CodeLines = Data.PriceValue.CodeLines;
        Cost.Bitcoin = Data.PriceValue.Bitcoin;

        for (int i = 0; i < lvlToReach; ++i)
        {
            Cost.CodeLines += (int)(Cost.CodeLines * Data.IncreaseByLevel);
            Cost.Bitcoin += (int)(Cost.Bitcoin * Data.IncreaseByLevel);
        }

        if (Data.IsGeneratePerClick)
        {
            GenerateResources = Data.GeneratedResourcesPerClick * ImprovementLevel;
        }
        else
        {
            GenerateResources = Data.GeneratedResources * ImprovementLevel;
        }
    }
}
