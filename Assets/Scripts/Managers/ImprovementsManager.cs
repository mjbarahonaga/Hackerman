using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovementsManager : MonoBehaviour
{
    public List<ImprovementController> ImprovementsBlocked;
    public List<ImprovementController> ImprovementsAvailable;

    public void IncreasedImprovement(ImprovementController improvement)
    {
        improvement.IncreasedLevel();
    }
}
