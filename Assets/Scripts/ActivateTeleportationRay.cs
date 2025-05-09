using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    public InputActionProperty leftCancel;
    public InputActionProperty rightCancel;

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;
    
    // Update is called once per frame
    void Update()
    {
        // we learn that there is a menu or not for the left ray
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out UnityEngine.Vector3 leftPos, out UnityEngine.Vector3 leftNormal, out int leftNumber, out bool leftValid);
        leftTeleportation.SetActive(!isLeftRayHovering && leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);
        
        // we learn that there is a menu or not for the right ray
        bool isRightRayHovering = rightRay.TryGetHitInfo(out UnityEngine.Vector3 rightPos, out UnityEngine.Vector3 rightNormal, out int rightNumber, out bool rightValid);
        rightTeleportation.SetActive(!isRightRayHovering && rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);    
    }
}
