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
            Debug.Log("I work! Left...");
            attachTransform = leftHandAttach;
        }
        else if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            Debug.Log("I work! Right...");
            attachTransform = rightHandAttach;
        }

        base.OnSelectEntered(args);
    }
}
