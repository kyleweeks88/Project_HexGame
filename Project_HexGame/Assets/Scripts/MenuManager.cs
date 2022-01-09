using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] TextMeshProUGUI _stateText;

    private void Awake()
    {
        GameManager.OnTurnPhaseChanged += TurnPhaseChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnTurnPhaseChanged -= TurnPhaseChanged;
    }

    private void TurnPhaseChanged(TurnPhase _turn)
    {

    }
}