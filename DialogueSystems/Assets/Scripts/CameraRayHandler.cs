using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


/// <summary>
/// Searches for game objects at the cursor position, and can return events if they're of the specified layer mask. Must use an overlay camera.
/// Currently used for tracking the cards.
/// </summary>
public class CameraRayHandler : MonoBehaviour
{

    [SerializeField] private Camera overlayCamera;
    [SerializeField] private LayerMask layerMask;
    private InputAction mousePositionAction;
    
    public static GameObject lastHoveredObject;

    public delegate void ObjectExit();
    public static event ObjectExit objectExit;
    public delegate void ObjectEnter();
    public static event ObjectEnter objectEnter;

    private void Awake()
    {
        var inputActionMap = new InputActionMap();
        mousePositionAction = inputActionMap.AddAction("MousePosition", binding: "<Mouse>/position");
        mousePositionAction.performed += OnMousePositionChanged;
    }

    private void OnEnable()
    {
        mousePositionAction.Enable();
    }

    private void OnDisable()
    {
        mousePositionAction.Disable();
    }


    // Raycaster that checks if the object under the mouse is in the correct camera layer.
    private void OnMousePositionChanged(InputAction.CallbackContext context)
    {
        if (overlayCamera == null)
        {
            Debug.LogError("Overlay camera unassigned.");
            return;
        }

        Vector2 mouseScreenPosition = context.ReadValue<Vector2>();
        Ray ray = overlayCamera.ScreenPointToRay(mouseScreenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (lastHoveredObject != hitObject)
            {
                if (lastHoveredObject != null)
                {
                    RayHitExit(lastHoveredObject);
                    // Debug.Log("Mouse exited: " + lastHoveredObject.name);
                }
               // Debug.Log("Mouse entered: " + hitObject.name);
                lastHoveredObject = hitObject;
                RayHitEnter(lastHoveredObject);
            }
        }
        else if (lastHoveredObject != null)
        {
            // Debug.Log("Mouse exited: " + lastHoveredObject.name);
            lastHoveredObject = null;
            RayHitExit(lastHoveredObject);
        }
    }

    public void RayHitExit(GameObject exited)
    {
        if (objectExit != null)
            objectExit();
    }
    public void RayHitEnter(GameObject entered)
    {
        if (objectEnter != null)
            objectEnter();
    }


}
