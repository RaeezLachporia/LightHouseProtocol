using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight; // Assign the Spotlight here
    public KeyCode toggleKey = KeyCode.F; // Default key to toggle flashlight

    private bool isOn = false;

    void Start()
    {
        if (flashlight != null)
            flashlight.enabled = false; // Flashlight starts off
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }
    }

    void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlight.enabled = isOn;
    }
}