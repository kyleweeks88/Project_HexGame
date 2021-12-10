using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    [SerializeField] int movementPoints = 20;
    public int MovementPoints { get => movementPoints; }

    [Tooltip("How long it takes the unit move to destination tile")]
    [SerializeField] float movementDuration = 1;
    [Tooltip("How long it takes the unit to rotate to face it's destination")]
    [SerializeField] float rotationDuration = 0.3f;

    GlowHighlight glowHighlight;
    // This queue path will be populated by the BFS which will be passed to this class.
    Queue<Vector3> pathPositions = new Queue<Vector3>();

    public event Action<Unit> MovementFinished;

    private void Awake()
    {
        glowHighlight = GetComponent<GlowHighlight>();
    }

    public void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        glowHighlight.ToggleGlow();
    }

    // Our BFS class will pass it's created path list to this class here.
    // This function will then dequeue positions one by one, assigning targets.
    public void MoveThroughPath(List<Vector3> _currentPath)
    {
        pathPositions = new Queue<Vector3>(_currentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
    }

    IEnumerator RotationCoroutine(Vector3 _endPos, float _rotDuration)
    {
        Quaternion startRot = transform.rotation;
        // this is to stop the unit from rotating on the x or z plane
        _endPos.y = transform.position.y;
        Vector3 dir = _endPos - transform.position;
        Quaternion endRot = Quaternion.LookRotation(dir, Vector3.up);

        // Approximately allows us to compare a float value more easily.
        // Abs gets us the absolute value of startRot and endRot to compare in the Dot Product .
        // The Dot product returns 1 if the unit is facing the desired direction -
        // otherwise it will return a value between 0 and 0.9 so we rotate.
        if(Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRot, endRot)), 1.0f) == false)
        {
            float timeElapsed = 0f;
            while(timeElapsed < _rotDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / _rotDuration;
                transform.rotation = Quaternion.Lerp(startRot, endRot, lerpStep);
                yield return null;
                //yield return new WaitForEndOfFrame();
            }
            transform.rotation = endRot;
        }
        // Once the rotation is complete and we are facing our new dest pos -
        // start to move the unit with this coroutine.
        StartCoroutine(MovementCoroutine(_endPos));
    }

    IEnumerator MovementCoroutine(Vector3 _endPos)
    {
        Vector3 startPos = transform.position;
        _endPos.y = startPos.y;
        float timeElapsed = 0f;

        while(timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPos, _endPos, lerpStep);
            yield return null;
        }
        transform.position = _endPos;

        if(pathPositions.Count > 0)
        {
            Debug.Log("Selecting the next position!");
            StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
        }
        else
        {
            Debug.Log("Movement finished!");
            MovementFinished?.Invoke(this);
        }
    }
}