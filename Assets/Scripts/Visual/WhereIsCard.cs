using UnityEngine;

public enum VisualStates
{
    Transition,
    InHand,
    Dragging
}

public class WhereIsCard : MonoBehaviour
{
    private HoverPreview hover;
    private CardVisual cv;

    // reference to a canvas on this object to set sorting order
    private Canvas canvas;
    
    private int TopSortingOrder = 500;

    // PROPERTIES
    private int slot = -1;
    public int Slot
    {
        get { return slot; }

        set
        {
            slot = value;
            if (value != -1)
            {
                canvas.sortingOrder = HandSortingOrder(slot);
            }
        }
    }

    private VisualStates state;
    public VisualStates VisualState
    {
        get { return state; }

        set
        {
            state = value;
            switch (state)
            {
                case VisualStates.Transition:
                    hover.ThisPreviewEnabled = false;
                    break;
                case VisualStates.Dragging:
                    hover.ThisPreviewEnabled = false;
                    break;
                case VisualStates.InHand:
                    hover.ThisPreviewEnabled = true;
                    break;
            }
        }
    }

    void Awake()
    {
        cv = GetComponent<CardVisual>();

        hover = GetComponent<HoverPreview>();

        canvas = GetComponentInChildren<Canvas>();
    }

    public void BringToFront()
    {
        canvas.sortingOrder = TopSortingOrder;
        canvas.sortingLayerName = "Above Everything";
    }

    // not setting sorting order inside of VisualStates property because when the card is drawn, 
    // we want to set an index first and set the sorting order only when the card arrives to hand. 
    public void SetHandSortingOrder()
    {
        if (slot != -1)
            canvas.sortingOrder = HandSortingOrder(slot);
        canvas.sortingLayerName = "Cards";

        //if (!TurnManager.Instance.CurrentPlayer.waitingForDiscard)
        //{
        //    if (slot != -1)
        //        canvas.sortingOrder = HandSortingOrder(slot);
        //    canvas.sortingLayerName = "Cards";
        //}
        //else
        //{
        //    if (slot != -1)
        //        canvas.sortingOrder = HandSortingOrder(slot) + TopSortingOrder;
        //    canvas.sortingLayerName = "Above Everything";

        //    GameObject preview = gameObject.transform.GetChild(1).gameObject;
        //    preview.GetComponentInChildren<Canvas>(true).sortingLayerName = "Above Everything";
        //    preview.GetComponentInChildren<Canvas>(true).sortingOrder = TopSortingOrder;
        //}
    }

    private int HandSortingOrder(int placeInHand)
    {
        return (-(placeInHand + 1) * 10);
    }


}