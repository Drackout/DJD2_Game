using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValidateCode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answearCode;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject PuzzleItself;


    public void validateCode(InteractiveData interactiveData)
    {
        if (answearCode.text == interactiveData.code.ToString())
        {
            answearCode.text = "Correct!";

            //play certain animation
            anim.SetTrigger("Interaction");
            //anim.GetComponent<Animator>().Play("Test_Paint"); //also works

            //disable puzzle
            PuzzleItself.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            answearCode.text = "WRONG!";
        }
    }

}
