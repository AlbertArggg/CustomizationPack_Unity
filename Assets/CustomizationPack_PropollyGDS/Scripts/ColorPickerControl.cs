using System;
using UnityEngine;
using UnityEngine.UI;

namespace CustomizationPackage
{
    /* Documentation
     * ColorPickerControl is a script that provides methods for creating and updating a color picker UI element, which allows users to select and customize colors.
     * initColorPickerControl(): Initializes the color picker with the specified material.
     * SetSV(float S, float V): Sets the saturation and value of the current color.
     * UpdateSVImage(): Updates the saturation-value image in the color picker.
     * UpdateAlphaValue(): Updates the alpha value of the material being customized.
     * UpdateMetalicValue(): Updates the metallic value of the material being customized.
     * UpdateGlossinessValue(): Updates the glossiness value of the material being customized.
     * CreateHueImage(): Creates the hue image of the color picker.
     * CreateSVImage(): Creates the saturation-value image of the color picker.
     * CreateOutputImage(): Creates the output image of the color picker.
     * UpdateOutputImage(): Updates the output image of the color picker.
     */

    public class ColorPickerControl : MonoBehaviour
    {
        // Public fields that store the current HSV values of the color picker. 
        public float currentHue, currentSat, currentVal;

        // Private fields that represent the hue, saturation/value (SV), and output images of the color picker.
        [SerializeField] private RawImage hueImage, satValImage, outputImage;

        // Private fields that represent the hue, alpha, metallic, and glossiness sliders of the color picker.
        [SerializeField] private Slider hueSlider;
        [SerializeField] private Slider metalicSlider;
        [SerializeField] private Slider GlossinessSlider;
        [SerializeField] private Slider alphaSlider;
        
        // Textures
        private Texture2D hueTexture, svTexture, outputTexture;

        // A reference to the menu manager that owns this color picker.
        MenuManager menuManager;

        // The index of the material being customized.
        [HideInInspector] public int currMatIndex = 0;

        // The material being customized.
        public Material Mat;

        // Initializes the color picker with the specified material.
        public void initColorPickerControl(Material _Mat, MenuManager mm, int mIndx)
        {
            menuManager = mm;       // reference to the menu manager.
            currMatIndex = mIndx;   // Store the current material index.
            Mat = _Mat;             // Store a reference to the current material.

            // Convert the current color to HSV values.
            Color.RGBToHSV(_Mat.color, out currentHue, out currentSat, out currentVal);

            // Create the hue, SV, and output images.
            CreateHueImage();
            CreateSVImage();
            CreateOutputImage();

            // Update the output image and slider values.
            UpdateOutputImage();
            hueSlider.value = currentHue;
            alphaSlider.value = _Mat.color.a;
            metalicSlider.value = _Mat.GetFloat("_Metallic");
            GlossinessSlider.value = _Mat.GetFloat("_Glossiness");
        }

        private void CreateHueImage()
        {
            // Create a new Texture2D object with a height of 16 pixels and a width of 1 pixel.
            hueTexture = new Texture2D(1, 16);

            // Set the wrap mode of the texture to Clamp, which means that the edge pixels are extended to fill the remaining space.
            hueTexture.wrapMode = TextureWrapMode.Clamp;

            // Give the texture a name for identification purposes.
            hueTexture.name = "HueTexture";

            // Iterate through each pixel in the texture.
            for (int i = 0; i < hueTexture.height; i++)
            {
                // Set the color of the current pixel based on its position in the texture.
                // The hue value is determined by dividing the current position by the total height of the texture.
                // The saturation and value are set to 1 and 0.95, respectively.
                hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexture.height, 1, 0.95f));
            }

            // Apply the changes to the texture.
            hueTexture.Apply();

            // Assign the texture to the hue image in the color picker.
            hueImage.texture = hueTexture;
        }

        private void CreateSVImage()
        {
            // Create a new Texture2D object with a height and width of 16 pixels.
            svTexture = new Texture2D(16, 16);

            // Set the wrap mode of the texture to Clamp, which means that the edge pixels are extended to fill the remaining space.
            svTexture.wrapMode = TextureWrapMode.Clamp;

            // Give the texture a name for identification purposes.
            svTexture.name = "SatValTexture";

            // Iterate through each pixel in the texture.
            for (int y = 0; y < svTexture.height; y++)
            {
                for (int x = 0; x < svTexture.width; x++)
                {
                    // Set the color of the current pixel based on its position in the texture.
                    // The hue value is set to the current hue value.
                    // The saturation and value are determined by dividing the current position by the total width or height of the texture.
                    svTexture.SetPixel(x, y, Color.HSVToRGB(currentHue, (float)x / svTexture.width, (float)y / svTexture.height));
                }
            }

            // Apply the changes to the texture.
            svTexture.Apply();

            // Assign the texture to the saturation-value image in the color picker.
            satValImage.texture = svTexture;
        }

        private void CreateOutputImage()
        {
            // Create a new Texture2D object with a size of 1x1 pixels.
            outputTexture = new Texture2D(1, 1);

            // Set the wrap mode of the texture to Clamp, which means that the edge pixel is extended to fill the remaining space.
            outputTexture.wrapMode = TextureWrapMode.Clamp;

            // Give the texture a name for identification purposes.
            outputTexture.name = "OutputTexture";

            // Convert the current hue, saturation, and value to a Color object.
            Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

            // Set the color of the single pixel in the texture to the current color.
            outputTexture.SetPixel(0, 0, currentColor);

            // Apply the changes to the texture.
            outputTexture.Apply();

            // Assign the texture to the output image in the color picker.
            outputImage.texture = outputTexture;
        }

        private void UpdateOutputImage()
        {
            // Convert the current hue, saturation, and value to a Color object.
            Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

            // Set the color of the single pixel in the texture to the current color.
            outputTexture.SetPixel(0, 0, currentColor);

            // Apply the changes to the texture.
            outputTexture.Apply();

            // If the menu manager is not assigned, exit the method.
            if (menuManager == null) { return; }

            // Set the color of the current material to the current color.
            menuManager.CustomizableObjectMaterials[currMatIndex].color = currentColor;
        }

        public void SetSV(float S, float V)
        {
            // Set the current saturation and value to the given values.
            currentSat = S; currentVal = V;

            // Update the output image in the color picker.
            UpdateOutputImage();
        }

        public void UpdateSVImage()
        {
            // If the SV texture is not assigned, exit the method.
            if (svTexture == null) { return; }

            // Set the current hue to the value of the hue slider.
            currentHue = hueSlider.value;

            // Iterate through each pixel in the SV texture.
            for (int y = 0; y < svTexture.height; y++)
            {
                for (int x = 0; x < svTexture.width; x++)
                {
                    // Set the color of the current pixel based on its position in the texture.
                    // The hue value is set to the current hue value.
                    // The saturation and value are determined by dividing the current position by the total width or height of the texture.
                    svTexture.SetPixel(x, y, Color.HSVToRGB(currentHue, (float)x / svTexture.width, (float)y / svTexture.height));
                }
            }

            // Apply the changes to the SV texture.
            svTexture.Apply();

            // Update the output image in the color picker.
            UpdateOutputImage();
        }

        public void UpdateAlphaValue()
        {
            // If the menu manager is not assigned, exit the method.
            if (menuManager == null) { return; }

            // Get the current material and set its color's alpha value to the value of the alpha slider.
            Material mat = menuManager.CustomizableObjectMaterials[currMatIndex];
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alphaSlider.value);
        }
        
        public void UpdateMetalicValue()
        {
            // If the menu manager is not assigned, exit the method.
            if (menuManager == null) { return; }

            // Get the current material and set its metallic value to the value of the metallic slider.
            Material mat = menuManager.CustomizableObjectMaterials[currMatIndex];
            mat.SetFloat("_Metallic", metalicSlider.value);
        }

        public void UpdateGlossinessValue()
        {
            // If the menu manager is not assigned, exit the method.
            if (menuManager == null) { return; }

            // Get the current material and set its glossiness value to the value of the glossiness slider.
            Material mat = menuManager.CustomizableObjectMaterials[currMatIndex];
            mat.SetFloat("_Glossiness", GlossinessSlider.value);
        }
    }
}