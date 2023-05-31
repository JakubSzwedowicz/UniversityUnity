using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Rendering;

public class Piece : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private Transform RootStartingPoint;

    private Vector3 _rightPosition;
    private Vector3 _startingPosition;
    SortingGroup _sortingGroup;
    public bool InRightPosition;
    private bool Selected;

    private bool _isMovingSmooth = false;
    private Vector3 _newPosition;
    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        _sortingGroup = GetComponent<SortingGroup>();
    }
    void Start()
    {
        _rightPosition = transform.position;
        _startingPosition = RootStartingPoint.position + new Vector3(Random.Range(-3f, 3f), Random.Range(3f, -3));
        SetStartingPosition();
    }

    private void Update()
    {
        if (_isMovingSmooth)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _newPosition, ref _velocity, _speed);
            if (transform.position == _newPosition)
            {
                _isMovingSmooth = false;
                _newPosition = Vector3.zero;
            }
        }
    }
    public void Select()
    {
        Selected = true;
        _sortingGroup.sortingOrder = 1;
    }

    public bool Deselect()
    {
        Selected = false;
        _sortingGroup.sortingOrder = 0;
        if (IsInRightPosition())
        {
            SetRightPosition();
            return true;
        }
        else
        {
            SetStartingPosition();
        }

        return false;
    }

    private bool IsInRightPosition()
    {
        if (Vector3.Distance(transform.position, _rightPosition) < 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetStartingPosition()
    {
        _newPosition = _startingPosition;
        _isMovingSmooth = true;
    }

    public void SetRightPosition()
    {
        transform.position = _rightPosition;
        InRightPosition = true;
        _isMovingSmooth = false;
    }

    public bool IsSelectable()
    {
        return !InRightPosition;
    }
}
