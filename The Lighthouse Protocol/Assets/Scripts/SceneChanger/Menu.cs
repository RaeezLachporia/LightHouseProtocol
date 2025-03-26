using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void backmenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        SceneManager.LoadScene(2);
    }

    public void TutorialTower()
    {
        SceneManager.LoadScene(4);
    }

    public void TutorialExp()
    {
        SceneManager.LoadScene(5);
    }

    public void TestTower()
    {
        SceneManager.LoadScene(1);
    }

    public void TestExp()
    {
        SceneManager.LoadScene(3);
    }

}
