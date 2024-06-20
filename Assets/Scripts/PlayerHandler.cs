using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private static int SpeedHash = Animator.StringToHash("Speed");

    public Transform Model;

    [Range(1f, 20f)]
    [SerializeField]
    private float rotationSpeed;

    [SerializeField] private AnimationCurve transitionCurve;

    private Animator _animator;
    private Vector3 _stickDirection;
    private Camera _mainCamera;
    private float _targetSpeed;
    private float _crouching;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main!;
        _animator = Model.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _stickDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        HandleInputData();
        HandleRotation();
    }

    private void HandleInputData()
    {
        _targetSpeed = Mathf.Lerp(_targetSpeed, Input.GetKey(KeyCode.LeftControl) ? 1.5f : (Input.GetKey(KeyCode.LeftShift) ? 6f : 2f), transitionCurve.Evaluate(10 * Time.deltaTime));
        _crouching = Mathf.Lerp(_crouching, Input.GetKey(KeyCode.LeftControl) ? 1f : 0f, 10 * Time.deltaTime);
        _animator.SetFloat(SpeedHash, Vector3.ClampMagnitude(_stickDirection,1).magnitude * _targetSpeed);
        if (Vector3.Dot(Model.forward, Vector3.ProjectOnPlane(_mainCamera.transform.TransformDirection(_stickDirection), Vector3.up)) >= 0.7f) _animator.SetFloat("Leaning", Vector3.SignedAngle(Model.forward, Vector3.ProjectOnPlane(_mainCamera.transform.TransformDirection(_stickDirection), Vector3.up), Vector3.up));
        _animator.SetFloat("Crouching", _crouching);
    }

    private void HandleRotation()
    {
        Vector3 rotationOffset = _mainCamera.transform.TransformDirection(_stickDirection);
        rotationOffset.y = 0f;
        float lookDirection = Vector3.SignedAngle(Model.forward, Vector3.ProjectOnPlane(_mainCamera.transform.forward, Vector3.up), Vector3.up);
        _animator.SetFloat("LookDirection", lookDirection);
        Model.forward += Vector3.Slerp(Model.forward, rotationOffset, Time.deltaTime * rotationSpeed);
    }
}
