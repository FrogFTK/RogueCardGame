using UnityEngine;
using System.Collections;

public class DrawACardCommand : Command
{
    private Player p;
    private Card card;
    private bool fast;
    private bool fromDeck;

    public DrawACardCommand(Card c, Player p, bool fromDeck)
    {        
        this.card = c;
        this.p = p;
        this.fromDeck = fromDeck;
    }

    public override void StartCommandExecution()
    {
        //Debug.Log("DrawACardCommand - Starting");
        p.handVisual.StartCoroutine(p.handVisual.GiveCardToPlayer(card.ca, card.UniqueCardID, true, fromDeck));
    }
}
