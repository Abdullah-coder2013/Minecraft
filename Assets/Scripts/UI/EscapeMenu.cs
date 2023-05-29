using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public GameObject escapeMenu;
    public GameObject escapeBackground;

    public void BacktoGame() {
        World.Instance.UIforEscaping = false;
        escapeBackground.SetActive(false);
        escapeMenu.SetActive(false);
    }

    public void Quit() {
        // SaveSystem.SaveWorld(World.Instance.worldData);
        World.Instance.UIforEscaping = true;
        escapeBackground.SetActive(false);
        escapeMenu.SetActive(false);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Escape() {
        World.Instance.UIforEscaping = true;
        escapeBackground.SetActive(true);
        escapeMenu.SetActive(true);
    }
}
