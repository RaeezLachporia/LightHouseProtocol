using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHouseDetectionRingForNAV : MonoBehaviour
{
    public float DetectionRing = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRing);
    }
}
