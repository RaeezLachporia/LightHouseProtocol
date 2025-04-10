using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TowerPlacementScripts : MonoBehaviour
{
    public List<GameObject> towerPrefabs;
    public List<int> towerPrices;
    public LayerMask placementLayer;
    public float gridSize = 2f;
    public GameObject ghostTowerPrefab;
    private GameObject currentGhostTower;
    private Camera cam;
    private bool isPlacingTower = false;
    private int TowerIndex = 0;
    //private int IndexForUpgrades = -1;
    private Dictionary<int, GameObject> placedTowers = new Dictionary<int, GameObject>();
    private int towerID = 0;
    public LaserBeam laserbeam = new LaserBeam();

    private GameObject selectedTower;
    void Start()
    {
        cam = Camera.main;
        
    }

    
    void Update()
    {
        if (isPlacingTower ) return;
        
            Vector3 worldPos = GetMouseWorldPos();
            Vector3 snapPos = snapToGrid(worldPos);
        

        if (currentGhostTower != null)
        {
            float ghostHeight = currentGhostTower.GetComponent<Renderer>().bounds.extents.y;
            Vector3 adjustedPos = new Vector3(snapPos.x, snapPos.y + ghostHeight, snapPos.z);

            currentGhostTower.transform.position = adjustedPos;

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
            if (col.gameObject==currentGhostTower)
            {
                continue;
            }
            if (col.CompareTag("PlacementLayer"))
            {
                return true;
            }
            if (col.CompareTag("Tower"))
            {
                return false;
            }

        }
        return true;
    }

    private void PlaceTowers(Vector3 position)
    {
        if (TowerIndex < 0 || TowerIndex >= towerPrices.Count) return;
        int IDCounter = towerID++;
        int towerCost = towerPrices[TowerIndex];
        if (GameManager.Instance.moneySpending(towerCost))
        {
            RaycastHit hit;
            Vector3 rayStart = position + Vector3.up * 5f;
            //Debug.DrawRay(rayStart)
            if (Physics.Raycast(position + Vector3.up * 5f, Vector3.down, out hit, 10f, placementLayer))
            {
                float towerHeight = towerPrefabs[TowerIndex].GetComponentInChildren<Renderer>().bounds.size.y;
                Vector3 adjustPosition = new Vector3(hit.point.x, hit.point.y + (towerHeight / 2), hit.point.z);
                GameObject newTower = Instantiate(towerPrefabs[TowerIndex], adjustPosition, Quaternion.identity);
                //newTower.tag = "Tower";
                Debug.Log("Tower has been place on the right layer and level" + towerPrefabs[TowerIndex]);
                placedTowers[IDCounter] = newTower;
                newTower.name = "Tower_" + IDCounter;
                Debug.Log("placed tower ID: " + IDCounter);
            }
            else
            {
                Debug.LogWarning("No valid ground");
            }
            

            if (currentGhostTower != null)
            {
                Destroy(currentGhostTower);
                currentGhostTower = null;
            }
            isPlacingTower = false;
            TowerIndex = -1;
            
        }
        else
        {
            Debug.Log("You dont have enough money to place the tower");
        }
       
    }

    public int CountTowers()
    {
        return placedTowers.Count;
    }
    public void SelectTOwer(int index)
    {
        TowerIndex = index;
        Debug.Log("Selected tower: " + towerPrefabs[TowerIndex].name + "| cost: " + towerPrices[TowerIndex]);

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

    public GameObject TowerIDChecker(int TowerId)
    {
        if (placedTowers.ContainsKey(towerID))
        {
            return placedTowers[towerID];
        }
        return null;
    }

    



}
