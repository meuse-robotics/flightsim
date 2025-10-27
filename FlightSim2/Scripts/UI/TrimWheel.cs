using UnityEngine;
using UnityEngine.UI;

namespace FlightSim
{
    public class TrimWheel : MonoBehaviour
    {
        #region Variables
        public InputController Input;
        public Slider TrimSlider;
        #endregion
        
        #region Custom Methods
        public void HandleUI()
        {
            if (Input)
            {
                TrimSlider.value = Input.Trim;
            }
        }
        #endregion        
    }
}