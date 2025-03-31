using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu1 : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject UpgrademenuUI;
    public GameObject MainUI;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuUI.SetActive(false);
        UpgrademenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuUI.active = true;

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.active = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        SceneManager.LoadScene(2);
    }

    public void OpenUpgrades()
    {
        Time.timeScale = 0f;
        MainUI.SetActive(false);
        UpgrademenuUI.SetActive(true);
    }

    public void CloseUpgrade()
    {
        Time.timeScale = 1f;
        UpgrademenuUI.SetActive(false);
        MainUI.SetActive(true);
    }
       
}