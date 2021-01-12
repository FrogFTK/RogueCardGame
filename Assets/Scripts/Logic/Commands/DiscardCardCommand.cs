using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardACardCommand : Command
{
    private Player p;
    private int index;
    public DiscardACardCommand(Player p, int index)
    {
        this.p = p;
        this.index = index;
    }
    public override void StartCommandExecution()
    {
        p.handVisual.StartCoroutine(p.handVisual.DiscardACard(index));
    }
}