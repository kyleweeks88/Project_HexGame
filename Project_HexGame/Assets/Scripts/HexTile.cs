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

    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    private void Awake()
    {
        //hexCoordinates = GetComponent<HexCoordinates>();
        //highlight = GetComponent<GlowHighlight>();

        hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();
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
