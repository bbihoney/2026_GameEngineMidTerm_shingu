using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject leaderPanel;

    public void GameStartButtonAction()
    {
         SceneManager.LoadScene("Level_1");
    }

    public void OpenHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        helpPanel.SetActive(false);
    }
    public void OpenLeaderPanel()
    {
        leaderPanel.SetActive(true);
    }

    public void CloseLeaderPanel()
    {
        leaderPanel.SetActive(false);
    }
    public void GameClose()
    {
        Application.Quit();
    }
}