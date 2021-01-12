using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisual : MonoBehaviour
{
    // Public references to enemy visuals
    public Canvas EnemyCanvas;
    
    public Image HealthbarImage;
    public TextMeshProUGUI HealthText;

    public GameObject ArmorIcon;
    public TextMeshProUGUI ArmorText;

    public GameObject IntentionsObject;
    public Image IntentionsIcon;
    public TextMeshProUGUI IntentionsText;

    //public StatusEffectsVisual SEVisual;

    string oldSortingName;
    int oldSortingOrder;



    public void AttackVisual()
    {
        Vector3 punchVector = new Vector3(-0.75f, 0, 0);
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOPunchPosition(punchVector, 0.25f, 0, 0));
        s.OnComplete(() =>
        {
            IntentionsObject.SetActive(false);
        });
    }

    public void TakeDamageVisual()
    {
        Vector3 punchVector = new Vector3(0.75f, 0, 0);
        Sequence s = DOTween.Sequence();
        s.AppendInterval(0.05f);
        s.Append(transform.DOPunchPosition(punchVector, 0.25f, 0, 0));
        s.AppendInterval(0.5f);
        s.OnComplete(() =>
        {
            // CommandExecutionComplete for DealDamageCommand and TakeDamageCommand
            //Command.CommandExecutionComplete();
        });
    }

    public void BringToFront()
    {
        oldSortingName = EnemyCanvas.sortingLayerName;
        oldSortingOrder = EnemyCanvas.sortingOrder;

        EnemyCanvas.sortingLayerName = "Above Everything";
        EnemyCanvas.sortingOrder = 500;
    }

    public void ResetCanvasSorting()
    {
        EnemyCanvas.sortingLayerName = oldSortingName;
        EnemyCanvas.sortingOrder = oldSortingOrder;
    }

    public static void ResetAllEnemyCanvas()
    {
        foreach (Enemy enemy in Enemy.EnemiesCreatedThisGame.Values)
        {
            if (enemy.IsAlive)
                enemy.Visuals.ResetCanvasSorting();
        }
    }
}
