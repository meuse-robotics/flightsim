using UnityEngine;
using UnityEngine.UI;

namespace FlightSim
{
    public class ThrottleLever : MonoBehaviour
    {
        #region Variables
        public InputController Input;
        public Slider ThrottleSlider;
        #endregion
        
        #region Custom Methods
        public void HandleUI()
        {
            if (Input)
            {
                ThrottleSlider.value = Input.RateThrottle;
            }
        }
        #endregion
    }
}