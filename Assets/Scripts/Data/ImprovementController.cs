using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovementController : MonoBehaviour
{
    public ImprovementsData Data;
    public int ImprovementLevel = 0;
    public Resources Cost;
    public Resources GenerateResources;

    public bool IsUnlocked = false; 

    public void Init()
    {
        Cost = Data.PriceValue;
        ImprovementLevel = 0;
    }

    // Do I have enough resources to get it?
    public bool IsAvailable(Resources amount)
    {
        return
            amount.Bitcoin >= Data.PriceValue.Bitcoin   &&
            amount.CodeLines >= Data.PriceValue.CodeLines;
    }

    // Is info blocked?
    public bool CheckUnlocked(Resources amount)
    {
        bool EnoughBitcoin = amount.Bitcoin >= (Data.PriceValue.Bitcoin * Data.PercentageToUnlock);
        bool EnoughCodeLines = amount.CodeLines >= (Data.PriceValue.CodeLines * Data.PercentageToUnlock);

        return IsUnlocked = EnoughBitcoin && EnoughCodeLines;
    }

    public void IncreasedLevel()
    {
        ++ImprovementLevel;
        GenerateResources = Data.GeneratedResources * ImprovementLevel;
        Cost += Cost * Data.IncreaseByLevel;
        
        //Call to update info 
    }
}
