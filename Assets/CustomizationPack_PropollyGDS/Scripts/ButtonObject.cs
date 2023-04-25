using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace CustomizationPackage
{
    public class ButtonObject : MonoBehaviour
    {
        MenuManager menu;           // A reference to the MenuManager object that owns this button.
        string Action, ButtonName;  // The action and name of the button.
        int ListID, ID;             // The list ID and object ID of the button.
        public Image img;           // The image component of the button.
        public Text txt;            // The text component of the button.

        public void InitButton(MenuManager _m, string _act, string _nm, int _lid, int _id, Sprite _img, string _disNm, Color _bc, Color _tc)
        {
            // Initializes the button with the specified parameters.
            menu = _m;
            Action = _act;
            ButtonName = _nm;
            ListID = _lid;
            ID = _id;
            img.sprite = _img;
            img.color = _bc;
            txt.text = _disNm;
            txt.color = _tc;

            // Sets the position of the text component to center it within the button.
            RectTransform rTxt = txt.rectTransform;
            rTxt.localPosition = new Vector3(0, ((-1 * _m.ButtonHeight/2)-25), 0);
        }

        public void InitBackButton(MenuManager _m, string _nm, Color _bc, Color _tc)
        {
            // Initializes a back button with the specified parameters.
            menu = _m;
            Action = "Launch Menu";
            ButtonName = _nm;
            ListID = -1;
            ID = -1;
            img.color = _bc;
            txt.text = "BACK";
            txt.color = _tc;
            RectTransform rTxt = txt.rectTransform;
            rTxt.localPosition = new Vector3(0, ((-1 * _m.ButtonHeight / 2) - 25), 0);
        }

        public void InitSaveButton(MenuManager _m, string _nm, Color _bc, Color _tc)
        {
            // Initializes a save button with the specified parameters.
            menu = _m;
            Action = "Save";
            ButtonName = _nm;
            ListID = -1;
            ID = -1;
            img.color = _bc;
            txt.text = "SAVE";
            txt.color = _tc;
            RectTransform rTxt = txt.rectTransform;
            rTxt.localPosition = new Vector3(0, ((-1 * _m.ButtonHeight / 2) - 25), 0);
        }

        public void Click()  
        {
            // Handles button clicks by calling the ButtonClick method in the MenuManager object.
            menu.ButtonClick(Action, ButtonName, ListID, ID); 
        }
    }
}