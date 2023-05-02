using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    [SerializeField]
    private bool _turnRotationOn;

    [SerializeField]
    private Vector3 _rotation;

    private void Update()
    {
        transform.position += -Vector3.forward * _movementSpeed * Time.deltaTime;

        if (_turnRotationOn) transform.Rotate(_rotation * _rotationSpeed * Time.deltaTime);
    }
}
