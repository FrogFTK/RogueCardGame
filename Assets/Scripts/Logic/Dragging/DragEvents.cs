using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DragEvents : MonoBehaviour
{
    public abstract void OnDragStart();
    public abstract void OnDragging();
    public abstract void OnDragEnd();

    protected abstract bool DragSuccess();

    public virtual bool CanDrag
    {
        get
        {
            return true; // GlobalSettings.Instance.CanControlThisPlayer(playerOwner);
        }
    }
    
}
