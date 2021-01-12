using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance;
    
        public Image currentPanel, nextPanel, prevPanel;

        public Button continueButton;

        private void Start()
        {
            if (Instance != null)
                Destroy(this);
            else
                Instance = this;

            if (!File.Exists(Application.persistentDataPath + "/currentRun.fk"))
            {
                continueButton.interactable = false;
            }
        }

        public void TweenPanels(Image panel)
        {
            panel.transform.DOLocalMove(Vector2.zero, 0.25f);
            currentPanel.transform.DOLocalMove(new Vector3(-1600, currentPanel.transform.localPosition.y), 0.25f)
                .OnComplete(()=>
                {
                    prevPanel = currentPanel;
                    currentPanel = panel;
                });
        }

        public void ReturnToPrevPanel()
        {
            nextPanel = prevPanel;
            currentPanel.transform.DOLocalMove(new Vector3(1600, currentPanel.transform.localPosition.y), 0.25f);
            prevPanel.transform.DOLocalMove(Vector3.zero, 0.25f)
                .OnComplete(()=>
                {
                    prevPanel = currentPanel;
                    currentPanel = nextPanel;
                    nextPanel = null;
                });
        }
    }
}
