using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour
{
    public static PlayerVisual Instance;
    
    // a script with references to all the visual game objects for this player
    public GameObject playerAvatar;

    public GameObject armorIcon;
    public TextMeshProUGUI armorText;

    public Image healthbarImage;
    public TextMeshProUGUI healthText;

    public TextMeshProUGUI manaText;

    private bool isTweening = false;

    //public StatusEffectsVisual SEVisual;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    public void VisualPunch(bool isAttacking = true)
    {
        if (isTweening)
            return;
        
        isTweening = true;

        float punchAmount = isAttacking ? 150 : -150;
        
        Vector3 punchVector = new Vector3(punchAmount, 0, 0);
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOPunchPosition(punchVector, 0.25f, 0, 0));
        s.OnComplete(() => isTweening = false);
    }

    public void UpdateArmorVisual()
    {
        armorIcon.SetActive(Player.Instance.Armor > 0);
        armorText.text = Player.Instance.Armor.ToString();
    }
    
    public void UpdateHealthVisual()
    {
        healthText.text = Player.Instance.CurrentHealth + "/" + Player.Instance.MaxHealth;
        healthbarImage.fillAmount = (float)Player.Instance.CurrentHealth / Player.Instance.MaxHealth;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) VisualPunch(false);
        if (Input.GetKeyDown(KeyCode.L)) VisualPunch();
    }
}
