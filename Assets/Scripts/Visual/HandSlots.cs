using UnityEngine;
using System.Collections;

// Place first and last children in array manually
// Others will be placed automatically with equal distances between first and last children
public class HandSlots : MonoBehaviour
{
    public Transform[] slots;

    float MinSpacing = 172;
    float MaxSpacing = 256;
    float changeInSpacing = 28;         // The amount used to modify the spcaing between slots
    float cardLimitForSpacing = 5;      // Limit of cards in hand before we need to change the spacing

    // Use this for initialization
    void Awake()
    {
        UpdateHandSlots(0);
    }


    public void UpdateHandSlots(int amountOfCardsInHand)
    {
        Vector3 firstSlot = slots[0].localPosition;
        Vector3 lastSlot = slots[slots.Length - 1].localPosition;

        // Distance between last and first child divided by number of spaces needed
        // Example: 10 children have 9 spaces between them
        float XDist = (lastSlot.x - firstSlot.x) / (float)(slots.Length - 1);

        Vector3 Dist;
        if (amountOfCardsInHand < cardLimitForSpacing)
        {
            XDist = -MaxSpacing;
        }
        else
        {
            XDist = -MaxSpacing + ((amountOfCardsInHand - cardLimitForSpacing) * changeInSpacing);
        }
        
        Dist = new Vector3(XDist, 0, 0);

        for (int i = 1; i < slots.Length; i++)
        {
            slots[i].transform.localPosition = slots[i - 1].transform.localPosition + Dist;
        }
    }


}
