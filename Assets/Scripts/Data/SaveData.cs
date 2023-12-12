using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public List<ImprovementController> ImprovementsBlocked;
    public List<ImprovementController> ImprovementsAvailable;

    public Resources PlayerResources;
}
