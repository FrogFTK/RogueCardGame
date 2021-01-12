using System.Collections;
using UnityEngine;

public class GiveBonusMana : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        Player.Instance.GetBonusMana(specialAmount);
    }
}
