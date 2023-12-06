using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Improvement", menuName = "Hackerman/Create Scriptable Improvement")]
public class ImprovementsData : ScriptableObject
{
    [BoxGroup("Logo", false)]
    [OnInspectorGUI("DrawLogo", append: true)]
    public Texture2D Logo;

    [BoxGroup("Resources", false)]
    public Resources GeneratedResources;

    [BoxGroup("PriceValue", false)]
    public Resources PriceValue;
    [BoxGroup("PriceValue", false)]
    [Range(0f,1f)]
    public float IncreaseByLevel = 0.1f; //Percentage

    [BoxGroup("PercentageToUnlock", false)]
    [Range(0f, 1f)]
    public float PercentageToUnlock = 0.1f; 



    private void DrawLogo()
    {
        if (this.Logo == null) return;

        GUILayout.BeginVertical(GUI.skin.label);
        GUILayout.Label(this.Logo);
        GUILayout.EndVertical();
    }
}
