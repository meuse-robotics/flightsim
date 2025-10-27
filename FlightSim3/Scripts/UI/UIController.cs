
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FlightSim
{
    public class UIController : MonoBehaviour
    {
        #region Variables
        public Tachometer Tachometer;
        public Speedometer Speedometer;
        public Altimeter Altimeter;
        public Attitude Attitude;
        public Compass Compass;
        public ThrottleLever ThrottleLever;
        public FlapKnob FlapKnob;
        public TrimWheel TrimWheel;
        #endregion

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //instruments = transform.GetComponentsInChildren<I_UI>().ToList<I_UI>();
        }

        // Update is called once per frame
        void Update()
        {
            /*foreach (I_UI instrument in instruments)
                {
                    instrument.HandleUI();
                }*/
            Tachometer.HandleUI();
            Speedometer.HandleUI();
            Altimeter.HandleUI();
            Attitude.HandleUI();
            Compass.HandleUI();
            ThrottleLever.HandleUI();
            FlapKnob.HandleUI();
            TrimWheel.HandleUI();
        }
    }
}