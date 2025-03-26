using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneProgressor : MonoBehaviour
{
    public string progressObjectTag = "Progress"; // Tag assigned to the "Progress" GameObject
    public float interactionRange = 3f; // Distance at which players can interact

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryProgressScene();
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