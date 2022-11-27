using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {
    public void ChangeScene(int sceneNumber) {
        SceneManager.LoadScene(sceneNumber);
    }

    public void QuitGame() {
        Application.Quit();
    }
    public void RestartButtonPressed(int sceneNumber) {
        SceneManager.LoadScene(sceneNumber);
    }
    public void MenuButtonPressed(int sceneMenu) {
        SceneManager.LoadScene(sceneMenu);
    }
}
