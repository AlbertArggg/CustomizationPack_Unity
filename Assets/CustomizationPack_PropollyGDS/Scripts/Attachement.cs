using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CustomizationPackage.CustomizableObject;

namespace CustomizationPackage
{
    public class Attachement : MonoBehaviour
    {
        [HideInInspector] public int ListID;
        // A public string that stores the name of the attachment list.
        public string AttachmentList;

        // A public string that stores the name of the attachment.
        public string AttachementName;

        // A public string that stores the display name of the attachment.
        public string AttachementDisplayName;

        // A public sprite that represents the attachment image.
        public Sprite AttachementImage;

        // A public integer that stores the ID of the object the attachment is attached to.
        public int ObjID;

        // A public string that stores the action of customizing the attachment.
        public string Action = "Customize";

        // Attachement mod Class
        [System.Serializable] public class VariableMod
        {
            public string VariableName = "NULL";
            public float VariableModifier = 0;
        }

        [HideInInspector] public bool active = false;

        // name and value change
        public List<VariableMod> attachmentMod = new List<VariableMod>();

        // A public method that returns a string representation of the attachment.
        public string Print()
        {
            // Create an empty string to store the attachment information.
            string str = "";

            // Add the attachment name to the string, followed by a new line character.
            str += AttachementName + ": \n";

            // Add the list ID to the string, preceded by the label "List ID: ", followed by a new line character.
            str += "List ID: " + ListID + "\n";

            // Add the object ID to the string, preceded by the label "Object ID: ", followed by a new line character.
            str += "Object ID: " + ObjID + "\n";

            // Add the action to the string, preceded by the label "Action: ", followed by a new line character.
            str += "Action: " + Action + "\n";

            // Add the attachment list name to the string, preceded by the label "Attachment List: ", followed by a new line character.
            str += "Attachment List: " + AttachmentList + "\n";

            // Add the display name to the string, preceded by the label "Display Name: ", followed by a new line character.
            str += "Display Name: " + AttachementDisplayName + "\n";

            // Add the attachment image to the string, preceded by the label "Attachment Image: ", followed by a new line character.
            str += "Attachment Image: " + AttachementImage + "\n";

            // Return the string representation of the attachment.
            return str;
        }
    }
}