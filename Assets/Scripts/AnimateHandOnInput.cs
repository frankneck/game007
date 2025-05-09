using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gribAnimationAction;
    public Animator handAnimator;

    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
        float gripValue = gribAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
        
    }
}
