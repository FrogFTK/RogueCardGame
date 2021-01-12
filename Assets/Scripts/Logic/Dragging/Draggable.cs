using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Draggable : MonoBehaviour
{
    public static Draggable ObjectBeingDragged { get; private set; }
    private DragEvents dragEvents;

    // Space between camera and cursor on the Z axis 
    private float zDisplacement;
    private Vector3 distanceToCursor;
    
    private bool isDragging = false; 
    
    private Transform trans;
    private Camera cam;
    
    private void Awake()
    {
        cam = Camera.main;
        trans = transform;
            
        dragEvents = GetComponent<DragEvents>();
    }

    //void OnMouseDown()
    //{
    //    if (da!= null)
    //    {
    //        if (!TurnManager.Instance.CurrentPlayer.waitingForDiscard)
    //        {
    //            if (da.CanDrag)
    //            {
    //                isDragging = true;
    //                // when we are dragging something, all previews should be off
    //                HoverPreview.PreviewsAllowed = false;
    //                ObjectBeingDragged = this;
    //                //da.OnStartDrag();
    //                zDisplacement = -Camera.main.transform.position.z + transform.position.z;
    //                distanceToCursor = -transform.position + MouseInWorldCoords();
    //            }
    //        }
    //        else
    //        {
    //            if (Player.Players[0].handVisual.DiscardTarget == null)
    //            {
    //                // Move card to discard position
    //                Vector3 discardPosition = Player.Players[0].handVisual.PlayPreviewSpot.position;
    //                transform.DOMove(discardPosition, 0.25f);
    //                Player.Players[0].handVisual.DiscardTarget = gameObject;

    //                gameObject.GetComponent<HoverPreview>().ThisPreviewEnabled = false;
    //            }
    //            else
    //            {
    //                if (Player.Players[0].handVisual.DiscardTarget != gameObject)
    //                {
    //                    WhereIsCard whereIsCard = Player.Players[0].handVisual.DiscardTarget.GetComponent<WhereIsCard>();

    //                    // Set old sorting order
    //                    //whereIsCard.SetHandSortingOrder();

    //                    // Move this card back to its hand slot position
    //                    HandVisual PlayerHand = TurnManager.Instance.CurrentPlayer.handVisual;
    //                    Vector3 oldCardPos = PlayerHand.slots.Children[whereIsCard.Slot].transform.localPosition;
    //                    Player.Players[0].handVisual.DiscardTarget.transform.DOLocalMove(oldCardPos, 0.25f);

    //                    // Move card to discard position
    //                    Vector3 discardPosition = Player.Players[0].handVisual.PlayPreviewSpot.position;
    //                    transform.DOMove(discardPosition, 0.25f);
    //                    Player.Players[0].handVisual.DiscardTarget = gameObject;
    //                    gameObject.GetComponent<HoverPreview>().ThisPreviewEnabled = false;
    //                }

    //            }
    //        }
    //    }

    //}

    // Update is called once per frame

    
    private void Update ()
    {
        if (!isDragging)
            return;
        
        // Reposition the card relative to the mouse position
        var mousePos = MouseInWorldCoords();
        trans.position = new Vector3(mousePos.x - distanceToCursor.x, mousePos.y - distanceToCursor.y, trans.position.z);   
        dragEvents.OnDragging();
    }

    private void OnMouseUp()
    {
        if (!isDragging) 
            return;
        
        isDragging = false;
        // turn all previews back on
        HoverPreview.PreviewsAllowed = true;
        ObjectBeingDragged = null;
        dragEvents.OnDragEnd();
    }   

    private void OnMouseDown()
    {
        if (!dragEvents.CanDrag) 
            return;
        
        isDragging = true;
        
        // Turn off previews while dragging
        HoverPreview.PreviewsAllowed = false;
        ObjectBeingDragged = this;
        dragEvents.OnDragStart();
        zDisplacement = -cam.transform.position.z + transform.position.z;
        distanceToCursor = -trans.position + MouseInWorldCoords();
    }
    
    // Returns mouse position in World coords for our GameObject to follow. 
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;
        
        return cam.ScreenToWorldPoint(screenMousePos);
    }
        
}
