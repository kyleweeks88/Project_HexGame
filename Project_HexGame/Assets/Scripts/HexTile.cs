using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class HexTile : MonoBehaviour
{
    [SerializeField] GlowHighlight highlight;
    private HexCoordinates hexCoordinates;

    [SerializeField] HexType hexType;

    // !!!TESTING!!!
    [SerializeField] HexLevel hexLevel;

    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    private void Awake()
    {
        hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();
    }

    // !!!TESTING!!!
    public int GetCost2(HexTile _unitHex)
    {
        int cost = 0;
            
        if (_unitHex.hexLevel != this.hexLevel)
        {
            switch (hexLevel)
            {
                case HexLevel.LevelOne:
                    if(_unitHex.hexLevel.Equals(HexLevel.LevelTwo))
                    {
                        cost += 5;
                    }
                    else if(_unitHex.hexLevel.Equals(HexLevel.LevelThree))
                    {
                        // CHECK IF PLAYER CAN CLIMB DOWN
                        cost += 10;
                    }
                        break;
                case HexLevel.LevelTwo:
                    cost += 5;
                    break;
                case HexLevel.LevelThree:
                    if(_unitHex.hexLevel.Equals(HexLevel.LevelTwo))
                    {
                        cost += 5;
                    }
                    else if (_unitHex.hexLevel.Equals(HexLevel.LevelOne))
                    {
                        // CHECK IF PLAYER CAN JUMP DOWN
                        cost += 10;
                    }
                    break;
            }
        }

        switch(hexType)
        {
            case HexType.Path:
                cost += 5;
                break;
            case HexType.Default:
                cost += 10;
                break;
            case HexType.Difficult:
                cost += 20;
                break;
        }

        return cost;
    }

    public int GetCost()
        => hexType switch
        {
            HexType.Difficult => 20,
            HexType.Default => 10,
            HexType.Path => 5,
            _ => throw new System.Exception($"Hex of type {hexType} not supported.")
        };

    public bool IsObstacle()
    {
        return this.hexType == HexType.Obstacle;
    }

    public void EnableHighlight() => highlight.ShouldToggleGlow(true);

    public void DisableHighlight() => highlight.ShouldToggleGlow(false);

    internal void ResetHighlight()
    {
        highlight.ResetGlowHighlight();
    }

    internal void HighlightPath()
    {
        highlight.HighlightValidPath();
    }
}

public enum HexType
{
    None,
    Default,
    Difficult,
    Path,
    Water,
    Obstacle
}

public enum HexLevel
{
    LevelOne,
    LevelTwo,
    LevelThree
}
