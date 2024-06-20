using UnityEngine;

public class FootIK : MonoBehaviour
{
    [SerializeField] private bool useIK;
    [SerializeField] private float footIKDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float distanceBeforeIK = 0.05f;
    [SerializeField] private Vector3 rightFootPositionOffset = Vector3.zero;
    [SerializeField] private Vector3 rightFootRotationOffset = Vector3.zero;
    [SerializeField] private Vector3 leftFootPositionOffset = Vector3.zero;
    [SerializeField] private Vector3 leftFootRotationOffset = Vector3.zero;

    private Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnAnimatorIK(int layerIndex)
    {

        if (useIK)
        {
            Ray rightFootRay = new Ray(_animator.GetBoneTransform(HumanBodyBones.RightFoot).position, Vector3.down);
            Physics.Raycast(rightFootRay, out RaycastHit rightFootHit, footIKDistance, groundLayer);
            if ((rightFootHit.collider == null))
            {
                Debug.DrawRay(rightFootRay.origin, rightFootRay.direction, Color.red);
            }
            else
            {
                Debug.DrawLine(rightFootRay.origin, rightFootHit.point, Color.green);
                _animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootHit.point + rightFootPositionOffset);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                _animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(Vector3.Cross(rightFootHit.normal, transform.right) * -1f) * Quaternion.Euler(rightFootRotationOffset));
                _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
            }

            Ray leftFootRay = new Ray(_animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, Vector3.down);
            Physics.Raycast(leftFootRay, out RaycastHit leftFootHit, footIKDistance, groundLayer);

            if ((leftFootHit.collider == null))
            {
                Debug.DrawRay(leftFootRay.origin, leftFootRay.direction, Color.red);
            }
            else
            {
                Debug.DrawLine(leftFootRay.origin, leftFootHit.point, Color.green);
                _animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootHit.point + leftFootPositionOffset);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                _animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(Vector3.Cross(leftFootHit.normal, transform.right) * -1f) * Quaternion.Euler(leftFootRotationOffset));
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            }
        }

    }
}
