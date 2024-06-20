using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLookAt : MonoBehaviour
{
    private Animator _animator;
    private Camera _mainCamera;

    [Range(0f, 1f), SerializeField] private float weight = 1f;
    [Range(0f, 1f), SerializeField] private float bodyWeight = .5f;
    [Range(0f, 1f), SerializeField] private float headWeight = 1f;
    [Range(0f, 1f), SerializeField] private float eyesWeight = .5f;
    [Range(0f, 1f), SerializeField] private float clampWeight = .5f;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main!;
    }

    private void OnAnimatorIK()
    {
        _animator.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
        Ray lookAtRay = new Ray(transform.position, _mainCamera.transform.forward);
        var lookAtPos = lookAtRay.GetPoint(25);
        lookAtPos.y = _animator.GetBoneTransform(HumanBodyBones.Head).position.y;
        _animator.SetLookAtPosition(lookAtPos);
    }
}
