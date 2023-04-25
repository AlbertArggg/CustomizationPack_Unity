using UnityEngine;

namespace CustomizationPackage
{
    /* Documentation
     * This class provides a script for rotating a camera around a pivot point with mouse input. It also allows for defining a specific area on the screen
     * where mouse input is registered. It is intended for use in a 3D environment where the camera needs to be repositioned by the user.
     * mouseSensitivity: This variable determines how fast the camera moves based on mouse input.
     * Pivot: This variable represents the pivot point around which the camera rotates.
     * xRotation: This variable holds the current rotation value around the x-axis.
     * yRotation: This variable holds the current rotation value around the y-axis.
     * currVal: This variable holds the current rotation value of the camera.
     * destVal: This variable holds the target rotation value of the camera.
     * currentMouseActiveArea: This array holds the values that define the area on the screen where mouse input is registered.
     * lastMousePosition: This variable holds the position of the mouse on the previous frame.
     * setPlayableArea(int[] vals): This method is used to define the specific area on the screen where mouse input is registered. 
     * It takes an array of integers that represent the left, right, top, and bottom boundaries of the area.
     * Start(): This method is called once when the script starts. It initializes some variables.
     * Update(): This method is called once per frame. It checks if the mouse is within the playable area and if so, calculates the new camera rotation
     * based on mouse input. If the mouse is not within the playable area, the camera rotation smoothly transitions to its destination value.
     */

    public class CameraRotate : MonoBehaviour
    {
        public Transform Camera;

        [HideInInspector] public float xRotation = 0f;
        [HideInInspector] public float yRotation = 0f;
        [HideInInspector] public Quaternion currVal;
        [HideInInspector] public Quaternion destVal;

        private float mouseSensitivity = 300f;
        private float wheelSensitivity = 2f;
        private float maxDistance = 20f;
        private float minDistance = 5;

        // currentMouseActiveArea determines the boundaries of the area where the mouse can be used to rotate the camera
        private int[] currentMouseActiveArea = new int[4];

        // lastMousePosition keeps track of the last position of the mouse
        private Vector3 lastMousePosition;

        public void InitCameraPivot(float RSens, float SSens, float MaxDist, float MinDist)
        {
            mouseSensitivity = RSens * 10;
            wheelSensitivity = SSens;
            maxDistance = MaxDist * -1;
            minDistance = MinDist * -1;
        }
        private void Start()
        {
            // Start method initializes some variables at the beginning of the script
            currVal = transform.rotation;
            destVal = transform.rotation;
            lastMousePosition = Input.mousePosition;
        }

        public void setPlayableArea(int[] vals)
        {
            // setPlayableArea sets the boundaries of the area where the mouse can be used to rotate the camera
            currentMouseActiveArea = vals;

            // If the right boundary of the area is not set, it defaults to 100% of the screen width
            if (currentMouseActiveArea[1] == 0) { currentMouseActiveArea[1] = 100; }

            // If the bottom boundary of the area is not set, it defaults to 100% of the screen height
            if (currentMouseActiveArea[3] == 0) { currentMouseActiveArea[3] = 100; }
        }

        void Update()
        {
            currVal = transform.rotation;
            Vector3 currentMousePosition = Input.mousePosition;

            // If the left mouse button is pressed and the mouse position has changed and is within the bounds of the active area
            if (Input.GetMouseButton(0) && lastMousePosition != currentMousePosition && currentMousePosition.x > Screen.width / 100 * currentMouseActiveArea[0]
                && currentMousePosition.x < Screen.width / 100 * currentMouseActiveArea[1]
                && currentMousePosition.y > Screen.height / 100 * currentMouseActiveArea[2]
                && currentMousePosition.y < Screen.width / 100 * currentMouseActiveArea[3])
            {
                // Calculate the amount of rotation based on the mouse movement
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                // Update the rotation values based on the mouse movement
                xRotation = transform.localRotation.eulerAngles.x - mouseY;
                yRotation = transform.localRotation.eulerAngles.y + mouseX;

                // Rotate the camera based on the new rotation values and set destVal
                transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
                destVal = transform.localRotation;
            }
            else 
            {
                // If the mouse is not being used to rotate the camera Lerp between the current rotation value and the destination rotation value to smoothly rotate the camera
                transform.localRotation = Quaternion.Lerp(currVal, destVal, Time.deltaTime * 4f);
            }

            // Zoom in and out using the mouse wheel
            float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            if (mouseWheel != 0f)
            {
                Vector3 position = Camera.transform.localPosition;
                position.z += mouseWheel * wheelSensitivity;
                if (position.z < maxDistance) { position.z = maxDistance; }
                if (position.z > minDistance) { position.z = minDistance; }
                Camera.transform.localPosition = position;
            }

            lastMousePosition = currentMousePosition;
        }
    }
}