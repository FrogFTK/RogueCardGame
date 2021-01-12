
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragTargetedSpell : DragEvents
{

    public TargetingOptions targetOptions = TargetingOptions.AllCharacters;

    private WhereIsCard whereIsCard;
    private VisualStates tempVisualState;
    private GameObject Target;
    private CardVisual cardVisual;
    
    private int oldHandSlot;

    private bool waitingForTarget = false;

    public override bool CanDrag => base.CanDrag && cardVisual.CanBePlayed;

    private void Awake()
    {
        cardVisual = GetComponentInParent<CardVisual>();
        whereIsCard = GetComponentInParent<WhereIsCard>();
    }

    public override void OnDragStart()
    {
        oldHandSlot = whereIsCard.Slot;

        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();
    }

    public override void OnDragging()
    {

    }

    public override void OnDragEnd()
    {
        // 1) Check if we are holding a card over the table
        if (DragSuccess())
        {
            Player.Instance.handVisual.PlayTargetedSpell(transform.gameObject);
        }
        else
        {
            // Set old sorting order 
            /*whereIsCard.Slot = oldHandSlot;
            whereIsCard.VisualState = VisualStates.InHand;
            whereIsCard.SetHandSortingOrder();

            // Move this card back to its slot
            var oldCardPos = Player.Instance.handVisual.handSlots.slots[oldHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 0.25f);
            
            // Move this card back to its slot position
            //HandVisual PlayerHand = Player.Instance.handVisual;
            //Vector3 oldCardPos = PlayerHand.handSlots.slots[oldHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 0.25f);*/
            
            ResetCardToHand();
        }
    }

    protected override bool DragSuccess()
    {
        return PlayArea.CursorOverPlayArea;
    }

    private void ResetCardToHand()
    {
        // Set old sorting order 
        whereIsCard.Slot = oldHandSlot;
        whereIsCard.VisualState = VisualStates.InHand;
        whereIsCard.SetHandSortingOrder();

        // Move this card back to its slot
        var oldCardPos = Player.Instance.handVisual.handSlots.slots[oldHandSlot].transform.localPosition;
        transform.DOLocalMove(oldCardPos, 0.25f);
    }
}