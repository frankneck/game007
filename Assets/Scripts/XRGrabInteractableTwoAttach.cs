using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    [Header("Attaches")]
    public Transform rightAttachPoint;
    public Transform leftAttachPoint;
    public Transform rightArmIKTarget;
    public Transform leftArmIKTarget;
    public VRMap rightHandVRMap;
    public VRMap leftHandVRMap;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachPoint;
            rightHandVRMap.SetTarget(rightAttachPoint);
        }
        else if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachPoint;
            leftHandVRMap.SetTarget(leftAttachPoint);
        }

        base.OnSelectEntered(args);
    }
    
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            rightHandVRMap.SetTarget(rightArmIKTarget); // XR Controller
        }
        else if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            leftHandVRMap.SetTarget(leftArmIKTarget); // XR Controller
        }

        base.OnSelectExited(args);
    }
}
