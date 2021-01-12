using UnityEngine;
using System.Collections;

public class GiveBonusManaForTurn: SpellEffect 
{
	public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        Player.Instance.GetBonusMana(specialAmount, true);
    }
}
