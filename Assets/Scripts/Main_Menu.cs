using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Main_Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("DJD2_Puzzles");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
