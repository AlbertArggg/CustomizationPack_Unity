using System.Collections.Generic;
using UnityEngine;
using System;

namespace CustomizationPackage
{
    /* Documentation:
     * this script provides a framework for creating customizable menus. It can be extended and modified to suit a variety of use cases.
     
     * Start(): This is the method that is called when the program starts. It initializes some components, creates the main menu object, 
                and launches the main menu. It also calls some other methods to handle attachments and create menus for the customizable object.

     * InitializeComponents(): This method initializes the MenuParentObject, CustObject, CustomizableObjectMaterials, and CameraPivotObject 
                variables by getting the corresponding objects from the scene.

     * CreateMenusFromCustObjectList(CustomizableObject CO, Transform mainMenu): This method creates a menu for each list in the CustomizableObject 
                CO and adds it to the AllMenus list. It also sets up the buttons for each menu.

     * HandleAttachements(Attachement[] attachments): This method handles the Attachement objects in the scene. It adds each attachment to the 
                Attachments list and sets its active state based on its ObjID value.

     * PaintMenuButtons(MenuObject mo): This method sets up the buttons for the Paint menu. It creates a button for each customizable material 
                and adds a submenu for selecting colors.

     * CustomizationMenuButtons(MenuObject mo): This method sets up the buttons for the other customization menus. It creates a button for each attachment object.
     * 
     * CreateMenuObject(Transform parent, int LID, string Name, string DisplayName, string View, string MenuArea): This method creates a 
                new menu object using the specified parameters and parent object.

     * CreateButtonObject(Transform parent, string Name, string DisplayName, string Action, int List, int ID, Sprite Image, int Sequence, Color BtnColor): 
                This method creates a new button object using the specified parameters and parent object.

     * CreateBackButtonObject(Transform parent, string Menu): This method creates a new back button object for the specified menu.
     * 
     * ButtonClick(string _act, string _btnNm, int _Lid, int _id): This method is called when a button is clicked. It uses a switch statement to 
     *          determine which action to take based on the value of _act.
     *          
     * findAndLaunch(string menuName): This method finds and launches the specified menu. It also rotates the camera if the menu has a non-zero camera 
     *          angle and initializes a color picker control for menus starting with "PSM".
     *          
     * Customize(int ListID, int ID): This method customizes the specified attachment object by setting its active state based on its ListID and ObjID values.
     */

    public class VariableDetails
    {
        public string VarName;
        public float VarBaseVal;
        public float VarModVal;
        public GameObject UIObject;

        public VariableDetails(string nm, float vl, GameObject VUIH) 
        {
            VarName = nm;
            VarBaseVal = vl;
            VarModVal = 0;
            UIObject = VUIH;
        }
    }

    public class MenuManager : MonoBehaviour
    {
        [Header("Camera Behaviour")]
        public float RotationalSensitivity = 20;
        public float ScrollWheelSensitivity = 2;

        [Header("Required Input")]
        public GameObject MenuPrefab;           // Drag and drop menu prefab here
        public GameObject PaintSubMenuPrefab;   // Drag and drop Paint Sub Menu prefab here
        public GameObject ButtonPrefab;         // Drag and drop button prefab here
        public GameObject BackButtonPrefab;     // Darg and drop back button prefab here
        public GameObject VariableUIPrefab;     // Drag and drop Variable prefab here

        private Transform MenuParentObject;                                 // parent object where menus spawn
        private Transform CustObject;                                       // customizable object (SUV, M4 ... )
        private Transform CameraPivotObject;                                // Camera parent object
        private List<Transform> AllMenus = new List<Transform>();           // all menus created by menu manager
        private List<Transform> Attachments = new List<Transform>();        // all attachements found in customizable object
        private MenuObject currentMenu;                                     // currently active menu in runtime
        [HideInInspector] public Material[] CustomizableObjectMaterials;    // Material array holding all customizable materials

        [Header("Panels")]
        public Sprite PanelSprite;                                          // sprite used as menu panel
        public Color PanelColor = Color.black;                              // sprite color value
        [Range(0.0f, 1.0f)] public float PanelColorAlphaValue;              // panel transparency [0-1]

        [Header("Buttons")]
        public Sprite PaintMenuButtonSprite;                                // sprite used in paint menu button
        public int ButtonWidth = 160;                                       // button width 
        public int ButtonHeight = 160;                                      // button height
        public int ButtonOffset = 20;                                       // distance between each spawned button
        public Color ButtonColor = Color.white;                             // button color
        public Color TextColor = Color.white;                               // text color
        [Range(0.0f, 1.0f)] public float ButtonColorAlphaValue = 1;         // button transparency [0-1]

        [Header("Variable Data")]
        public Color PositiveColor = Color.green;
        public Color NegativeColor = Color.red;
        public Color NeutralColor = Color.grey;

        List<VariableDetails> VarValues = new List<VariableDetails>();
        string lastAdjustedColor = "PSM_0";

        private void Start()
        {
            // get Menu parent object, Customizable object, customizable materials and Camera pivot object
            InitializeComponents();

            // Get the 'CustomizableObject' component from the 'CustObject' game object.
            CustomizableObject CO = CustObject.GetComponent<CustomizableObject>();

            // Create Variable UI objects
            for (int i = 0; i < CO.Variables.Count; i++)
            {
                GameObject VarUI = GameObject.Instantiate(VariableUIPrefab);
                VarUI.transform.SetParent(MenuParentObject);
                VarUI.GetComponent<VariableUIHandler>().InitVarHandler(NegativeColor, PositiveColor, NeutralColor, TextColor, CO.Variables[i].VariableValue, CO.Variables[i].VariableName, i);
                VariableDetails VD = new VariableDetails(CO.Variables[i].VariableName, CO.Variables[i].VariableValue, VarUI);
                VarValues.Add(VD);
                CO.Variables[i].UIObject = VarUI;
            }

            // Create a new main menu object using the specified parameters and parent object.
            Transform mainMenu = CreateMenuObject(MenuParentObject, 0, CO.MenuObjectName, CO.MainMenuDisplayName, "DontMessWithMyView", "Bottom");

           // reset main menu local position to (0,0,0)
            RectTransform rt = mainMenu.GetComponent<RectTransform>();
            rt.localPosition = Vector3.zero;

            // get all attachement type objects from the scene
            Attachement[] attachments = FindObjectsOfType<Attachement>();
            HandleAttachements(attachments);

            // create a menu for each list in Customizable object and add menu to menu List
            CreateMenusFromCustObjectList(CO, mainMenu);
            AllMenus.Add(mainMenu.transform);

            // Launch main menu and disable the rest
            FindAndLaunch("Main Menu");
        }

        private void InitializeComponents()
        {
            // get Menu parent object, Customizable object, customizable materials and Camera pivot object
            MenuParentObject = FindObjectOfType<MenuParentObject>().transform;
            CustObject = FindObjectOfType<CustomizableObject>().transform;
            CustomizableObjectMaterials = CustObject.GetComponent<CustomizableObject>().CustomizableMats;
            CameraPivotObject = FindObjectOfType<CameraRotate>().transform;

            float minD, MaxD;
            minD = CustObject.GetComponent<CustomizableObject>().minDistance;
            MaxD = CustObject.GetComponent<CustomizableObject>().maxDistance;

            CameraPivotObject.GetComponent<CameraRotate>().InitCameraPivot(RotationalSensitivity, ScrollWheelSensitivity, MaxD, minD);
            CameraPivotObject.GetComponent<CameraRotate>().Camera.transform.localPosition = new Vector3(0,0, ((MaxD+minD)/2)*-1);
        }

        private void CreateMenusFromCustObjectList(CustomizableObject CO, Transform mainMenu)
        {
            for (int i = 0; i < CO.Menus.Count; i++)
            {
                // Create the menu object
                Transform Menu = CreateMenuObject(MenuParentObject, (i + 1), CO.Menus[i].MenuName, CO.Menus[i].DisplayName, CO.Menus[i].View.ToString(), "Bottom");
                // Set the menu object's local position
                Menu.GetComponent<RectTransform>().localPosition = Vector3.zero;
                // Get the menu object's script component
                MenuObject mo = Menu.GetComponent<MenuObject>();
                // Set the menu object's list ID
                mo.ListID = i;
                // Add a back button to the menu
                CreateBackButtonObject(Menu, "Main Menu");
                // Set the menu buttons based on the menu name
                if (CO.Menus[i].MenuName != "Paint") { CustomizationMenuButtons(mo); } else { PaintMenuButtons(mo); }
                // Add a button to the main menu to launch this menu
                CreateButtonObject(mainMenu.transform, CO.Menus[i].MenuName, CO.Menus[i].DisplayName, "Launch Menu", -1, i, CO.Menus[i].ButtonImage, i, ButtonColor);
                // Add the menu to the list of all menus
                AllMenus.Add(Menu.transform);
            }
        }

        private void HandleAttachements(Attachement[] attachments)
        {
            foreach (Attachement attachment in attachments)
            {
                // Get the attachment's transform
                Transform attachmentTransform = attachment.transform;
                // Add the attachment transform to the list of attachments
                Attachments.Add(attachmentTransform);
                // Set the attachment's active state based on its ObjID value
                attachment.active = (attachmentTransform.GetComponent<Attachement>().ObjID == 0);
                attachmentTransform.gameObject.SetActive(attachmentTransform.GetComponent<Attachement>().ObjID == 0);
            }
        }

        private void PaintMenuButtons(MenuObject mo)
        {
            for (int j = 0; j < CustomizableObjectMaterials.Length; j++)
            {
                // Create a button for the customizable material
                CreateButtonObject(mo.transform, "PSM_" + j.ToString(), CustomizableObjectMaterials[j].name.ToUpper(),
                    "Launch Menu", 0, -1, PaintMenuButtonSprite, j, CustomizableObjectMaterials[j].color);
            }

            // Instantiate the paint submenu prefab
            GameObject mp = Instantiate(PaintSubMenuPrefab);
            // Set the paint submenu's parent
            mp.transform.SetParent(MenuParentObject);
            // Set the paint submenu's local position
            mp.GetComponent<RectTransform>().localPosition = Vector3.zero;
            // Set the paint submenu's name
            mp.name = "PSM";
            // Initialize the paint submenu's menu script component
            mp.GetComponent<MenuObject>().InitMenu(this, "PSM", "DontMessWithMyView", "Left");
            // Initialize the paint submenu's values
            mp.GetComponent<PaintSubMenuManager>().InitValues(PanelColor, TextColor, ButtonColor, ButtonWidth, ButtonHeight);
            // Initialize the back button on the paint submenu
            mp.GetComponent<PaintSubMenuManager>().BackButton.GetComponent<ButtonObject>().InitBackButton(this, "Paint", ButtonColor, TextColor);
            // Add the paint submenu to the list of all menus
            AllMenus.Add(mp.transform);
        }

        private void CustomizationMenuButtons(MenuObject mo)
        {
            // Create a list to keep track of found attachments
            List<string> foundAttachements = new List<string>();

            for (int j = 0; j < Attachments.Count; j++)
            {
                // Check if the attachment is for this menu
                if (Attachments[j].GetComponent<Attachement>().AttachmentList == mo.MenuName)
                {
                    // Add the attachment to the menu's list of attachments
                    mo.Attachements.Add(Attachments[j].gameObject.transform);
                    // Set the attachment's list ID
                    Attachement a = Attachments[j].GetComponent<Attachement>();
                    a.ListID = mo.ListID;

                    // Check if a button has already been created for this attachment
                    if (foundAttachements.Contains(a.AttachementName + a.ObjID) == false)
                    {
                        // Add the attachment to the list of found attachments
                        foundAttachements.Add(a.AttachementName + a.ObjID);
                        // Create a button for the attachment
                        CreateButtonObject(mo.transform, a.AttachementName, a.AttachementDisplayName, "Customize", a.ListID, a.ObjID, a.AttachementImage, a.ObjID, ButtonColor);
                    }
                }
            }
        }

        private Transform CreateMenuObject(Transform parent, int LID, string Name, string DisplayName, string View, string MenuArea)
        {
            // Instantiate a new menu prefab
            GameObject mp = Instantiate(MenuPrefab);
            // Set the parent of the menu
            mp.transform.SetParent(parent);
            // Set the name of the menu
            mp.name = Name;
            // Initialize the menu's script component with the given properties
            mp.GetComponent<MenuObject>().InitMenu(this, LID, Name, DisplayName, View, MenuArea, PanelColor, TextColor);
            // Return the transform of the new menu object
            return mp.transform;
        }

        private void CreateButtonObject(Transform parent, string Name, string DisplayName, string Action, int List, int ID, Sprite Image, int Sequence, Color BtnColor)
        {
            // Instantiate a new button prefab
            GameObject Button = Instantiate(ButtonPrefab);

            // Set the parent of the button
            Button.transform.SetParent(parent);

            // Set the name of the button
            Button.name = Name;

            // Initialize the button's script component with the given properties
            Button.GetComponent<ButtonObject>().InitButton(this, Action, Name, List, ID, Image, DisplayName, BtnColor, TextColor);

            // Get the button's rect transform and position it on the screen based on the given sequence
            RectTransform buttonRTrans = Button.GetComponent<RectTransform>();
            buttonRTrans.localPosition = new Vector3(((-960 + ButtonWidth + ButtonOffset) + ((ButtonWidth + ButtonOffset) * Sequence)), -405, 0);
            buttonRTrans.sizeDelta = new Vector2(ButtonWidth, ButtonHeight);
        }

        private void CreateBackButtonObject(Transform parent, string Menu)
        {
            // Instantiate a new back button prefab
            GameObject Button = Instantiate(BackButtonPrefab);

            // Set the parent of the button
            Button.transform.SetParent(parent);

            // Set the name of the button
            Button.name = "Back";

            // Initialize the button's script component with the given properties
            Button.GetComponent<ButtonObject>().InitBackButton(this, Menu, ButtonColor, TextColor);

            // Get the button's rect transform and position it on the screen
            RectTransform buttonRTrans = Button.GetComponent<RectTransform>();
            buttonRTrans.localPosition = new Vector3(840, -420, 0);
            buttonRTrans.sizeDelta = new Vector2(ButtonWidth, ButtonHeight);
        }

        public void ButtonClick(string _act, string _btnNm, int _Lid, int _id)
        {
            // Uses a switch statement to determine which action to take based on the value of _act
            switch (_act)
            {
                // If _act is "Launch Menu", call the findAndLaunch method with _btnNm parameter
                case "Launch Menu":
                    FindAndLaunch(_btnNm);
                    break;

                // If _act is "Customize", call the Customize method with _Lid and _id parameters
                case "Customize":
                    Customize(_Lid, _id);
                    break;
            }
        }

        public void FindAndLaunch(string menuName)
        {
            // Iterates through a collection of Transforms representing all the available menus
            foreach (Transform m in AllMenus)
            {
                // Sets the corresponding GameObject to active if the name of the menu matches the given menuName parameter
                m.gameObject.SetActive(menuName.StartsWith(m.GetComponent<MenuObject>().MenuName));

                // If the name of the menu matches the given menuName parameter
                if (menuName.StartsWith(m.GetComponent<MenuObject>().MenuName))
                {
                    // Rotates the camera if the menu has a non-zero camera angle
                    float[] rotVal = m.GetComponent<MenuObject>().CameraAngle;
                    if (rotVal[0] != 0 || rotVal[1] != 0)
                    {
                        CameraPivotObject.GetComponent<CameraRotate>().destVal = Quaternion.Euler(rotVal[0], rotVal[1], 0f);
                    }

                    // Initializes a color picker control for menus starting with "PSM"
                    if (menuName.StartsWith("PSM"))
                    {
                        lastAdjustedColor = menuName;
                        string[] p = menuName.Split('_');
                        int MatIndex = Int32.Parse(p[1]);
                        ColorPickerControl CC = FindObjectOfType<ColorPickerControl>();
                        CC.transform.GetComponent<PaintSubMenuManager>().title.text = CustomizableObjectMaterials[MatIndex].name.ToUpper();
                        CC.initColorPickerControl(CustomizableObjectMaterials[MatIndex], this, MatIndex);
                    }

                    // Updates the current menu and sets the playable area of the camera to match the new menu
                    currentMenu = m.transform.GetComponent<MenuObject>();
                    CameraPivotObject.GetComponent<CameraRotate>().setPlayableArea(currentMenu.GetComponent<MenuObject>().playableArea);

                    if (menuName == "Paint")
                    {
                        ReadjustColorButton(lastAdjustedColor);
                    }
                }
            }
        }

        public void ReadjustColorButton(string colorButton)
        {
            string[] p = colorButton.Split('_');
            int MatIndex = Int32.Parse(p[1]);

            Transform button = FindGameObjectByName(currentMenu.transform, lastAdjustedColor);
            button.GetComponent<ButtonObject>().img.color = CustomizableObjectMaterials[MatIndex].color;
        }

        public Transform FindGameObjectByName(Transform rootTransform, string gameObjectName)
        {
            Transform result = null;

            for (int i = 0; i < rootTransform.childCount; i++)
            {
                Transform childTransform = rootTransform.GetChild(i);

                if (childTransform.name == gameObjectName)
                {
                    return childTransform.gameObject.transform;
                }

                result = FindGameObjectByName(childTransform, gameObjectName);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public void Customize(int listID, int id)
        {
            HashSet<string> modifiedAttachmentKeys = new HashSet<string>();

            foreach (Transform attachmentTransform in Attachments)
            {
                Attachement attachment = attachmentTransform.GetComponent<Attachement>();

                if (attachment.ListID == listID)
                {
                    UpdateAttachment(attachment, id, modifiedAttachmentKeys);
                }
            }
            RecalculateUI();
        }

        private void UpdateAttachment(Attachement attachment, int id, HashSet<string> modifiedAttachmentKeys)
        {
            bool wasActive = attachment.active;
            attachment.active = (attachment.ObjID == id);
            attachment.transform.gameObject.SetActive(attachment.active);

            string attachmentKey = attachment.AttachmentList + "_" + attachment.ObjID;
            if (attachment.active != wasActive && !modifiedAttachmentKeys.Contains(attachmentKey))
            {
                modifiedAttachmentKeys.Add(attachmentKey);
                ModifyVariables(attachment, wasActive);
            }
        }

        private void ModifyVariables(Attachement attachment, bool wasActive)
        {
            foreach (Attachement.VariableMod vm in attachment.attachmentMod)
            {
                for (int i = 0; i < VarValues.Count; i++)
                {
                    if (vm.VariableName == VarValues[i].VarName)
                    {
                        if (attachment.active)
                        {
                            VarValues[i].VarModVal += vm.VariableModifier;
                        }
                        else
                        {
                            VarValues[i].VarModVal -= vm.VariableModifier;
                        }
                    }
                }
            }
        }

        public void RecalculateUI()
        {
            for (int i = 0; i < VarValues.Count; i++)
            {
                VarValues[i].UIObject.GetComponent<VariableUIHandler>().adjustUI(VarValues[i].VarBaseVal, VarValues[i].VarBaseVal+VarValues[i].VarModVal);
            }
        }
    }
}