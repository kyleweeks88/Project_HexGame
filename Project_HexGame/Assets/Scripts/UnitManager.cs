using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I NEED TO REWATCH THE ENTIRE TUTORIAL VIDEO FOR THIS CLASS!!!
// DON'T REALLY UNDERSTAND CLEARLY HOW ALL THISS WORKS YET!!!
public class UnitManager : MonoBehaviour
{
    [SerializeField] HexGrid hexGrid;

    [SerializeField] MovementSystem movementSystem;

    public bool PlayersTurn { get; private set; } = true;

    [SerializeField] Unit selectedUnit;
    HexTile previouslySelectedHex;

    public void HandleUnitSelected(GameObject _unit)
    {
        if (!PlayersTurn) { return; }

        Unit unitRef = _unit.GetComponent<Unit>();

        if (CheckIfTheSameUnitSelected(unitRef)) { return; }

        PrepareUnitForMovement(unitRef);
    }

    private bool CheckIfTheSameUnitSelected(Unit _unitRef)
    {
        if(this.selectedUnit == _unitRef)
        {
            ClearOldSelection();
            return true;
        }
        return false;
    }

    public void HandleTerrainSelected(GameObject _hexGO)
    {
        if(selectedUnit == null || !PlayersTurn) { return; }

        HexTile selectedHex = _hexGO.GetComponent<HexTile>();

        if(HandleHexOutOfRange(selectedHex.HexCoords) ||
            HandleSelectedHexIsUnitHex(selectedHex.HexCoords)) { return; }

        HandleTargetHexSelected(selectedHex);
    }

    private void PrepareUnitForMovement(Unit _unitRef)
    {
        if (this.selectedUnit != null)
            ClearOldSelection();

        this.selectedUnit = _unitRef;
        this.selectedUnit.Select();
        movementSystem.ShowRange(this.selectedUnit, this.hexGrid);
    }

    private void ClearOldSelection()
    {
        previouslySelectedHex = null;
        this.selectedUnit.Deselect();
        movementSystem.HideRange(this.hexGrid);
        this.selectedUnit = null;
    }

    private void HandleTargetHexSelected(HexTile _selectedHex)
    {
        if(previouslySelectedHex == null || previouslySelectedHex != _selectedHex)
        {
            previouslySelectedHex = _selectedHex;
            movementSystem.ShowPath(_selectedHex.HexCoords, this.hexGrid);
        }
        else
        {
            movementSystem.MoveUnit(selectedUnit, this.hexGrid);
            PlayersTurn = false;
            selectedUnit.OnMovementFinished += ResetTurn;
            ClearOldSelection();
        }
    }

    private bool HandleSelectedHexIsUnitHex(Vector3Int _hexPos)
    {
        if(_hexPos == hexGrid.GetClosestHex(selectedUnit.transform.position))
        {
            selectedUnit.Deselect();
            ClearOldSelection();
            return true;
        }
        return false;
    }

    private bool HandleHexOutOfRange(Vector3Int _hexPos)
    {
        if(movementSystem.IsHexInRange(_hexPos) == false)
        {
            // HERE THERE SHOULD BE SOME FEEDBACK LETTING THE PLAYER KNOW
            // THAT THE SELECTED HEXTILE IS OUT OF RANGE OF THE UNIT'S MOVEMENT
            Debug.Log("Hex out of range!");
            return true;
        }
        return false;
    }

    private void ResetTurn(Unit _selectedUnit)
    {
        _selectedUnit.OnMovementFinished -= ResetTurn;
        PlayersTurn = true;
    }
}
