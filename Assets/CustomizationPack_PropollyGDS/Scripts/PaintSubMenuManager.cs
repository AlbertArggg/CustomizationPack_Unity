using UnityEngine;
using UnityEngine.UI;

namespace CustomizationPackage
{
    /* Documentation
     * This script is part of a Unity customization package and manages the behavior of a paint submenu
     * The script includes public variables for the panel, bars, back button, and texts that will be modified.
     * The InitValues method sets the color, size, and text color of the panel, back button, bars, and texts based on the passed-in parameters.
     * The InitValues method takes a Color object for the panel color (PClr), button color (BtnClr), and text color (TxtClr), 
     * as well as integers for the button width (w) and height (h).
     * The method then sets the colors of the panel, back button, and bars, as well as the size of the back button, and the color of the texts
     */

    public class PaintSubMenuManager : MonoBehaviour
    {
        public Transform Panel;             // panel image transform
        public Transform[] Bars;            // array of transform of images used as background for slider UI
        public Transform BackButton;        // Button
        public Text[] texts;                // array of texts in this menu
        public Text title;

        public void InitValues(Color PClr, Color BtnClr, Color TxtClr, int w, int h)
        {
            // set color of panel image
            Panel.GetComponent<Image>().color = PClr;

            // set color of back button sprite
            BackButton.GetComponent<Image>().color = BtnClr;

            // resize button to fit menu manager width / height values of user choice
            BackButton.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);

            // set color of slider background imgs
            foreach (Transform t in Bars)
            {
                t.GetComponent<RawImage>().color = BtnClr;
            }

            // set color of text objects 
            foreach (Text t in texts)
            {
                t.color = TxtClr;
            }
        }
    }
}