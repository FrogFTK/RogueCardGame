using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPlayable
{
    int ID { get; }
}

public interface ICharacter
{
    int ID { get; }

    int Health { get;    set;}
	int Armor { get;    set;}
    
    void Die();
    void AttackVisual();
    void TakeDamageVisual();
}

