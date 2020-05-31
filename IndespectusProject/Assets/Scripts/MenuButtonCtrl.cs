using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonCtrl : MonoBehaviour {

    public GameObject instructPanel;
    public void PlayBtn() {
        SceneManager.LoadScene(1);       
    }

    public void InstructBtn() {
        instructPanel.SetActive(true);
    }

    public void BackBtn() {
        instructPanel.SetActive(false);
    }

    public void ExitBtn() {
        Application.Quit();
    }

   
}
