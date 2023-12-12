using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasImprovementsManager : MonoBehaviour
{
    public GameObject CanvasRef;
    public GameObject ListPerks;

    public TextMeshProUGUI CodeLines;
    public TextMeshProUGUI Bitcoin;

    public TextMeshProUGUI CodeLinesPerSec;
    public TextMeshProUGUI BitcoinPerSec;

    private void Start()
    {
        ImprovementsManager.OnAddCanvas += AddPerkToCanvas;
    }

    private void OnDestroy()
    {
        ImprovementsManager.OnAddCanvas -= AddPerkToCanvas;
    }

    public void UpdateCurrentResources(Resources value)
    {
        CodeLines.text = $"{(int)value.CodeLines}";
        Bitcoin.text = $"{(int)value.Bitcoin}";
    }

    public void UpdateGenerateResources(Resources value)
    {
        CodeLinesPerSec.text = $"{value.CodeLines:F1} p/s";
        BitcoinPerSec.text = $"{value.Bitcoin:F1} p/s";
    }

    public void AddPerkToCanvas(ImprovementController perk)
    {
        var controller = CanvasRef.GetComponent<CanvasController>();
        if (controller == null) return;
        controller.CurrentPerk = perk;
        Instantiate(CanvasRef, ListPerks.transform);
    }

    public void UpdatePerks(List<ImprovementController> perks)
    {
        var controller = CanvasRef.GetComponent<CanvasController>();
        if (controller == null) return;
        int length = perks.Count;
        for (int i = 0; i < length; ++i)
        {
            controller.CurrentPerk = perks[i];
            Instantiate(CanvasRef, ListPerks.transform);
        }
    }
}
