using UnityEngine;
using System.Collections;

// Place first and last children in array manually
// Others will be placed automatically with equal distances between first and last children
public class SameDistanceChildren : MonoBehaviour
{
    public Transform[] Children;

    float MinSpacing = 172;
    float MaxSpacing = 256;
    float SpacingDelta = 28;
    float limitForSpacingFix = 5;        // Limit of cards in hand before we need to fix spacing

    // Use this for initialization
    void Awake () 
    {
        UpdateChildren();
	}
    

    public void UpdateChildren(int amountOfCardsInHand = 0)
    {
        Vector3 firstChild = Children[0].localPosition;
        Vector3 lastChild = Children[Children.Length - 1].localPosition;
        
        // Distance between last and first child divided by number of spaces needed
        // Example: 10 children have 9 spaces between them
        float XDist = (lastChild.x - firstChild.x) / (float)(Children.Length - 1);

        Vector3 Dist;
        if (Mathf.Abs(XDist) > MaxSpacing)
        {
            XDist = -MaxSpacing;
        }
        else if (Mathf.Abs(XDist) < MinSpacing)
        {
            XDist = -MinSpacing;
        }

        // If there are more than a certain number of cards in hand we adjust the spacing between them
        if (amountOfCardsInHand > limitForSpacingFix)
        {
            XDist = -MaxSpacing + ((amountOfCardsInHand - limitForSpacingFix) * SpacingDelta);
        }
        // Otherwise, just space as much as possible
        else
        {
            XDist = -MaxSpacing;
        }
        
        Dist = new Vector3(XDist, 0, 0);

        for (int i = 1; i < Children.Length; i++)
        {
            Children[i].transform.localPosition = Children[i - 1].transform.localPosition + Dist;
        }
    }
    

}
