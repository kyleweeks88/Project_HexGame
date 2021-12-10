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

    public void EnableHighlight() => highlight.ToggleGlow(true);

    public void DisableHighlight() => highlight.ToggleGlow(false);
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
