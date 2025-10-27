using UnityEngine;
using UnityEngine.UI;

namespace FlightSim
{
    public class FlapKnob : MonoBehaviour
    {
        #region Variables
        public InputController Input;
        public Slider FlapSlider;
        #endregion

        #region Custom Methods
        public void HandleUI()
        {
            if (Input)
            {
                FlapSlider.value = -Input.Flap;
            }
        }
        #endregion
    }
}