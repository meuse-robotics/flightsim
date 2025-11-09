using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace FlightSim
{
    public class Altimeter : MonoBehaviour
    {
        #region Variables
        public MainController Airplane;
        public Slider AltSlider;
        public Slider AltSlider_2;
        public TextMeshProUGUI AltVal;
        private const float meterToFeet = 3.28f; //ノットに変換
        public InputController Input;
        public TextMeshProUGUI AutoText;
        #endregion

        #region Interface Methods
        public void HandleUI()
        {
            if (Airplane)
            {
                float currentAlt = Airplane.CurrentAlt * meterToFeet;
                AltVal.text = Math.Round(currentAlt).ToString();
                AltSlider.value = currentAlt % 1000f / 1000f;
                AltSlider_2.value = currentAlt % 10000f / 10000f;
                if (Input.isAutoLevel) AutoText.alpha = 1f;
                else AutoText.alpha = 0f;
            }
        }
        #endregion
    }
}