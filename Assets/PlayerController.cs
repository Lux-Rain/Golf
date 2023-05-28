using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rb;
    private CameraController _cameraController;
    private InputReader _inputReader;
    private bool _canShoot = false;

    [SerializeField]
    private CameraController cameraControllerPrefab;
    [SerializeField]
    private float force = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnStartLocalPlayer()
    {
        _cameraController = Instantiate(cameraControllerPrefab);
        _cameraController.SetTarget(transform);
        _inputReader = new InputReader();
        ListenInput();
        base.OnStartLocalPlayer();
    }

    private void ListenInput()
    {
        _inputReader.OnClickEvent += OnClick;
    }

    private void OnClick(bool obj)
    {
        if (obj)
        {
            Ray ray = Camera.main.ScreenPointToRay(_inputReader.MousePosition());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    _canShoot = true;
                    Debug.Log("Clicked on me");
                }
                else
                {
                    Debug.Log("Clicked on " + hit.transform.name);
                }
            }
        }
        else
        {
            if (_canShoot)
            {
                Vector2 playerScreenPos = Camera.main.WorldToViewportPoint(transform.position);
                Vector2 mousePos = Camera.main.ScreenToViewportPoint(_inputReader.MousePosition());
                Vector2 direction = (mousePos - playerScreenPos).normalized;
                Vector3 realDirection = new Vector3(direction.x, 0, direction.y);
                float distance = Vector2.Distance(playerScreenPos, mousePos);
                Debug.Log($"Direction: {direction}, Distance: {distance}");
                _canShoot = false;
                Vector3 force = -(_cameraController.transform.forward * direction.y + _cameraController.transform.right * direction.x) * (this.force * distance);
                rb.velocity = force;
            }
        }
    }
}
