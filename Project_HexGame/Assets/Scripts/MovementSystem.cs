using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// I NEED TO REWATCH THE ENTIRE TUTORIAL VIDEO FOR THIS CLASS!!!
// DON'T REALLY UNDERSTAND CLEARLY HOW ALL THISS WORKS YET!!!
public class MovementSystem : MonoBehaviour
{
    BFSResult movementRange = new BFSResult();
    List<Vector3Int> currentPath = new List<Vector3Int>();

    public void HideRange(HexGrid _hexGrid)
    {
        foreach (Vector3Int hexPos in movementRange.GetRangePositions())
        {
            _hexGrid.GetHexTileAt(hexPos).DisableHighlight();
        }
        movementRange = new BFSResult();
    }

    public void ShowRange(Unit _selectedUnit, HexGrid _hexGrid)
    {
        CalculateRange(_selectedUnit, _hexGrid);

        Vector3Int unitPos = _hexGrid.GetClosestHexCoords(_selectedUnit.transform.position);

        foreach (Vector3Int hexPos in movementRange.GetRangePositions())
        {
            if (unitPos == hexPos)
                continue;
            _hexGrid.GetHexTileAt(hexPos).EnableHighlight();
        }
    }

    public void CalculateRange(Unit _selectedUnit, HexGrid _hexGrid)
    {
        movementRange = GraphSearch.BFSGetRange(_selectedUnit, _hexGrid,
            _hexGrid.GetClosestHexCoords(_selectedUnit.transform.position),
            _selectedUnit.MovementPoints);
    }

    public void ShowPath(Unit _selectedUnit, Vector3Int _selectedHexPos, HexGrid _hexGrid)
    {
        if(movementRange.GetRangePositions().Contains(_selectedHexPos))
        {
            foreach (Vector3Int hexPos in currentPath) 
            {
                _hexGrid.GetHexTileAt(hexPos).ResetHighlight();
            }

            currentPath = movementRange.GetPathTo(_selectedHexPos);

            foreach (Vector3Int hexPos in currentPath)
            { 
                _hexGrid.GetHexTileAt(hexPos).HighlightPath();
                _selectedUnit.currentPath.Add(hexPos);
            }
        }
    }

    public void MoveUnit(Unit _selectedUit, HexGrid _hexGrid)
    {
        Debug.Log($"Moving unit {_selectedUit.name}.");

        // Here we want to select the default positions of the Unit's transforms -
        // and NOT the hex coordinates from the HexGrid.
        // I DON'T UNDERSTAND HOW THIS FUNCTION WORKS.
        // (TUTORIAL: Hex Grid Movement P4 - Character Movement P1 - 18:00)
        _selectedUit.MoveThroughPath(currentPath.Select(
            pos => _hexGrid.GetHexTileAt(pos).transform.position).ToList());
    }

    public bool IsHexInRange(Vector3Int _hexPos)
    {
        return movementRange.IsHexPositionInRange(_hexPos);
    }
}
