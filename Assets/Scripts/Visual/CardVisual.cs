using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    public CardAsset cardAsset;
    
    public CardVisual PreviewManager;

    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ManaCostText;
    public TextMeshProUGUI DescriptionText;
    
    public Image CardBackground;
    public Image CardTypeBackground;
    public Image CardImage;
    public Image CardGlowImage;

    private void Awake()
    {
        if (cardAsset != null)
            ReadCardFromAsset();
        else
            cardAsset = (CardAsset)Resources.Load("Default");
    }

    private bool canBePlayed = false;
    public bool CanBePlayed
    {
        get => canBePlayed;

        set
        {
            canBePlayed = value;
            CardGlowImage.gameObject.SetActive(canBePlayed);
            PreviewManager.CardGlowImage.gameObject.SetActive(canBePlayed);
        }
    }

    public void ReadCardFromAsset()
    {

        NameText.text = cardAsset.name;
        
        ManaCostText.text = cardAsset.ManaCost.ToString();
        
        DescriptionText.text = cardAsset.Description;

        if (PreviewManager != null)
        {
            // this is a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }
        
        GetComponentInChildren<Canvas>().overrideSorting = true;
    }

    public void UpdateDescription()
    {
        //DescriptionText.text = cardAsset.Description.Replace("SA0+STRENGTH", (cardAsset.SpecialAmounts[0] + Player.Players[0].Strength).ToString());

        //if (PreviewManager != null)
        //{
        //    // this is a card and not a preview
        //    // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
        //    PreviewManager.DescriptionText.text = cardAsset.Description.Replace("SA0+STRENGTH", (cardAsset.SpecialAmounts[0] + Player.Players[0].Strength).ToString());
        //}
    }
}
