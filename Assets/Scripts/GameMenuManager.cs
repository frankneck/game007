using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameMenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject options;
    public GameObject panel;
    public Transform head;
    public InputActionProperty showButton;

    // Update is called once per frame
    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            // if menu is active we set it as inactive
            menu.SetActive(!menu.activeSelf);
            panel.SetActive(!panel.activeSelf);

            if (options.activeSelf)
            {
                options.SetActive(!options.activeSelf);
            }

            menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z) * 3f;
        }

        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;

        options.transform.position = menu.transform.position;
        options.transform.rotation = menu.transform.rotation;

        panel.transform.position = menu.transform.position;
        panel.transform.rotation = menu.transform.rotation;
    }
}
