using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrefabRefs : MonoBehaviour
{
    [Header("Numbers and Values")]
    public float CardPreviewTime = 1f;
    public float CardTransitionTime = 1f;
    public float CardPreviewTimeFast = 0.2f;
    public float CardTransitionTimeFast = 0.35f;

    [Header("Prefabs and Assets")]
    public GameObject SpellCardPrefab;
    public GameObject TargetedSpellCardPrefab;
    public GameObject BlankCardPrefab;
    public GameObject DamageEffectPrefab;
    public GameObject ExplosionPrefab;
    public GameObject ScreenFadePrefab;

    [Header("Intentions Icons")]
    public Sprite AttackIntentionIcon;
    public Sprite BlockIntentionIcon;

    [Header("Status Effect Icons")]
    public GameObject SickIcon;
    public GameObject BuffStrengthIcon;

    [Header("Other")]
    public GameObject ChooseTargetPanel;
    public GameObject EndOfFightPanel;
    public Button EndTurnButton;
    public GameObject GameOverPanel;
    
    public Text GameOverText;
    
    //public Dictionary<AreaPosition, Player> Players = new Dictionary<AreaPosition, Player>();
    
    // SINGLETON
    public static PrefabRefs Instance;

    void Awake()
    {
        Instance = this;
    }
}
