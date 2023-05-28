using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private Vector3 _offset = new Vector3(0, 0, -10);
    private InputReader _inputReader;
    private Vector2 _vel;
    private bool canMoveCamera;
    private float _xRotation = 0f;

    [SerializeField]
    private float sensitivity = 1f;

    [SerializeField]
    private Transform pivot;
    [SerializeField]
    private GameObject camera;
    private void Awake()
    {
        _inputReader = new InputReader();
        ListenInput();
    }

    private void ListenInput()
    {
        _inputReader.OnCameraMoveEvent += OnCameraMove;
        _inputReader.OnValidCameraMoveEvent += OnValidCameraMove;
    }

    private void OnValidCameraMove(bool obj)
    {
        canMoveCamera = obj;
    }

    private void OnCameraMove(Vector2 obj)
    {
        _vel = obj;
    }
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (canMoveCamera)
        {
            transform.Rotate(Vector3.up, _vel.x * sensitivity * Time.deltaTime);
            _xRotation -= _vel.y * sensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            pivot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        if (target != null)
        {
            transform.position = target.position;
        }
        camera.transform.localPosition = _offset;
    }
}
