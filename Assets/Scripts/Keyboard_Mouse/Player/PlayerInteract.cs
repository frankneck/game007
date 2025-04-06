using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    public LayerMask mask;
    private bool isInteract;
    private Camera cam;
    private InputManager inputManager;
    private PlayerUI playerUI;

    private void Awake()
    {
        cam = GetComponent<PlayerLook>().cam;
        inputManager = GetComponent<InputManager>();
        playerUI = GetComponent<PlayerUI>();
    }
    private void Update()
    {
        isInteract = false;
        playerUI.UpdateText(string.Empty);
        playerUI.UpdateCursor(isInteract);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                isInteract = !isInteract;
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.message);
                playerUI.UpdateCursor(isInteract);

                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }

}

