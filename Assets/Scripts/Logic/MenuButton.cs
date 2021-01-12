using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Logic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public string nextScene;
    public Image nextPanel;

    public void OnClick()
    {
        if (nextPanel != null)
            MainMenuManager.Instance.TweenPanels(nextPanel);
        else
        {
            SceneManager.LoadScene(nextScene);
        }

    }

}
