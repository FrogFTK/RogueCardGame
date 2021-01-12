using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Card : IPlayable
{
    // All cards created this fight
    public static Dictionary<int, Card> CardsCreated = new Dictionary<int, Card>();

    public Player owner;
    public int UniqueCardID;

    // The card asset that stores all the info about this card
    public CardAsset ca;

    // List of effects that will be attached to this card when it`s created
    //public List<SpellEffect> effects = new List<SpellEffect>();
    
    public int ID => UniqueCardID;

    public int CurrentManaCost { get; set; }

    public bool CanBePlayed
    {
        get
        {
            //bool ownersTurn = (TurnManager.Instance.CurrentPlayer == owner);
            //bool ownersTurn = true;
            return CurrentManaCost <= owner.ManaLeft;
        }
    }

    // CONSTRUCTOR
    public Card(CardAsset ca)
    {
        this.ca = ca;

        // get a unique ID
        UniqueCardID = IDFactory.GetUniqueID();
        
        CurrentManaCost = ca.ManaCost;
        
        // add this card to a dictionary with its ID as a key		
        CardsCreated.Add(UniqueCardID, this);
        
    }
    
    public void ResetManaCost()
    {
        CurrentManaCost = ca.ManaCost;
    }

}
