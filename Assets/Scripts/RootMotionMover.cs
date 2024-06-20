using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionMover : MonoBehaviour
{
    public bool useGravity = true;

    private Vector3 rootMotion;
    private Vector3 rootRotation;
    private Animator _animator;
    private CharacterController _cc;
    private float _velocityY;

    private bool _wasGroundedLastFrame;
    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        //_animator.SetBool("IsGrounded", _cc.isGrounded);
        //if (Input.GetKeyDown(KeyCode.Space) && _cc.isGrounded) {
        //    _animator.SetTrigger("Jump");
        //    _animator.SetBool("IsGrounded", false);
        //}
    }

    private void OnAnimatorMove()
    {
        if (_cc == null) return;
        rootMotion += _animator.deltaPosition;
        rootRotation += _animator.deltaRotation.eulerAngles;
        if (!useGravity) return;

        _velocityY += -9.81f * Time.deltaTime * Time.deltaTime;

        //if(!_wasGroundedLastFrame && _cc.isGrounded)
        //{
        //    _animator.SetFloat("LandingVelocity", _velocityY);
        //    _animator.SetTrigger("Land");
        //}

        if (_cc.isGrounded)
        {
            _velocityY = -0.2f;
            //_animator.SetFloat("LandingVelocity", _velocityY);
        }


        rootMotion.y += _velocityY;
        _animator.SetFloat("VelocityY", _velocityY);
    }

    private void LateUpdate()
    {
        if (_cc == null) return;
        _cc.Move(rootMotion);
        transform.eulerAngles += rootRotation;
        rootMotion = Vector3.zero;
        rootRotation = Vector3.zero;
        _wasGroundedLastFrame = _cc.isGrounded;
    }
}
