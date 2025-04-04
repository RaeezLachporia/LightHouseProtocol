using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FirstPersonController : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float runSpeed = 9f;
    public float jumpForce = 3.2f;
    public float gravity = 80f;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Transform cameraTransform;
    private float xRotation = 0f;

    // Stamina Variables
    public float maxStamina = 100f;
    private float currentStamina;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;
    public float staminaRegenDelay = 2f;
    private float lastSprintTime;

    public float jumpStaminaCost = 10f;

    // Health Variables
    public float maxHealth = 100f;
    private float currentHealth;

    // Crouch Variables
    public float crouchSpeed = 3f;
    public float crouchHeight = 1f;
    public float normalHeight = 2f;
    private bool isCrouching = false;

    // UI References
    public Slider staminaBar;
    public Slider healthBar;
    public TextMeshProUGUI collectedResourcesText;
    public TextMeshProUGUI holdingResourcesText; // NEW UI for held resources
    public GameObject PauseMenu;

    // Resource Tracking
    private Dictionary<string, int> heldResources = new Dictionary<string, int>(); // NEW

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        currentStamina = maxStamina;
        currentHealth = maxHealth;

        // Find UI elements if not assigned
        if (collectedResourcesText == null)
            collectedResourcesText = GameObject.Find("CollectedResourcesText")?.GetComponent<TextMeshProUGUI>();

        if (holdingResourcesText == null) // NEW: Assign Holding UI
            holdingResourcesText = GameObject.Find("HoldingResourcesText")?.GetComponent<TextMeshProUGUI>();

        UpdateCollectedResourcesUI();
        UpdateHoldingResourcesUI(); // NEW

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
            ChangeSliderColor(staminaBar, Color.yellow);
        }

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            ChangeSliderColor(healthBar, Color.red);
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCrouching;
        float speed = isSprinting ? runSpeed : (isCrouching ? crouchSpeed : walkSpeed);

        if (isSprinting)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            lastSprintTime = Time.time;
        }

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && currentStamina > 0)
        {
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
            currentStamina = Mathf.Max(0, currentStamina - jumpStaminaCost);
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if (!isSprinting && Time.time > lastSprintTime + staminaRegenDelay)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        if (staminaBar != null) staminaBar.value = currentStamina;
        if (healthBar != null) healthBar.value = currentHealth;

        HandleCrouch();

        if (Input.GetKeyDown(KeyCode.E))
            PickupResource("Metal"); // Example pickup

        if (Input.GetKeyDown(KeyCode.G))
            DropResource("Metal"); // Example drop

        UpdateCollectedResourcesUI();
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            StandUp();
    }

    void Crouch()
    {
        if (controller != null)
        {
            controller.height = crouchHeight;
            isCrouching = true;
        }
    }

    void StandUp()
    {
        if (controller != null)
        {
            controller.height = normalHeight;
            isCrouching = false;
        }
    }

    void ChangeSliderColor(Slider slider, Color color)
    {
        Image fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage != null)
            fillImage.color = color;
    }

    // Update the UI for collected resources
    public void UpdateCollectedResourcesUI()
    {
        if (collectedResourcesText == null) return;

        Dictionary<string, int> resources = CollectionPoint.GetCollectedResources();
        collectedResourcesText.text = "Collected:\n";

        foreach (var resource in resources)
            collectedResourcesText.text += $"{resource.Key}: {resource.Value}\n";
    }

    // NEW: Update UI for holding resources
    void UpdateHoldingResourcesUI()
    {
        if (holdingResourcesText == null) return;

        if (heldResources.Count == 0)
        {
            holdingResourcesText.text = "Holding: None";
        }
        else
        {
            holdingResourcesText.text = "Holding:\n";
            foreach (var item in heldResources)
                holdingResourcesText.text += $"{item.Key}: {item.Value}\n";
        }
    }

    // NEW: Pickup logic
    void PickupResource(string resourceName)
    {
        if (!heldResources.ContainsKey(resourceName))
            heldResources[resourceName] = 0;
        heldResources[resourceName]++;

        UpdateHoldingResourcesUI();
    }

    // NEW: Drop logic
    void DropResource(string resourceName)
    {
        if (heldResources.ContainsKey(resourceName) && heldResources[resourceName] > 0)
        {
            heldResources[resourceName]--;

            if (heldResources[resourceName] <= 0)
                heldResources.Remove(resourceName);

            UpdateHoldingResourcesUI();
        }
    }

    public void EnterPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseMenu.SetActive(true);
    }
}