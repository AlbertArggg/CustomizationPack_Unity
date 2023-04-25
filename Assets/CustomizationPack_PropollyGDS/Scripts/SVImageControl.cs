using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CustomizationPackage
{
    /* Documentation:
     * Controls the color picker's saturation and value. Implements the IDragHandler and IPointerClickHandler interfaces 
     * to detect user input on the SV image. The UpdateColor method updates the color based on the current position of 
     * the picker on the SV image. The Awake method sets the initial state of the SV image and picker image.
     */

    public class SVImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image pickerImage;
        private RawImage SVimage;
        private ColorPickerControl CC;
        private RectTransform rectTransform, pickerTransform;

        // Implementing IDragHandler interface. This method will be called when the user is dragging on the SV image.
        public void OnDrag(PointerEventData eventData) { UpdateColor(eventData); }

        // Implementing IPointerClickHandler interface. This method will be called when the user clicks on the SV image.
        public void OnPointerClick(PointerEventData eventData) { UpdateColor(eventData); }

        void Awake()
        {
            SVimage = GetComponent<RawImage>();                             // Getting the RawImage component of the SV image.
            CC = FindObjectOfType<ColorPickerControl>();                    // Getting the ColorPickerControl instance from the scene.
            rectTransform = GetComponent<RectTransform>();                  // Getting the RectTransform component of the SV image.
            pickerTransform = pickerImage.GetComponent<RectTransform>();    // Getting the RectTransform component of the picker image.
            pickerTransform.localPosition = new Vector3(0, 0, 0);           // Setting the picker image's local position to (0, 0, 0).
        }

        void UpdateColor(PointerEventData eventData)
        {
            // Getting the position of the event in the local space of the SV image.
            Vector3 pos = rectTransform.InverseTransformPoint(eventData.position);

            // Getting half the width of the SV image.
            float deltaX = rectTransform.sizeDelta.x * 0.5f;

            // Getting half the height of the SV image.
            float deltaY = rectTransform.sizeDelta.y * 0.5f; 

            // Clamping the x and y positions within the bounds of the SV image.
            if (pos.x < -deltaX) { pos.x = -deltaX; }
            if (pos.x > deltaX) { pos.x = deltaX; }
            if (pos.y < -deltaY) { pos.y = -deltaY; }
            if (pos.y > deltaY) { pos.y = deltaY; }

            // Getting the x / y position of the picker relative to the center of the SV image.
            float x = pos.x + deltaX;
            float y = pos.y + deltaY;

            // Normalizing the x / y position of the picker.
            float xNorm = x / rectTransform.sizeDelta.x;
            float yNorm = y / rectTransform.sizeDelta.y;

            // Setting the position of the picker to the clamped position.
            pickerTransform.localPosition = pos;

            // Setting the color of the picker image based on the normalized y position of the picker.
            pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNorm);

            // Setting the SV values in the color picker controller.
            CC.SetSV(xNorm, yNorm);
        }
    }
}