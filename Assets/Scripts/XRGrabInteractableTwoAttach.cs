using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    [Header("Attaches")]
    public Transform rightAttachPoint;
    public Transform leftAttachPoint;


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Right Hand") && rightAttachPoint != null)
        {
            attachTransform = rightAttachPoint;
        }
        else if (args.interactorObject.transform.CompareTag("Left Hand") && leftAttachPoint != null)
        {
            attachTransform = leftAttachPoint;
        }

        base.OnSelectEntered(args);
    }
}
