using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TargetingOptions
{
    NoTarget,
    Allies, 
    Enemies,
    AllEnemies,
    AllCharacters
}

[System.Serializable]
public class CardAsset : ScriptableObject
{
    public int index;
    //public CharacterAsset characterAsset;  // if this is null, it`s a neutral card
	public Sprite CardImage;
    [TextArea(2,3)]
    public string Description;
    
    public List<string> SpellEffects = new List<string>();
    public List<int> SpecialAmounts = new List<int>();
    
    public int ManaCost;
    public bool ArmorPiercing;
    public bool IsTemporary;
    public bool BanishAfterUse;
    public TargetingOptions Targets;
    
    public int CardRarity;      // 0 = Basic, 1 = Common, 2 = Uncommon, 3 = Rare, 4 = ?????
}
