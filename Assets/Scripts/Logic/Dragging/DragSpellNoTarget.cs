using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragSpellNoTarget: DragEvents
{

    private int oldHandSlot;
    private WhereIsCard whereIsCard;
    private CardVisual cv;

    public override bool CanDrag
    {
        get
        { 
            // TODO : include full field check
            return base.CanDrag && cv.CanBePlayed;
        }
    }

    private void Awake()
    {
        whereIsCard = GetComponent<WhereIsCard>();
        cv = GetComponent<CardVisual>();
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
        // If cursor is over the play area
        if (DragSuccess())
        {
            Player.Instance.PlayASpellFromHand(GetComponent<IDHolder>().UniqueID, -1);
        }
        else
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

    protected override bool DragSuccess()
    {
        return PlayArea.CursorOverPlayArea;
    }


}
