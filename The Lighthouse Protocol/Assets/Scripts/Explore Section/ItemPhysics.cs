using UnityEngine;

public class ItemPhysics : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Make sure the Player has the tag "Player"
        {
            Vector3 pushDirection = collision.contacts[0].point - transform.position;
            pushDirection.y = 0; // Prevents it from flying upward too much
            pushDirection.Normalize();

            rb.AddForce(pushDirection * 0.01f, ForceMode.Impulse); // Adjust force as needed
        }
    }
}