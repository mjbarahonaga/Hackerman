using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovementsManager : MonoBehaviour
{
    public static Action<ImprovementController, bool> OnImprovementAvailable;
    public static Action<ImprovementController> OnAddCanvas;
    public List<ImprovementController> ImprovementsBlocked;
    public List<ImprovementController> ImprovementsAvailable;
    //public List<ImprovementController> ImprovementsLockedInfo;

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


        // it is heard by CanvasController
        OnImprovementAvailable?.Invoke(improvement, improvement.IsAvailable(resources));
        
    }

    //public void CheckUnlockInfoImprovements(Resources resources)
    //{
    //    int length = ImprovementsLockedInfo.Count;
    //    ImprovementController tmp = null;
    //    for (int i = 0; i < length; ++i)
    //    {
    //        tmp = ImprovementsLockedInfo[i];
    //        if (tmp.IsUnlocked)
    //        {
    //            if (tmp.CheckUnlockInfo(resources))
    //            {
    //                ImprovementsLockedInfo.RemoveAt(i);
    //                --i;
    //                --length;
    //            }
    //        }
    //    }
    //}



    public void IncreasedImprovement(ImprovementController improvement, 
        Resources resources)
    {
        if(improvement.IsAvailable(resources))
            improvement.IncreasedLevel();
    }

    public Resources GeneratedResources()
    {
        Resources resources = new Resources();
        int length = ImprovementsAvailable.Count;
        for (int i = 0; i < length; ++i)
        {
            if(ImprovementsAvailable[i].ImprovementLevel > 0)
                resources += ImprovementsAvailable[i].GenerateResources;
        }

        return resources;
    }

    public Resources GeneratedPerClickResources()
    {
        Resources resources = new Resources();
        int length = ImprovementsAvailable.Count;
        for (int i = 0; i < length; ++i)
        {
            if (ImprovementsAvailable[i].Data.IsGeneratePerClick)
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
            OnAddCanvas?.Invoke(ImprovementsBlocked[0]);
            // Maybe to add to CanvasManager
            //ImprovementsLockedInfo.Add(ImprovementsBlocked[0]);
            ImprovementsBlocked.RemoveAt(0);
        }
    }

    
    // Testing
    public IEnumerator Start()
    {
        // Waiting to everything is initialized
        yield return new WaitForEndOfFrame();
        ImprovementsAvailable.Add(ImprovementsBlocked[0]);
        OnAddCanvas?.Invoke(ImprovementsBlocked[0]);
        ImprovementsBlocked.RemoveAt(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ImprovementsBlocked.Count == 0) return;
            ImprovementsAvailable.Add(ImprovementsBlocked[0]);
            OnAddCanvas?.Invoke(ImprovementsBlocked[0]);
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
