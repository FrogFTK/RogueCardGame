using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class HoverPreview: MonoBehaviour
{
    public GameObject turnThisOffWhenPreviewing;  // Null will not turn anything off 
    public Vector3 targetPosition;
    public float targetScale;
    public GameObject previewGameObject;
    //public bool ActivateInAwake;

    private static HoverPreview _currentlyViewing;

    private static bool _previewsAllowed = true;
    public static bool PreviewsAllowed
    {
        get => _previewsAllowed;

        set 
        {
            _previewsAllowed = value;
            
            if (!_previewsAllowed)
                StopAllPreviews();
        }
    }

    private bool thisPreviewEnabled;
    public bool ThisPreviewEnabled
    {
        private get => thisPreviewEnabled;

        set 
        { 
            thisPreviewEnabled = value;
            if (!thisPreviewEnabled)
                StopThisPreview();
        }
    }

    private bool OverPlayArea { get; set;}
 
    /*void OnMouseEnter()
    {
        OverCollider = true;
        if (PreviewsAllowed && ThisPreviewEnabled)
            PreviewThisObject();
    }

    void OnMouseOver()
    {
        OverCollider = true;
        if (PreviewsAllowed && ThisPreviewEnabled && (_currentlyViewing != this))
            PreviewThisObject();
    }

    void OnMouseExit()
    {
        OverCollider = false;

        if (!PreviewingSomeCard())
            StopAllPreviews();
    }*/
    
    public void OnCursorEnter()
    {
        OverPlayArea = true;
        if (PreviewsAllowed && ThisPreviewEnabled)
            PreviewThisObject();
    }
    
    public void OnCursorExit()
    {
        OverPlayArea = false;

        if (!PreviewingSomeCard())
            StopAllPreviews();
    }
    
    private void PreviewThisObject()
    {
        // Disable the previous preview if there is one already
        StopAllPreviews();

        _currentlyViewing = this;
        previewGameObject.SetActive(true);

        // Disable an object if we have to
        if (turnThisOffWhenPreviewing!=null)
            turnThisOffWhenPreviewing.SetActive(false); 

        previewGameObject.transform.localPosition = Vector3.zero;
        previewGameObject.transform.localScale = Vector3.one;

        previewGameObject.transform.DOLocalMove(targetPosition, 1f).SetEase(Ease.OutQuint);
        previewGameObject.transform.DOScale(targetScale, 1f).SetEase(Ease.OutQuint);
    }

    private void StopThisPreview()
    {
        previewGameObject.transform.DOKill();
        previewGameObject.SetActive(false);
        previewGameObject.transform.localScale = Vector3.one;
        previewGameObject.transform.localPosition = Vector3.zero;
        
        // Enable object that was disabled if one exists
        if (turnThisOffWhenPreviewing!=null)
            turnThisOffWhenPreviewing.SetActive(true); 
        
    }

    // STATIC FUNCTIONS
    private static void StopAllPreviews()
    {
        if (_currentlyViewing == null) 
            return;
        
        _currentlyViewing.previewGameObject.transform.DOKill();
        _currentlyViewing.previewGameObject.SetActive(false);
        _currentlyViewing.previewGameObject.transform.localScale = Vector3.one;
        _currentlyViewing.previewGameObject.transform.localPosition = Vector3.zero;
        
        // Enable object that was disabled if one exists
        if (_currentlyViewing.turnThisOffWhenPreviewing!=null)
            _currentlyViewing.turnThisOffWhenPreviewing.SetActive(true);

    }

    private static bool PreviewingSomeCard()
    {
        if (!PreviewsAllowed)
            return false;

        var allHoverBlowups = FindObjectsOfType<HoverPreview>();

        return allHoverBlowups.Any(hb => hb.OverPlayArea && hb.ThisPreviewEnabled);
    }

}
