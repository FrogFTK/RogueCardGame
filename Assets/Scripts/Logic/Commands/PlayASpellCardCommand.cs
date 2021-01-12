using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private Card card;

    public PlayASpellCardCommand(Card card)
    {
        this.card = card;
    }

    public override void StartCommandExecution()
    {
        Player.Instance.handVisual.PlayASpellFromHand(card.UniqueCardID);
    }
}
