using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public List<ImprovementController> ImprovementsBlocked = new List<ImprovementController>();
    public List<ImprovementController> ImprovementsAvailable = new List<ImprovementController>();
    public List<int> PerksLvl = new List<int>();
    public Resources PlayerResources;
}
