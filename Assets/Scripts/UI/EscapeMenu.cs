using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public World world;
    public GameObject escapeMenu;
    public GameObject escapeBackground;

    public void BacktoGame() {
        world.UIforEscaping = false;
        escapeBackground.SetActive(false);
        escapeMenu.SetActive(false);
    }

    public void Quit() {
        world.inUI = false;
        escapeBackground.SetActive(false);
        escapeMenu.SetActive(false);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Escape() {
        world.UIforEscaping = true;
        escapeBackground.SetActive(true);
        escapeMenu.SetActive(true);
    }
}
