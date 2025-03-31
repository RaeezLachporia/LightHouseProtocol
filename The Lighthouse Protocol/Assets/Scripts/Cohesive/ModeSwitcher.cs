using UnityEngine;

public class ModeSwitcher : MonoBehaviour
{
    public GameObject player; // Assign the First-Person Player GameObject
    public Camera mainCamera; // Assign the Top-Down Camera
    private GameObject uiSystem; // Store the UI reference

    private bool isInFirstPerson = false;

    void Start()
    {
        // Find the UI System once and store it
        uiSystem = GameObject.Find("UISystem");

        // Ensure UI System is active at the start
        if (uiSystem != null)
            uiSystem.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Ensure the cursor is visible

        RenderSettings.fog = false;

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
        isInFirstPerson = !isInFirstPerson; // Properly toggle the state

        if (isInFirstPerson)
        {
            // Switch to First-Person Mode
            mainCamera.gameObject.SetActive(false);
            player.SetActive(true);
            RenderSettings.fog = true;
            RenderSettings.fogDensity = 0.03f;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

            // Disable UI System if it exists
            if (uiSystem != null)
                uiSystem.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // Hide cursor in first-person

            Debug.Log("Switched to First-Person Mode");
        }
        else
        {
            // Switch to Top-Down Mode
            player.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            RenderSettings.fog = false;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;

            // Enable UI System if it exists
            if (uiSystem != null)
                uiSystem.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; // Show cursor in top-down

            Debug.Log("Switched to Top-Down Mode");
        }
    }
}