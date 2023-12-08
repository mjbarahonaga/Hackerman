using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Improvement", menuName = "Hackerman/Create Scriptable Improvement")]
[Serializable]
public class ImprovementsData : ScriptableObject
{
    [BoxGroup("Name", false)]
    public string Name;

    [BoxGroup("Logo", false)]
    [OnInspectorGUI("DrawLogo", append: true)]
    public Texture2D Logo;

    [ReadOnly]
    public Sprite Sprite;
    
    [BoxGroup("Resources", false)]
    public Resources GeneratedResources;
    [BoxGroup("Resources", false)]
    [TextArea(1, 2)]
    public string InfoPerk = "";

    [BoxGroup("PriceValue", false)]
    public Resources PriceValue;
    [BoxGroup("PriceValue", false)]
    [Range(0f,1f)]
    public float IncreaseByLevel = 0.1f; //Percentage

    [BoxGroup("PercentageToUnlock", false)]
    [Range(0f, 1f)]
    public float PercentageToUnlock = 0.1f;

    [BoxGroup("SpecialEffect", false)]
    public UnityEvent SpecialEffect;
    [BoxGroup("SpecialEffect", false)]
    public bool JustInLvl1 = false;

    private void OnValidate()
    {
        if(Logo != null)
        {
            Sprite = Sprite.Create(Logo, new Rect(0.0f, 0.0f, Logo.width, Logo.height), 
                new Vector2());
        }
    }

    private void DrawLogo()
    {
        if (this.Logo == null) return;

        GUILayout.BeginVertical(GUI.skin.label);
        GUILayout.Label(this.Logo);
        GUILayout.EndVertical();
    }
}
