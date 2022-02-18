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
    [SerializeField] Button nextButton_P1;
    [SerializeField] Button nextButton_P2;

    private void Start()
    {
        nextButton_P1.gameObject.SetActive(true);
        //nextButton_P2.gameObject.SetActive(false);
    }

    public void TurnPhaseChanged()
    {
        switch(GameManager.Instance.turnPhase)
        {
            case TurnPhase.Movement:
                // Activate/Deactivate appropriate UI elements.
                // Move to next phase.
                print("TESTY");
                //nextButton_P1.gameObject.SetActive(false);
                //GameManager.Instance.UpdateTurnPhase(TurnPhase.Combat);
                break;
            case TurnPhase.Combat:
                // Activate/Deactivate appropriate UI elements.
                // Move to next phase.
                GameManager.Instance.UpdateTurnPhase(TurnPhase.Movement);
                break;
        }
    }
}