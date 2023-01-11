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

    public void GoToHow()
    {
        SceneManager.LoadScene("How_To_Play");
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Main_Menu");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
