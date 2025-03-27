using UnityEngine;

public class ModeSwitcher : MonoBehaviour
{
    public GameObject player; // Assign the First-Person Player GameObject
    public Camera mainCamera; // Assign the Top-Down Camera

    private bool isInFirstPerson = false;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Automatically find the Main Camera if not assigned
        }

        if (player != null)
        {
            player.SetActive(false); // Start in Top-Down Mode
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleMode();
        }
    }

    void ToggleMode()
    {
        isInFirstPerson = !isInFirstPerson;

        if (isInFirstPerson)
        {
            // Switch to First-Person Mode
            mainCamera.gameObject.SetActive(false);
            player.SetActive(true);
        }
        else
        {
            // Switch to Top-Down Mode
            player.SetActive(false);
            mainCamera.gameObject.SetActive(true);
        }
    }
}