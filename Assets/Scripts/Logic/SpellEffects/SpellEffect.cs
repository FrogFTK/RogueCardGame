﻿using UnityEngine;
using System.Collections;

public class SpellEffect
{
    public bool ArmorPiercing;

	public virtual void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("No Spell effect with this name found! Check for typos in CardAssets");
    }

    public virtual void ActivateEffect(int specialAmount = 0, ICharacter target = null, ICharacter owner = null)
    {
        Debug.Log("No Spell effect with this name found! Check for typos in CardAssets");
    }

}
