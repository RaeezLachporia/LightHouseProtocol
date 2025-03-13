using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacementScripts : MonoBehaviour
{
    public List<GameObject> towerPrefabs;
    public LayerMask placementLayer;
    public float gridSize = 2f;
    public GameObject ghostTowerPrefab;
    private GameObject currentGhostTower;
    private Camera cam;
    private bool isPlacingTower = false;
    private int TowerIndex = -1;
    void Start()
    {
        cam = Camera.main;
    }

    
    void Update()
    {
        if (isPlacingTower || TowerIndex == -1) return;
        
            Vector3 worldPos = GetMouseWorldPos();
            Vector3 snapPos = snapToGrid(worldPos);
        

        if (currentGhostTower != null)
        {
            currentGhostTower.transform.position = snapPos;
            currentGhostTower.GetComponent<Renderer>().material.color = IsvalidPlacement(snapPos) ? Color.green : Color.red;
        }

        if (Input.GetMouseButtonDown(0) && IsvalidPlacement(snapPos))
        {
            PlaceTowers(snapPos);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, Mathf.Infinity, placementLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private Vector3 snapToGrid(Vector3 worldPos)
    {
        float snapX = Mathf.Round(worldPos.x / gridSize) * gridSize;
        float snapZ = Mathf.Round(worldPos.z / gridSize) * gridSize;
        return new Vector3(snapX, 0f, snapZ);
    }

    private bool IsvalidPlacement(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, gridSize / 2f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Tower"))
            {
                return false;
            }
        }
        return true;
    }

    private void PlaceTowers(Vector3 position)
    {
        GameObject newTower = Instantiate(towerPrefabs[TowerIndex], position, Quaternion.identity);
        newTower.tag = "Tower";
        Debug.Log("Tower placed: " + towerPrefabs[TowerIndex].name);
    }

    public void SelectTOwer(int index)
    {
        TowerIndex = index;
        Debug.Log("Selected tower: " + towerPrefabs[TowerIndex].name);

        if (currentGhostTower != null)
        {
            Destroy(currentGhostTower);
        }
        currentGhostTower = Instantiate(towerPrefabs[TowerIndex]);
        SetGhostTransparency(currentGhostTower, 0.5f);
    }

    private void SetGhostTransparency(GameObject ghost, float alpha)
    {
        Renderer rend = ghost.GetComponent<Renderer>();
        if (rend != null)
        {
            Color color = rend.material.color;
            color.a = alpha;
            rend.material.color = color;
        }
    }
}
