using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;
    public TurnPhase turnPhase;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action<TurnPhase> OnTurnPhaseChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateGameState(GameState _newState)
    {
        gameState = _newState;

        switch (_newState)
        {
            case GameState.UnitPlacement:
                break;
            case GameState.Turn_P1:
                break;
            case GameState.Turn_P2:
                break;
            case GameState.Victory:
                break;
            case GameState.Deafeat:
                break;
        }

        OnGameStateChanged?.Invoke(_newState);
    }

    public void UpdateTurnPhase(TurnPhase _newPhase)
    {
        switch (_newPhase)
        {
            case TurnPhase.Movement:
                break;
            case TurnPhase.Combat:
                break;
        }

        OnTurnPhaseChanged?.Invoke(_newPhase);
    }
}

public enum GameState
{
    UnitPlacement,
    Turn_P1,
    Turn_P2,
    Victory,
    Deafeat
}

public enum TurnPhase
{
    Movement,
    Combat
}
