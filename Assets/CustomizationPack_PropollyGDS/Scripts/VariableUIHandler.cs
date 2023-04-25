using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomizationPackage
{
    public class VariableUIHandler : MonoBehaviour
    {
        public string VarName;
        public Transform[] VarMeter;
        public Text Title;
        public RectTransform RT;

        Color negColor, PosColor, NeutColor;

        public void InitVarHandler(Color NegCol, Color PosCol, Color NeutCol, Color TextCol, float VarValue, string Name, int Seq)
        {
            negColor = NegCol;
            PosColor = PosCol;
            NeutColor = NeutCol;

            VarName = Name;
            Title.text = VarName.ToUpper();

            RT.localPosition = new Vector3(680f, (490f - (40f * Seq)), 0f);

            int DisplayVal = (int)VarValue / 5;

            for (int i = 0; i < VarMeter.Length; i++)
            {
                if (i < DisplayVal)
                    VarMeter[i].transform.GetComponent<Image>().color = PosCol;

                else
                    VarMeter[i].transform.GetComponent<Image>().color = NeutCol;
            }
        }

        public void adjustUI(float baseValue, float Value)
        {
            int DisplayValNew = (int)Value / 5;
            int DisplayValOld = (int)baseValue / 5;

            for (int i = 0; i < VarMeter.Length; i++)
            {
                if (i < DisplayValNew)
                    VarMeter[i].transform.GetComponent<Image>().color = PosColor;

                else if (i >= DisplayValNew && i < DisplayValOld)
                    VarMeter[i].transform.GetComponent<Image>().color = negColor;

                else
                    VarMeter[i].transform.GetComponent<Image>().color = NeutColor;
            }
        }
    }
}