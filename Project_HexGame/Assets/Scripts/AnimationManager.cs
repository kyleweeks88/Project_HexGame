using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator myAnimator;
    [SerializeField] Unit unit;
    int isMovingParam = Animator.StringToHash("isMoving");

    private void Awake()
    {
        myAnimator = GetComponentInChildren<Animator>();
        unit = GetComponent<Unit>();
        //unit.OnMovementFinished += HandleMoveAnimation;
    }

    private void Update()
    {
        myAnimator.SetBool(isMovingParam, unit.isMoving);
    }
}
