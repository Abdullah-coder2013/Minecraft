using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class DieMenu : MonoBehaviour
{
    public GameObject dieMenu;
    public GameObject dieBackground;

    public World world;

    public Player player;

    public void Respawn() {
        world.UIforDying = false;
        dieMenu.SetActive(false);
        dieBackground.SetActive(false);
        player.transform.position = new Vector3(player.transform.position.x, 200f, player.transform.position.z);
    }

    public void Die() {
        world.UIforDying = true;
        dieMenu.SetActive(true);
        dieBackground.SetActive(true);
    }

    public void Quit() {
        world.inUI = false;
        dieMenu.SetActive(false);
        dieBackground.SetActive(false);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
