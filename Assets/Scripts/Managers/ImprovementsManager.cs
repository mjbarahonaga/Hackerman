using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovementsManager : MonoBehaviour
{
    public static Action<ImprovementController, bool> OnImprovementAvailable;
    public List<ImprovementController> ImprovementsBlocked;
    public List<ImprovementController> ImprovementsAvailable;
    public List<ImprovementController> ImprovementsLockedInfo;

    public void CheckChangeStateImprovements(Resources resources)
    {
        int length = ImprovementsAvailable.Count;
        for (int i = 0; i < length; ++i)
        {
            AvailabilityImprovement(ImprovementsAvailable[i], resources);
        }
    }

    public void AvailabilityImprovement(ImprovementController improvement, 
    Resources resources)
    {

            // TODO : Change state on canvas
            // It has to be heared by CanvasManager
        OnImprovementAvailable?.Invoke(improvement, improvement.IsAvailable(resources));
        
    }

    public void CheckUnlockInfoImprovements(Resources resources)
    {
        int length = ImprovementsLockedInfo.Count;
        ImprovementController tmp = null;
        for (int i = 0; i < length; ++i)
        {
            tmp = ImprovementsLockedInfo[i];
            if (tmp.IsUnlocked)
            {
                if (tmp.CheckUnlockInfo(resources))
                {
                    ImprovementsLockedInfo.RemoveAt(i);
                    --i;
                    --length;
                }
            }
        }
    }



    public void IncreasedImprovement(ImprovementController improvement, 
        Resources resources)
    {
        if(improvement.IsAvailable(resources))
            improvement.IncreasedLevel();
    }

    public Resources GeneratedResources()
    {
        Resources resources = null;
        int length = ImprovementsAvailable.Count;
        for (int i = 0; i < length; ++i)
        {
            resources += ImprovementsAvailable[i].GenerateResources;
        }

        return resources;
    }

    // When a perk reaches level 1,
    // it checks if the next perk has to be unlocked
    public void CheckUnblockImprovement()
    {
        int index = ImprovementsAvailable.Count - 1;
        if (index < 0) return;
        if (ImprovementsAvailable[index].ImprovementLevel == 1)
        {
            // Move from blocked to available list, if it's empty, return
            if (ImprovementsBlocked.Count == 0) return;
            ImprovementsAvailable.Add(ImprovementsBlocked[0]);
            // Maybe to add to CanvasManager
            ImprovementsLockedInfo.Add(ImprovementsBlocked[0]);
            ImprovementsBlocked.RemoveAt(0);
        }
    }

    #region UNITY METHODS

    private void OnEnable()
    {
        ImprovementController.OnCheckUnblockNextPerk += CheckUnblockImprovement;
    }

    private void OnDisable()
    {
        ImprovementController.OnCheckUnblockNextPerk -= CheckUnblockImprovement;
    }

    #endregion
}
