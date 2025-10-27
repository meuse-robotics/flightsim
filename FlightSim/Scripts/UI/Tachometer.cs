using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace FlightSim
{
    public class Tachometer : MonoBehaviour
    {
        #region Variables
        public EngineController engine;
        private float maxScale = 3000f; //計器上の最大値
        public Slider TachoSlider;
        public TextMeshProUGUI TachoVal;
        #endregion

        #region Interface Methods
        public void HandleUI()
        {
            if (engine)
            {
                float rpm = engine.CurrentRPM;
                TachoVal.text = Math.Floor(rpm).ToString();
                TachoSlider.value = rpm / maxScale;
            }
        }
        #endregion
    }
}