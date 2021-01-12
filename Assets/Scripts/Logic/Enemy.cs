using System.Collections.Generic;
using UnityEngine;

public enum EnemyIntentions
{
    Attack,
    Block,
    Buff,
    Debuff,
    AttackAndBuff,
    AttackAndDebuff,
    AttackAndBuffAndDebuff,
    BlockAndBuff,
    BlockAndDebuff,
    BlockAndBuffAndDebuff,
    BuffAndDebuff,
}

public class Enemy : MonoBehaviour
{
    public EnemyIntentions intentions;

    public EnemyVisual Visuals;

    //public EnemyCombatPattern combatPattern;

    // STATIC For managing IDs
    public static Dictionary<int, Enemy> EnemiesCreatedThisGame = new Dictionary<int, Enemy>();

    private int baseHealth = 1;
    public int BaseHealth
    {
        get
        {
            return baseHealth;
        }

        set
        {
            baseHealth = value;
        }
    }
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            //HealthText.text = health + "/" + BaseHealth;
            //HealthbarImage.fillAmount = (float)health / baseHealth;

            if (value <= 0)
                Die();
        }
    }
    private int armor;
    public int Armor
    {
        get { return armor; }
        set
        {

            if (value <= 0)
            {
                armor = 0;
                //ArmorIcon.SetActive(false);
            }
            else
            {
                armor = value;
                //ArmorIcon.SetActive(true);
                //ArmorText.text = Armor.ToString();
            }
        }
    }
    private int strength = 0;
    public int Strength
    {
        get
        {
            return strength;
        }

        set
        {
            strength = value;
        }
    }
    
    //public StatusEffects statusEffects;

    #region Interface Properties
    private int EnemyID;
    public int ID
    {
        get { return EnemyID; }
    }
    #endregion
    
    
    private bool isAlive = true;
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }

        set
        {
            isAlive = value;
        }
    }
    
    // EVENTS TO KNOW WHEN TO CAUSE EFFECTS
    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments SpellPlayedEvent;
    public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;

    void Awake()
    {
        //EnemyID = IDFactory.GetUniqueID();

        LoadEnemyFromAsset();

        //HealthbarImage.fillAmount = 1;
        
        EnemiesCreatedThisGame.Add(EnemyID, this);

    }

    public virtual void OnTurnStart()
    {
        Armor = 0;

        //for (int i = 0; i < statusEffects.OnTurnStartEffects.Count; i++)
        //{
        //    statusEffects.OnTurnStartEffects[i].Tick();
        //}

        if (StartTurnEvent != null)
            StartTurnEvent.Invoke();
    }

    public virtual void OnTurnEnd()
    {
        //Debug.Log("Enemy - OnTurnEnd()");
        ChooseIntentions();
    }

    public void ChooseIntentions()
    {
        //combatPattern.SetIntentions(this);
        //switch(intentions)
        //{
        //    case EnemyIntentions.Attack:
        //        IntentionsIcon.sprite = GlobalSettings.Instance.AttackIntentionIcon;
        //        break;
        //    case EnemyIntentions.Block:
        //        IntentionsIcon.sprite = GlobalSettings.Instance.BlockIntentionIcon;
        //        break;
        //}
        //IntentionsObject.SetActive(true);
    }

    public void Die()
    {
        IsAlive = false;
        gameObject.SetActive(false);

        //int deadEnemyCount = 0;
        //foreach (Enemy e in TurnManager.Instance.Enemies)
        //{
        //    if (!e.IsAlive)
        //    {
        //        deadEnemyCount++;
        //    }
        //}

        //if (deadEnemyCount == TurnManager.Instance.Enemies.Length)
        //{
        //    TurnManager.Instance.GameWon();
        //}
    }

    

    private void OnMouseDown()
    {
        //if (Player.Players[0].handVisual.CardWaitingForTarget != null)
        //{
        //    int cardID = Player.Players[0].handVisual.CardWaitingForTarget.GetComponent<IDHolder>().UniqueID;
        //    CardLogic card = CardLogic.CardsCreatedThisGame[cardID];
        //    Player.Players[0].handVisual.CardWaitingForTarget = null;
            
        //    Player.Players[0].PlayASpellFromHand(card, this);
        //    Player.Players[0].AllowInput(true);
        //    GlobalSettings.Instance.EndTurnButton.interactable = true;
        //    ResetAllEnemyCanvas();
        //}
    }

    public void TakeTurn()
    {
        //if (isAlive)
        //    combatPattern.CheckPattern(this);
    }

    public void TakeDamage(int amt)
    {
        int amount = amt;
        int amountAfter = 0;
        int armorAfter = 0;

        if (Armor == 0)
        {
            amountAfter = amount;
            armorAfter = Armor;
        }
        else
        {
            armorAfter = Armor - amount;
            amountAfter = amount - Armor;
            if (armorAfter < 0)
            {
                armorAfter = 0;
            }
            if (amountAfter < 0)
                amountAfter = 0;
        }
        
        if (amountAfter > 0)
            Health -= amountAfter;

        Armor = armorAfter;

        //TakeDamageVisual();
    }

    

    void LoadEnemyFromAsset()
    {
        //BaseHealth = enemyAsset.MaxHealth;
        //Health = enemyAsset.MaxHealth;
        //Armor = enemyAsset.Armor;

        //combatPattern = System.Activator.CreateInstance(System.Type.GetType(enemyAsset.PatternScriptName)) as EnemyCombatPattern;
    }

    // STATUS EFFECTS

    // 0 = Sickness
    // 1 = Hype
    public void ApplyStatusEffects(int i, int time = 1)
    {
        //switch (i)
        //{
        //    // SICKNESS
        //    case 0:
        //        isSick = true;

        //        if (sickTime > 0)
        //            sickTime += time;
        //        else
        //            sickTime = time;
        //        break;
        //    // HYPE
        //    case 1:
        //        isHype = true;

        //        if (hypeTime > 0)
        //            hypeTime += time;
        //        else
        //            hypeTime = time;
        //        break;
        //}
    }

    public void TickStatusEffects()
    {

    }
}
