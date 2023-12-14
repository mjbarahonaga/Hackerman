using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ImprovementsManager : MonoBehaviour
{
    [SerializeReference]
    public static Action<ImprovementController, bool> OnImprovementAvailable;
    [SerializeReference]
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
        Resources resources = new Resources(); // Default
        int length = ImprovementsAvailable.Count;
        for (int i = 0; i < length; ++i)
        {
            if (ImprovementsAvailable[i].Data.IsGeneratePerClick)
                resources += ImprovementsAvailable[i].GenerateResources;
        }

        return resources.CodeLines > 0 ? resources : new Resources(1,0); // new Resources(1,0) default
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

    public Task DefaultInit()
    {
        // Waiting to everything is initialized
        int length = ImprovementsBlocked.Count;
        for (int i = 0; i < length; ++i)
        {
            ImprovementsBlocked[i].Init();
        }
        ImprovementsAvailable.Add(ImprovementsBlocked[0]);
        OnAddCanvas?.Invoke(ImprovementsBlocked[0]);
        ImprovementsBlocked.RemoveAt(0);
        return Task.CompletedTask;
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
