using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GlobalSettings: MonoBehaviour 
{
    [Header("Animation Times")]
    public float CardPreviewTime = 1f;
    public float CardTransitionTime= 1f;
    public float CardPreviewTimeFast = 0.2f;
    public float CardTransitionTimeFast = 0.35f;

    [Header("Prefabs and Assets")]
    public GameObject BlankCardPrefab;
    public GameObject DamageEffectPrefab;
    public GameObject ExplosionPrefab;
    public GameObject ScreenFadePrefab;

    [Header("Other")]
    public GameObject ChooseTargetPanel;
    public GameObject EndOfFightPanel;
    public Button EndTurnButton;
    public GameObject GameOverPanel;
    public Text GameOverText;

    [Header("Intention Icon Refs")]
    public Sprite AttackIntentionIcon;
    public Sprite BlockIntentionIcon;

    [Header("Status Effect Icon Refs")]
    public GameObject SickIcon;
    public GameObject BuffStrengthIcon;
    
    // SINGLETON
    public static GlobalSettings Instance;

    void Awake()
    {
        Instance = this;
    }
}
