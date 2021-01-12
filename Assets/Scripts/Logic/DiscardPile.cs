using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public List<CardAsset> cards = new List<CardAsset>();
    public Player owner;

    public void ShuffleToDeck()
    {
        foreach (CardAsset card in cards)
        {
            owner.deck.cards.Add(card);
        }

        owner.deck.cards.Shuffle();
        cards.Clear();
    }
}
