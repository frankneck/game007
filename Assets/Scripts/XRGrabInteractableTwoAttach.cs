using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    [Header("Attaches")]
    public Transform leftHandAttach;
    public Transform rightHandAttach;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log(args.interactorObject.transform.tag);
        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            Debug.Log($"I work! {args.interactorObject.transform.name} ");
            attachTransform = leftHandAttach;
        }
        else if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            Debug.Log($"I work! {args.interactorObject.transform.tag}");
            attachTransform = rightHandAttach;
        }
        else
        {
            Debug.Log("I don't work! " + args.interactorObject.transform.name);
        }

        base.OnSelectEntered(args);
    }
}
