using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlightSim
{
    public class Speedometer : MonoBehaviour
    {
        #region Variables
        public Properties properties;
        public Slider SpeedSlider;
        public TextMeshProUGUI SpeedText;
        public float maxSpeedInKnots = 160f; //計器の最大値
        #endregion

        private const float mpsToKnots = 1.9438f; //ノットに変換

        #region Interface Methods
        public void HandleUI()
        {
            if (properties)
            {
                float currentKnots = properties.ForwardSpeed * mpsToKnots;
                SpeedText.text = Math.Floor(currentKnots).ToString();
                SpeedSlider.value = currentKnots / maxSpeedInKnots;
            }
        }
        #endregion
    }
}