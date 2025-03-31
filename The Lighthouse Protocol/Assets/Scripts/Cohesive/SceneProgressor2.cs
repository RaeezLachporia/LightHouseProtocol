using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneProgressor2 : MonoBehaviour
{

    private Camera playerCamera;

    public string progressObjectTag = "Progress"; // Tag assigned to the "Progress" GameObject
    public float interactionRange = 3f; // Distance at which players can interact

    void Start()
    {
        playerCamera = Camera.main;  // Initialize the camera
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3f))
            {
                if (hit.collider.gameObject.CompareTag("Progress"))
                {
                    SceneManager.LoadScene(1); // Ensure this only runs for the correct object
                }
            }
        }
    }

    void TryProgressScene()
    {
        GameObject progressObject = GameObject.FindGameObjectWithTag(progressObjectTag);
        if (progressObject != null)
        {
            float distance = Vector3.Distance(transform.position, progressObject.transform.position);
            if (distance <= interactionRange)
            {
                SceneManager.LoadScene(1); // Loads scene index 1
            }
        }
    }
}