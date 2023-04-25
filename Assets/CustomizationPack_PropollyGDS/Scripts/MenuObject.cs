using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomizationPackage
{
    /* Documentation
     * Unity script for managing the behavior and properties of a menu object.
     * The script is part of a customization package.
     */

    public class MenuObject : MonoBehaviour
    {
        public int ListID;              // Menu List ID
        public string MenuName;         // List / Menu name
        public Image MenuPanel;         // Image used in background of menu
        public Text MenuText;           // Menu Title

        [HideInInspector] public float[] CameraAngle = new float[3];        // array of values used to rotate camera when menu is selected
        [HideInInspector] public int[] playableArea = { 0, 100, 0, 100 };   // area in which the user can click and drag to rotate customizable object when this menu is launched
        public List<Transform> Attachements = new List<Transform>();        // attachements belonging to this List / menu

        public void InitMenu(MenuManager mm, string Name, string View, string MenuArea)
        {
            // initializes the Menu object with required variable values
            MenuName = Name;

            switch (View)
            {
                case "TopDown_View":
                    CameraAngle = new float[] { -310, -90, 0 };
                    break;

                case "Isometric_View":
                    CameraAngle = new float[] { 20, -30, 0 };
                    break;

                case "Left_View":
                    CameraAngle = new float[] { 10, -72, 0 };
                    break;

                case "Right_View":
                    CameraAngle = new float[] { 10, 72, 0 };
                    break;

                case "Front_View":
                    CameraAngle = new float[] { 8, -8, 0 };
                    break;

                case "Rear_View":
                    CameraAngle = new float[] { 10, 170, 0 };
                    break;

                case "DontMessWithMyView":
                    CameraAngle = new float[] { 0, 0, 0 };
                    break;

                default:
                    View = "DontMessWithMyView";
                    CameraAngle = new float[] { 0, 0, 0 };
                    break;
            }

            switch (MenuArea)
            {
                case "Bottom":
                    playableArea = new int[] { 0, 100, 23, 100 };
                    break;

                case "Left":
                    playableArea = new int[] { 27, 100, 0, 100 };
                    break;

                default:
                    playableArea = new int[] { 0, 100, 0, 100 };
                    break;
            }
        }
        public void InitMenu(MenuManager mm, int LID, string Name, string DisplayName, string View, string MenuArea, Color pannelColor, Color textColor)
        {
            // initializes the Menu object with required variable values
            ListID = LID;
            MenuName = Name;
            MenuPanel.color = pannelColor;
            MenuText.text = DisplayName;
            MenuText.color = textColor;

            switch (View)
            {
                case "TopDown_View":
                    CameraAngle = new float[] { -310, -60, 0 };
                    break;

                case "Isometric_View":
                    CameraAngle = new float[] { 20, -30, 0 };
                    break;

                case "Left_View":
                    CameraAngle = new float[] { 10, -72, 0 };
                    break;

                case "Right_View":
                    CameraAngle = new float[] { 10, 72, 0 };
                    break;

                case "Front_View":
                    CameraAngle = new float[] { 8, -8, 0 };
                    break;

                case "Rear_View":
                    CameraAngle = new float[] { 10, 170, 0 };
                    break;

                case "DontMessWithMyView":
                    CameraAngle = new float[] { 0, 0, 0 };
                    break;

                default:
                    View = "DontMessWithMyView";
                    CameraAngle = new float[] { 0, 0, 0 };
                    break;
            }

            switch (MenuArea)
            {
                case "Bottom":
                    playableArea = new int[] { 0, 100, 23, 100 };
                    break;

                case "Left":
                    playableArea = new int[] { 27, 100, 0, 100 };
                    break;

                default:
                    playableArea = new int[] { 0, 100, 0, 100 };
                    break;
            }
        }
    }
}