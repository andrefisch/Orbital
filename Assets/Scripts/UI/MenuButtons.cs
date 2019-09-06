using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PlayArcadeMode()
    {
        SceneManager.LoadScene("Main");
    }

    public void PlayMasochistMode()
    {
        SceneManager.LoadScene("Masochist");
    }

    public void HighScores()
    {
        SceneManager.LoadScene("HighScores");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
