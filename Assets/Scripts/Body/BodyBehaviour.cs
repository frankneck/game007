using UnityEngine;

public class FootIK : MonoBehaviour
{
    public Transform rightFootTarget; // RIGHT LEG IK_target
    public Transform leftFootTarget;  // LEFT LEG IK_target
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        // Raycast для правой ноги
        RaycastHit hit;
        if (Physics.Raycast(rightFootTarget.position + Vector3.up * 1f, Vector3.down, out hit, 2f))
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + Vector3.up * 0.1f);
        }

        // Raycast для левой ноги
        if (Physics.Raycast(leftFootTarget.position + Vector3.up * 1f, Vector3.down, out hit, 2f))
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + Vector3.up * 0.1f);
        }
    }
}