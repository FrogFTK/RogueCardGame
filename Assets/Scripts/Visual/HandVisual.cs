using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.Serialization;

public class HandVisual : MonoBehaviour
{
    [FormerlySerializedAs("DeckTransform")] [Header("Transform Refs")]
    public Transform deckTransform;
    public Transform discardTransform;
    
    public Transform cardPlayedTransform;
    
    public Transform chooseTargetTransform;

    public bool canTakeCards = true;
    public HandSlots handSlots;
    
    // List of all CardVisual GameObjects
    public List<GameObject> CardsInHand = new List<GameObject>();

    private GameObject cardWaitingForTarget;
    public GameObject CardWaitingForTarget
    {
        get
        {
            return cardWaitingForTarget;
        }

        set
        {
            cardWaitingForTarget = value;
        }
    }

    private GameObject discardTarget;
    public GameObject DiscardTarget
    {
        get
        {
            return discardTarget;
        }

        set
        {
            discardTarget = value;
        }
    }

    // Add a new card GameObject to hand
    public void AddCard(GameObject card)
    {
        // We always insert a new card as 0th element in CardsInHand List 
        CardsInHand.Insert(0, card);

        // Parent this card to our Slots GameObject
        card.transform.SetParent(handSlots.transform);
        
        AdjustHandSlots();
    }

    // Remove a card GameObject from hand
    public void RemoveCard(GameObject card)
    {
        // Remove a card from the list
        CardsInHand.Remove(card);
        
        AdjustHandSlots();
    }

    // Remove card with a given index from hand
    public void RemoveCardAtIndex(int index)
    {
        CardsInHand.RemoveAt(index);

        AdjustHandSlots();
    }

    // Get a card GameObject with a given index in hand
    public GameObject GetCardAtIndex(int index)
    {
        return CardsInHand[index];
    }
    
    // Move Slots GameObject according to the number of cards in hand
    private void UpdatePlacementOfSlots()
    {
        float posX;
        if (CardsInHand.Count > 0)
            posX = (handSlots.slots[0].transform.localPosition.x - handSlots.slots[CardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // Tween Slots GameObject to new position in 0.3 seconds
        handSlots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    // Shift all cards to their new slots
    private void PlaceCardsOnNewSlots()
    {
        foreach (var g in CardsInHand)
        {
            // Tween this card to a new Slot
            g.transform.DOLocalMoveX(handSlots.slots[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);

            // Apply correct sorting order and HandSlot value for later 
            var w = g.GetComponent<WhereIsCard>();
            if (w == null)
                Debug.Log("WhereIsCard not found on object: " + g.name);
            w.Slot = CardsInHand.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    private void AdjustHandSlots()
    {
        handSlots.UpdateHandSlots(CardsInHand.Count);

        // Re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }
    
    // CARD DRAW FUNCTIONS

    // Creates a card and returns a new card as a GameObject
    GameObject CreateCard(CardAsset c, Vector3 position, Vector3 eulerAngles)
    {
        var card = Instantiate(GlobalSettings.Instance.BlankCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;

        // Check for targeted or non-targeted spell
        if (c.Targets != TargetingOptions.NoTarget)
        {
            card.AddComponent<DragTargetedSpell>();
            //DragSpellOnTarget dragSpell = card.GetComponentInChildren<DragSpellOnTarget>();
            //dragSpell.Targets = c.Targets;
        }

        card.GetComponentInChildren<Canvas>().sortingLayerName = "Above Everything";

        // apply the look of the card based on the info from CardAsset
        var cv = card.GetComponent<CardVisual>();
        cv.cardAsset = c;
        cv.ReadCardFromAsset();

        return card;
    }

    // Gives player a new card
    public IEnumerator GiveCardToPlayer(CardAsset c, int uniqueId, bool fast = false, bool fromDeck = true)
    {
        GameObject card;
        //if (fromDeck)
        card = CreateCard(c, deckTransform.position, Vector3.zero);

        // Bring card to front while it travels from draw spot to hand
        var w = card.GetComponent<WhereIsCard>();
        w.BringToFront();
        w.Slot = 0;
        w.VisualState = VisualStates.Transition;

        // Add the card gameobject to the hand list
        AddCard(card);


        // pass a unique ID to this card.
        var id = card.AddComponent<IDHolder>();
        id.UniqueID = uniqueId;

        // move card to the hand;
        var moveAndScale = DOTween.Sequence();
        moveAndScale.Append(card.transform.DOLocalMove(handSlots.slots[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTimeFast));
        moveAndScale.Insert(0f, card.transform.DOScale(new Vector3(5, 5, 1), GlobalSettings.Instance.CardTransitionTimeFast));
        //if (canTakeCards)
        //    s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast));

        yield return moveAndScale.WaitForPosition(GlobalSettings.Instance.CardTransitionTimeFast / 2);
        {
            ChangeLastCardStatusToInHand(card, w);
            var cv = card.GetComponent<CardVisual>();
            if (cv != null)
                cv.CanBePlayed = Card.CardsCreated[id.UniqueID].CurrentManaCost <= Player.Instance.ManaLeft;
        }
    }

    // Called when card arrives to hand 
    void ChangeLastCardStatusToInHand(GameObject card, WhereIsCard w)
    {
        w.VisualState = VisualStates.InHand;

        // Set correct sorting order
        w.SetHandSortingOrder();

        //Debug.Log("DrawACardCommand - Complete");
        Command.CommandExecutionComplete();
    }

    public IEnumerator DiscardACard(int index)
    {
        var cardToDiscard = CardsInHand[index];
        RemoveCardAtIndex(index);

        var s = DOTween.Sequence();

        // move cards to the discard
        s.Append(cardToDiscard.transform.DOMove(discardTransform.position, GlobalSettings.Instance.CardTransitionTimeFast));
        s.Insert(0f, cardToDiscard.transform.DORotate(new Vector3(0f, -179f, 0f), GlobalSettings.Instance.CardTransitionTimeFast));

        yield return s.WaitForPosition(GlobalSettings.Instance.CardTransitionTimeFast / 3);
        Command.CommandExecutionComplete();

        s.OnComplete(() =>
        {
            Destroy(cardToDiscard.gameObject);
        });
    }

    // PLAYING SPELLS

    // 2 Overloaded method to show a spell played from hand
    public void PlayASpellFromHand(int cardId)
    {
        var card = IDHolder.GetGameObjectWithID(cardId);
        PlayASpellFromHand(card);
    }

    public void PlayASpellFromHand(GameObject cardVisual)
    {
        // Remove the card gameobject from hand 
        cardVisual.GetComponent<WhereIsCard>().VisualState = VisualStates.Transition;
        RemoveCard(cardVisual);
        cardVisual.transform.SetParent(null);

        // CommandExecutionComplete for PlayASpellCommand
        Command.CommandExecutionComplete();


        // Tween card gameobject to the preview spot
        var s = DOTween.Sequence();
        s.Append(cardVisual.transform.DOMove(cardPlayedTransform.position, 0.35f));
        s.AppendInterval(0.25f);
        
        // Remove the card gameobject when tween is complete
        s.OnComplete(()=>
        {
            Player.Instance.HighlightPlayableCards();
            Destroy(cardVisual);
        });
    }

    public void PlayTargetedSpell(GameObject card)
    {
        int cardId = card.GetComponent<IDHolder>().UniqueID;
        cardWaitingForTarget = card;
        
        //Player.Players[0].AllowInput(false);
        //GlobalSettings.Instance.EndTurnButton.interactable = false;

        //// Bring the card and possible targets above the choose target panel
        //foreach (Enemy enemy in TurnManager.Instance.Enemies)
        //{
        //    if (enemy != null)
        //        enemy.BringToFront();
        //}

        card.GetComponentInChildren<Canvas>().sortingLayerName = "Above Everything";
        card.GetComponentInChildren<Canvas>().sortingOrder = 500;

        // Tween card gameobject to the preview spot
        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOMove(chooseTargetTransform.position, 0.35f));
        s.AppendInterval(0.25f);
        s.OnComplete(() => {});
    }

    public void CancelTargeting()
    {
        // Enable card highlights and allow cards to recieving input
        //if (TurnManager.Instance.gameStarted)
        //{
        //    Player.Players[0].AllowInput(true);
        //    GlobalSettings.Instance.EndTurnButton.interactable = true;
        //}

        //Enemy.ResetAllEnemyCanvas();

        //cardWaitingForTarget.GetComponentInChildren<DragSpellOnTarget>().ResetCardToHand();
        //cardWaitingForTarget = null;
    }

    public void ConfirmDiscard()
    {
        //if (discardTarget != null)
        //{
        //    TurnManager.Instance.CurrentPlayer.DiscardCardAtIndex(discardTarget.GetComponent<WhereIsTheCardOrCreature>().Slot);
        //    FightUIManager.Instance.discardPanel.SetActive(false);
        //    TurnManager.Instance.CurrentPlayer.waitingForDiscard = false;
        //    GlobalSettings.Instance.EndTurnButton.gameObject.SetActive(true);

        //    ResetCanvasSorting();
        //}
    }

    public void BringHandToFront()
    {
        //foreach(GameObject go in CardsInHand)
        //{
        //    go.GetComponent<WhereIsTheCardOrCreature>().BringToFront();
        //    //go.GetComponentInChildren<Canvas>().sortingLayerName = "Above Everything";
        //    //go.GetComponentInChildren<Canvas>().sortingOrder += 500;

        //    GameObject preview = go.transform.GetChild(1).gameObject;
        //    preview.GetComponentInChildren<Canvas>(true).sortingLayerName = "Above Everything";
        //    preview.GetComponentInChildren<Canvas>(true).sortingOrder += 500;
        //}
    }

    public void ResetCanvasSorting()
    {
        //foreach (GameObject go in CardsInHand)
        //{
        //    go.GetComponent<WhereIsTheCardOrCreature>().SetHandSortingOrder();
        //    //go.GetComponentInChildren<Canvas>().sortingLayerName = "Cards";
        //    //go.GetComponentInChildren<Canvas>().sortingOrder -= 500;
            
        //    GameObject preview = go.transform.GetChild(1).gameObject;
        //    preview.GetComponentInChildren<Canvas>(true).sortingLayerName = "Above Everything";
        //    preview.GetComponentInChildren<Canvas>(true).sortingOrder = 0;
        //}
    }
    
    public void UpdateCardDescriptions()
    {
        //foreach(GameObject card in CardsInHand)
        //{
        //    card.GetComponent<OneCardManager>().UpdateDescription();
        //}
    }
}
