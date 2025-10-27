using System.Collections.Generic;
using UnityEngine;

namespace FlightSim
{
    public class MainController : MonoBehaviour
    {
        #region System
        private Rigidbody rb;
        #endregion

        #region Variables
        public InputController Input;
        public Properties Prop;
        public EngineController Engine;
        public List<WheelController> Wheels = new List<WheelController>();
        public List<ControlSurfaces> ControlSurfaces = new List<ControlSurfaces>();
        //public ControlSurfaces ControlSurface;
        public float Weight = 700f; //機体重量
        private float currentAlt;
        public float CurrentAlt
        {
            get{ return currentAlt; }
        }
        #endregion

        #region Custom Methods
        void Start()
        {
            rb = GetComponent<Rigidbody>(); //Rigidbodyを取得
            if(rb)
            {
                rb.mass = Weight;
                Prop = GetComponent<Properties>();
                if(Prop)
                {
                    Prop.InitProperties(rb, Input);
                }
            }
            if(Wheels != null)
            {
                foreach (WheelController wheel in Wheels)
                {
                    wheel.InitWheel();
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (rb && Input)
            {
                /*float forwardPower = 1000f;
                float liftPower = 5500f;
                Vector3 Power = new Vector3(0f, liftPower, forwardPower);
                rb.AddForce(Power);*/
                HandleEngine();
                HandleProperties();
                HandleWheel();
                HandleControlSurfaces();
                HandleAltitude();
            }
        }
        #endregion

        #region Custom Methods
        void HandleEngine()
        {
            if(Engine != null)
            {
                rb.AddForce(Engine.CalculateForce(Input.RateThrottle));
            }
        }

        void HandleProperties()
        {
            if(Prop != null)
            {
                Prop.UpdateProperties();
            }
        }

        void HandleWheel()
        {
            if (Wheels != null)
            {
                foreach (WheelController wheel in Wheels)
                {
                    wheel.HandleWheel(Input);
                }
            }
        }

        void HandleControlSurfaces()
        {
            foreach (ControlSurfaces controlSurface in ControlSurfaces)
            {
                controlSurface.HandleControlSurface(Input);
            }
            //ControlSurface.HandleControlSurface(Input);
        }
        
        void HandleAltitude()
        {
            currentAlt = transform.position.y;
            //Debug.Log(currentAlt);
        }
        #endregion
    }
}

