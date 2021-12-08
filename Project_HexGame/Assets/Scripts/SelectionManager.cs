using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    public LayerMask selectionMask;
    public HexGrid hexGrid;

    List<Vector3Int> neighbors = new List<Vector3Int>();

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void HandleClick(Vector3 mousePos)
    {
        GameObject result;
        if(FindTarget(mousePos, out result))
        {
            HexTile selectedHexTile = result.GetComponent<HexTile>();

            selectedHexTile.DisableHighlight();
            foreach (Vector3Int neighbor in neighbors)
            {
                hexGrid.GetTileAt(neighbor).DisableHighlight();
            }

            // Create a new struct (which exists inside the GraphSearch class) and -
            // populate it by calling the function BFSGetRange inside GraphSearch -
            // passing in the active hexGrid, the hextile we've selected by mouse -
            // and a hard-coded movement value of 20 points.
            BFSResult bfsResult = GraphSearch.BFSGetRange(hexGrid, selectedHexTile.HexCoords, 20);

            // This List<Vector3Int> neighbors will be populated by our bfsResult.
            // I DONT UNDERSTAND HOW THE COROUTINE 'GetRangePositions' WORKS RIGHT NOW!
            neighbors = new List<Vector3Int>(bfsResult.GetRangePositions());

            foreach (Vector3Int neighbor in neighbors)
            {
                hexGrid.GetTileAt(neighbor).EnableHighlight();
            }

            Debug.Log($"Neighbors for {selectedHexTile.HexCoords} are: ");
            foreach (Vector3Int neighborPos in neighbors)
            {
                Debug.Log(neighborPos);
            }
        }
    }

    private bool FindTarget(Vector3 mousePos, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if(Physics.Raycast(ray, out hit, selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }
}
