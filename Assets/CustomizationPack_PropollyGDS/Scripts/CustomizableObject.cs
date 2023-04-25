using System.Collections.Generic;
using UnityEngine;

namespace CustomizationPackage
{
    public class CustomizableObject : MonoBehaviour
    {
        public enum ObjectType { Vehicle, Weapon, Human }   // object types enum

        // A public field that stores the object type of this CustomizableObject.
        [SerializeField] public ObjectType objectType;

        // A public field that stores the name of the main menu object.
        [SerializeField] public string MenuObjectName = "Main Menu";

        // A public field that stores the display name of the main menu.
        [SerializeField] public string MainMenuDisplayName = "MAIN MENU";

        // Camera zoom levels
        public float minDistance;
        public float maxDistance;

        // A serializable class that represents a menu object.
        [System.Serializable] public class MenuObj
        {
            // An enumeration of view modes.
            public enum ViewMode
            {
                TopDown_View,
                Isometric_View,
                Left_View,
                Right_View,
                Front_View,
                Rear_View,
                DontMessWithMyView
            }

            // An integer that stores the sequence ID of the menu object.
            public int SeqID;

            // A string that stores the name of the menu object.
            public string MenuName;

            // A string that stores the display name of the menu object.
            public string DisplayName;

            // A sprite that represents the menu button image.
            public Sprite ButtonImage;

            // A view mode that represents the camera view when the menu is launched.
            public ViewMode View;
        }
        
        // A serializable class that represents the object's variales. 
        [System.Serializable] public class ObjectVariables
        {
            public string VariableName;
            [Range(0f, 100f)] public float VariableValue;
            [HideInInspector] public GameObject UIObject;
        }

        // An array of materials that can be customized.
        public Material[] CustomizableMats;

        // A list of menu objects that can be launched.
        public List<MenuObj> Menus = new List<MenuObj>();

        // List of Variables relevent to this customizable object
        public List<ObjectVariables> Variables = new List<ObjectVariables>();
    }
}