using System;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SwithAttachPoint : MonoBehaviour
{
    public Transform leftHandAttach;
    public Transform rightHandAttach;

    private XRGrabInteractable grabInteractable;
    private Transform attachTransform;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);   
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        var interactor = args.interactorObject;

        if (interactor.transform.name.Contains("Right")) 
            attachTransform = rightHandAttach; 
        else 
            attachTransform = leftHandAttach;
    }

    private void FixedUpdate()
    {
        grabInteractable.attachTransform = attachTransform;
    }
}
