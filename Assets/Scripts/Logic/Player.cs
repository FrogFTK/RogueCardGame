using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Logic;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    #region Character Stats
    public int CurrentHealth
    {
        get => RunManager.Instance.currentRun.stats.currentHp;
        private set
        { 
            RunManager.Instance.currentRun.stats.currentHp = value <= 0 ? 0 : value;
            playerVisual.UpdateHealthVisual();
        }
    }
    public int MaxHealth
    {
        get => RunManager.Instance.currentRun.stats.maxHp;
        set
        {  
            RunManager.Instance.currentRun.stats.maxHp = value <= 0 ? 0 : value;
            playerVisual.UpdateHealthVisual();
        }
    }
    public int Armor
    {
        get => RunManager.Instance.currentRun.stats.currentArmor;
        set
        {
            RunManager.Instance.currentRun.stats.currentArmor = value <= 0 ? 0 : value;
            playerVisual.UpdateArmorVisual();
        }
    }
    public int Strength
    {
        get
        {
            return RunManager.Instance.currentRun.stats.power;
        }

        set
        {
            RunManager.Instance.currentRun.stats.power = value;
            //handVisual.UpdateCardDescriptions();
        }
    }
    public int DrawAmountForTurn
    {
        get => RunManager.Instance.currentRun.stats.drawAmountForTurn;
        set => RunManager.Instance.currentRun.stats.drawAmountForTurn = value;
    }
    #endregion

    #region Logic Refs
    public Deck deck;
    public Hand hand;
    public DiscardPile discardPile;
    //public StatusEffects statusEffects;
    #endregion

    #region Mana Stuff
    public TextMeshProUGUI manaText;
    
    private int baseMana = 3;
    
    // Mana available this turn
    private int manaLeft;
    
    // Mana the player has this fight
    private int totalMana;
    private int TotalMana
    {
        get => totalMana;
        set
        {
            totalMana = value < 0 ? 0 : value;
            ManaVisual.Instance.UpdateManaText(manaLeft, totalMana);
        }
    }
    
    public int ManaLeft
    {
        get => manaLeft;
        private set
        {
            manaLeft = value < 0 ? 0 : value;
            manaText.text = manaLeft + "/" + totalMana;
            ManaVisual.Instance.UpdateManaText(manaLeft, totalMana);
        }
    }

    private int bonusMana;
    public int BonusMana
    {
        get => bonusMana;
        set => bonusMana = value;
    }

    private int bonusManaThisTurn;
    public int BonusManaThisTurn
    {
        get => bonusManaThisTurn;
        set => bonusManaThisTurn = value;
    }
    #endregion

    public PlayerVisual playerVisual;
    public HandVisual handVisual;

    public int handLimit = 8;

    #region Events

    // EVENTS FOR EFFECT TIMING
    public delegate void VoidWithNoArguments();
    public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;
    public event VoidWithNoArguments CardPlayedEvent;

    #endregion

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
        
        //EndTurnEvent += DiscardHand;
    }

    public void SetupPlayer()
    {
        AssignPlayerStats();
        
        LoadDeck();
        deck.cards.Shuffle();
    }
    
    private void AssignPlayerStats()
    {
        CurrentHealth = RunManager.Instance.currentRun.stats.currentHp;
        MaxHealth = RunManager.Instance.currentRun.stats.maxHp;
        Armor = RunManager.Instance.currentRun.stats.startingArmor;
        
        TotalMana = baseMana + bonusMana;
        ManaLeft = TotalMana + bonusManaThisTurn;
    }

    private void LoadDeck()
    {
        foreach (var index in RunManager.Instance.currentDeck)
        {
            deck.cards.Add(CardLibrary.Instance.GetCardByIndex(index));
        }
    }
    
    public void OnTurnStart()
    {
        DrawForTurn();
        ////Debug.Log("Player - OnTurnStart()");
        //new ShowMessageCommand("Your Turn", 2f).AddToQueue();

        //ManaThisTurn = baseMana + bonusMana;
        //ManaLeft = ManaThisTurn;
        
        //Armor = 0;

        //for (int i = 0; i < statusEffects.OnTurnStartEffects.Count; i++)
        //{
        //    statusEffects.OnTurnStartEffects[i].Tick();
        //}

        //if (StartTurnEvent != null)
        //    StartTurnEvent.Invoke();
    }

    public void OnTurnEnd()
    {
        //HoverPreview.PreviewsAllowed = false;

        //// Invoke all EndTurn events if any exist
        //if (EndTurnEvent != null)
        //    EndTurnEvent.Invoke();

        //// Clear all bonus mana gained for the turn
        //ManaThisTurn -= BonusManaThisTurn;
        //BonusManaThisTurn = 0;

        //GetComponent<TurnMaker>().StopAllCoroutines();
        
        //new DelayCommand(0.35f).AddToQueue();
    }

    //// PLAYER ACTIONS ////

    // get mana from coin or other spells 
    public void GetBonusMana(int amount, bool forTurn = false)
    {
        ManaLeft += amount;

        if (forTurn)
            BonusManaThisTurn += amount;
        else
        {
            BonusMana += amount;
            TotalMana += amount;
        }
    }

    // Draw hand for the turn
    public void DrawForTurn()
    {
        for (var i = 0; i < DrawAmountForTurn; i++)
        {
            DrawACard();
        }
        
    }

    // Draw a single card from the deck to the hand
    public void DrawACard(bool fast = false)
    {
        if (deck.cards.Count > 0)
        {
            if (hand.cards.Count >= handLimit) 
                return;
            
            // Add card to hand
            var newCard = new Card(deck.cards[0])
            {
                owner = this
            };
            hand.cards.Insert(0, newCard);
            
            deck.cards.RemoveAt(0);
            
            new DrawACardCommand(hand.cards[0], this, true).AddToQueue();
        }
        else
        {
            // Deck is empty, so we shuffle discard pile back to deck
            discardPile.ShuffleToDeck();
            DrawACard();
        }

    }

    // Discard entire hand
    public void DiscardHand()
    {
        if (hand.cards.Count <= 0) return;
        
        for (var i = hand.cards.Count; i > 0; i--)
        {
            DiscardCardAtIndex(0);
        }
    }

    public void DiscardCardAtIndex(int index)
    {
        // check that there is a card with this index
        if (index < hand.cards.Count)
        {
            //Debug.Log("Index is " + index);
            //Debug.Log("CardsInHand.Count is " + hand.CardsInHand.Count);
            discardPile.cards.Add(hand.cards[index].ca);
            hand.cards.RemoveAt(index);
            new DiscardACardCommand(this, index).AddToQueue();
        }
        else
        {
            Debug.Log("hand is empty");
        }
    }

    // Get card NOT from deck (a token or a coin)
    /*public void GetACardNotFromDeck(CardAsset cardAsset)
    //{
    //    //if (hand.CardsInHand.Count < 10) // PArea.handVisual.slots.Children.Length)
    //    //{
    //    //    // 1) logic: add card to hand
    //    //    CardLogic newCard = new CardLogic(cardAsset)
    //    //    {
    //    //        owner = this
    //    //    };
    //    //    hand.CardsInHand.Insert(0, newCard);
    //    //    // 2) send message to the visual Deck
    //    //    //new DrawACardCommand(hand.CardsInHand[0], this, fast: true, fromDeck: false).AddToQueue();
    //    //}
    //    // no removal from deck because the card was not in the deck
    //}*/

    // 2 FUNCTIONS FOR PLAYING SPELLS
    // 1st overload - takes ids as arguments
    // (convenient to call this method from visual)
    public void PlayASpellFromHand(int spellCardUniqueId, int targetUniqueId)
    {
        PlayASpellFromHand(Card.CardsCreated[spellCardUniqueId], null);
    }

    // 2nd overload - takes a card/target directly instead of id
    private void PlayASpellFromHand(Card playedCard, ICharacter target)
    {
        ManaLeft -= playedCard.CurrentManaCost;
        
        // Move this card to CardPlayedSpot
        new PlayASpellCardCommand(playedCard).AddToQueue();
        
        // Activate the cards effects
        if (playedCard.ca.SpellEffects.Count > 0)
        {
            for(var i = 0; i < playedCard.ca.SpellEffects.Count; i++)
            {
                var effect = System.Activator.CreateInstance(System.Type.GetType(playedCard.ca.SpellEffects[i])) as SpellEffect;
                effect.ActivateEffect(playedCard.ca.SpecialAmounts[i], target);
            }
        }
        else
        {
            Debug.Log("No effects found on card asset");
        }

        // add card to discard pile then remove this card from hand
        discardPile.cards.Add(playedCard.ca);
        hand.cards.Remove(playedCard);
    }

    public void TakeDamage(int amt)
    {
        var amount = amt;
        var amountAfter = 0;
        var armorAfter = 0;

        if (Armor == 0)
            amountAfter = amount;
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
            CurrentHealth -= amountAfter;

        Armor = armorAfter;
        
        PlayerVisual.Instance.VisualPunch(false);
        PlayerVisual.Instance.UpdateHealthVisual();
    }

    public void Die()
    {
        //Instance.GameLost();
    }

    public void HighlightPlayableCards(bool removeAllHighlights = false)
    {
        foreach (var c in hand.cards)
        {
            var g = IDHolder.GetGameObjectWithID(c.UniqueCardID);
            var cv = IDHolder.GetGameObjectWithID(c.UniqueCardID).GetComponent<CardVisual>();
            if (g != null)
                cv.CanBePlayed = (c.CanBePlayed) && !removeAllHighlights;
        }
    }
    
    public void AllowInput(bool b)
    {
        //// Show "Choose Target" panel and turn off hover previews
        //GlobalSettings.Instance.ChooseTargetPanel.SetActive(!b);
        //HoverPreview.PreviewsAllowed = b;

        //// Remove card highlights and block cards from receiving input
        //Player.Players[0].HighlightPlayableCards(!b);
    }
    
    
    
    
    
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawACard();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            DiscardCardAtIndex(2);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HighlightPlayableCards();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            DiscardHand();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            HoverPreview.PreviewsAllowed = !HoverPreview.PreviewsAllowed;
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(10);
        }
    }
}
