using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public TextMeshProUGUI NamePerk;
    public TextMeshProUGUI PriceCL;
    public TextMeshProUGUI PriceBitcoin;
    public TextMeshProUGUI Info;

    public Image LogoPerk;

    // To hide if the perks doesnt need this resource
    public GameObject CLGameObject;
    public GameObject BitcoinGameObject;

    public CanvasGroup AlphaCanvas;
    public Button ButtonToBuy;

    public ImprovementController CurrentPerk = null;

    public void InitCanvas(ImprovementController perk)
    {
        CurrentPerk = perk;

        NamePerk.text = $"{perk.Data.Name} - LVL: {perk.ImprovementLevel}";
        var Value = perk.Cost;
        if (Value.CodeLines > 0)
        {
            CLGameObject.gameObject.SetActive(true);
            PriceCL.text = Value.CodeLines.ToString();
        }
        if(Value.Bitcoin > 0)
        {
            BitcoinGameObject.gameObject.SetActive(true);
            PriceBitcoin.text = Value.Bitcoin.ToString();
        }

        Info.text = perk.Data.InfoPerk;
        ButtonToBuy.onClick.AddListener(() => GameManager.Instance.CheckToBuy(CurrentPerk));
        ImprovementsManager.OnImprovementAvailable += CheckIsAvaliable;
        ImprovementController.OnUpdateInfo += UpdateInfo;
    }

    public void UpdateInfo(ImprovementController perk)
    {
        if (perk != CurrentPerk) return;

        NamePerk.text = $"{CurrentPerk.Data.Name} - LVL: {CurrentPerk.ImprovementLevel}";
        var Value = CurrentPerk.Cost;
        if (Value.CodeLines > 0)
        {
            CLGameObject.gameObject.SetActive(true);
            PriceCL.text = Value.CodeLines.ToString();
        }
        if (Value.Bitcoin > 0)
        {
            BitcoinGameObject.gameObject.SetActive(true);
            PriceBitcoin.text = Value.Bitcoin.ToString();
        }
    }

    public void CheckIsAvaliable(ImprovementController perk, bool b)
    {
        if (CurrentPerk != perk) return;
        IsAvaliable(b);
    }

    public void IsAvaliable(bool b)
    {
        if (b == true)
            AlphaCanvas.alpha = 1f;
        else
            AlphaCanvas.alpha = 0.3f;
    }

    private void OnDestroy()
    {
        ButtonToBuy.onClick.RemoveAllListeners();
        ImprovementsManager.OnImprovementAvailable -= CheckIsAvaliable;
        ImprovementController.OnUpdateInfo -= UpdateInfo;
    }
}
