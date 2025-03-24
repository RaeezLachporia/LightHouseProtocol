using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionPoint : MonoBehaviour
{
    public static Dictionary<string, int> collectedResources = new Dictionary<string, int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("mats"))
        {
            string resourceName = other.gameObject.name.Replace("(Clone)", "").Trim(); // Clean up the name

            if (collectedResources.ContainsKey(resourceName))
            {
                collectedResources[resourceName]++;
            }
            else
            {
                collectedResources[resourceName] = 1;
            }

            Debug.Log($"Collected {resourceName}. Total: {collectedResources[resourceName]}");
            Destroy(other.gameObject); // Remove resource from world
        }
    }

    public static Dictionary<string, int> GetCollectedResources()
    {
        return collectedResources;
    }
}