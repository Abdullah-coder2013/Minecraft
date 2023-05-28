using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class WorldCreation : MonoBehaviour
{
    public GameObject worldCreationMenu;
    public GameObject worldCreationBackground;
    public TMP_InputField worldName;
    public TMP_Dropdown worlds;

    string savesPath = World.Instance.appPath + "/saves/";

    private void Awake() {
        FillDropdown();
    }
    public void FillDropdown() {
        foreach (string s in Directory.GetFiles(savesPath)) {
            worlds.options.Add(new TMPro.TMP_Dropdown.OptionData(s));
        }
    }

    public void CreateWorld() {
        World.Instance.worldData = SaveSystem.LoadWorld(worldName.text);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
