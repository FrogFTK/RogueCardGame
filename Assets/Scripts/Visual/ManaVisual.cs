using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaVisual : MonoBehaviour
{
    // for Singleton Pattern
    public static ManaVisual Instance;
    public TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    public void UpdateManaText(int manaLeft, int manaThisTurn)
    {
        text.text = manaLeft + "/" + manaThisTurn;
    }
}
