using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public static CardLibrary Instance;

    [SerializeField]
    private List<CardAsset> cards;
    
    public void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
        
        DontDestroyOnLoad(this);
    }

    public CardAsset GetCardByIndex(int cardIndex)
    {
        var card = cards[cardIndex];

        return card;
    }

}
