using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text days;
    public Text gold;
    

    private void OnEnable()
    {
        days.text = "Days survived " + SaveSystem.LoadDays();
        gold.text = "Coins: " + SaveSystem.GetLoadCoins();
    }
    public void DeleteSaves()
    {
        string path = Application.persistentDataPath + "/Upgrades.inf";
        try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        path = Application.persistentDataPath + "/Coins.inf";
        try
        {
            File.Delete(path);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        path = Application.persistentDataPath + "/Days.inf";
        try
        {
            File.Delete(path);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        days.text = "Days survived " + SaveSystem.LoadDays();
        gold.text = "Coins: " + SaveSystem.GetLoadCoins();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MapGenerationSettings");
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
