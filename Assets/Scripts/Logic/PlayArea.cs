using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    // Returns true if we are hovering over a play area
    public static bool CursorOverPlayArea { get; set; }

    public void OnCursorEnter()
    {
        CursorOverPlayArea = true;
    }
    
    public void OnCursorExit()
    {
        CursorOverPlayArea = false;
    }
}
