using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/* 
 
 A script for a topdown camera for an RTS-style game, with both keyboard and mouse inputs.

 The values depend greatly on the scale of objects in the world. 
 
 */


//TODO: Convert to Unity's new input system.

namespace StrategyCamera { 
    public class CameraController : MonoBehaviour
    {
        #region Declaration
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Vector3 zoomAmount = new Vector3(0,-0.2f,0.2f);
        [Tooltip("Minimum and maximum values on the zoom.")]
        [SerializeField] private Vector2 zoomRange = new Vector2(29, 50);
        private float movementSpeed;


        [Header("Speed Modifiers")]
        [Tooltip("Speed the camera goes by default.")]
        [SerializeField] private float normalSpeed = 0.5f;
        [Tooltip("Speed the camera goes when modified, e.g. by holding shift.")]
        [SerializeField] private float fastSpeed = 1.8f;


        [Tooltip("Stickiness of the smoothing on movements.")]
        [SerializeField] private float movementTime = 5f;

        private Vector3 newPosition;
        private Quaternion newRotation;
        private Vector3 newZoom;

        // Mouse Inputs

        [Tooltip("Modifier on how fast the mouse drag affects camera movement.")]
        [SerializeField] private float mouseDragSpeed = 5f;
        [Tooltip("Modifier on the speed of mouse scrolling on zoom.")]
        [SerializeField] private float mouseScrollSpeed = 15f;

        // Panning
        private Vector3 mouseDragStartPosition;
        private Vector3 mouseDragCurrentPosition;

        // Rotation
        [Header("Rotation")]
        [Tooltip("Rotation goes with or against the axis pressed.")]
        [SerializeField] private bool invertedRotateAxis = true;
        [Tooltip("Rotate at 90 degree angles, or freely.")]
        [SerializeField] private bool snapRotation = true;
        [Tooltip("Speed of camera rotation.")]
        [SerializeField] private float rotationSpeed = 1;

        private Vector3 mouseRotateStartPosition;
        private Vector3 mouseRotateCurrentPosition;
        private bool rotateClickActive = false;


        #endregion Declaration


        // Start is called before the first frame update
        void Start()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            HandleAllCameraMovement();
        }

        void HandleAllCameraMovement()
        {
            HandleKeyboardMovement();
            HandleKeyboardRotation();
            HandleKeyboardZoom();
            HandleMousePan();
            HandleMouseZoom();
            HandleMouseRotation();
            

            // Location
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            // Rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            // Zoom


            Debug.Log(newZoom);

            // If the camera's zoom is smaller than the minimum range (zoomRange.x) or greater than the maximum range (zoomRange.y).
            if (newZoom.y < zoomRange.x) 
            { 
                newZoom = new Vector3(0, zoomRange.x, -zoomRange.x);
            } 
            else if (newZoom.y > zoomRange.y)
            {
                newZoom = new Vector3(0, zoomRange.y, -zoomRange.y);
            }


            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }


        private void HandleMouseRotation()
        {
            if (Input.GetMouseButtonDown(2))
            {
                mouseRotateStartPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                mouseRotateCurrentPosition = Input.mousePosition;

                Vector3 difference = mouseRotateStartPosition - mouseRotateCurrentPosition;
                mouseRotateStartPosition = mouseRotateCurrentPosition;
                if (invertedRotateAxis)
                {
                    newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
                }
                else
                {
                    newRotation *= Quaternion.Euler(Vector3.up * (difference.x / 5f));
                }
            }
        }

        private void HandleMouseZoom()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount * mouseScrollSpeed;
            }
        }

        private void HandleMousePan()
        {
            // On mouse click
            if (Input.GetMouseButtonDown(1))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    mouseDragStartPosition = ray.GetPoint(entry);
                }
            }
            // On mouse drag
            if (Input.GetMouseButton(1))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    mouseDragCurrentPosition = ray.GetPoint(entry);

                    newPosition = transform.position + (mouseDragStartPosition - mouseDragCurrentPosition) * mouseDragSpeed;
                }
            }
        }

        private void HandleKeyboardZoom()
        {
            if (Input.GetKey(KeyCode.R))
            {
                newZoom += zoomAmount;
            }
            if (Input.GetKey(KeyCode.F))
            {
                newZoom -= zoomAmount;
            }
        }

        private void HandleKeyboardMovement()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            if (Input.GetKey(KeyCode.W))
            {
                newPosition += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                newPosition += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                newPosition += (transform.right * -movementSpeed);
            } 
        }
        private void HandleKeyboardRotation()
        {
            if (snapRotation)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    SnapRotation(true);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SnapRotation(false);
                }
            } else
            {
                if (Input.GetKey(KeyCode.Q))
                {

                    newRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
                }
                if (Input.GetKey(KeyCode.E))
                {
                    newRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
                }
            }
        }


        private void SnapRotation(bool posOrNeg)
        {
            // Amount to rotate in one snap.
            float snapAngle = 90f;

            if (posOrNeg)
            {
                newRotation *= Quaternion.Euler(Vector3.up * snapAngle);
            } else
            {
                newRotation *= Quaternion.Euler(Vector3.up * -snapAngle);
            }
            
        }
    }



}
