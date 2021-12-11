using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    [SerializeField] NavMeshSurface navSurface;
    [SerializeField] string bakeableLayer;
    [SerializeField] string originalLayer;

    // !!!TESTING!!! //
    [SerializeField] Unit unit;

    private void Awake()
    {
        navSurface = GetComponent<NavMeshSurface>();
        unit.OnMovementStarted += PrepareTilesForNavMesh;
        unit.OnMovementFinished += ClearMesh;
    }

    void PrepareTilesForNavMesh(List<Vector3Int> _currentPath, HexGrid _hexGrid)
    {
        foreach (Vector3Int hexPos in _currentPath)
        {
            // Get the original layer name

            //Change the layer name to be bakeable
            _hexGrid.GetTileAt(hexPos).gameObject.GetComponentInChildren<MeshCollider>().gameObject.layer = LayerMask.NameToLayer("HexTile_Bakeable");
            // build navmesh
            BakeMesh(null);
        }
    }

    void BakeMesh(Unit _unit)
    {
        navSurface.BuildNavMesh();
    }

    void ClearMesh(Unit _unit)
    {
        navSurface.RemoveData();
    }
}
