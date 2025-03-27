using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    public GameObject SceneStarter;
    public GameObject PlayerUI;
    public GameObject PlayerPause;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        PlayerUI.SetActive(false);
        PlayerPause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnsceneStart()
    {
        PlayerUI.SetActive(true);
        Time.timeScale = 1;
        SceneStarter.SetActive(false);
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            SceneStarter.SetActive(false);
        }*/

    }
}
