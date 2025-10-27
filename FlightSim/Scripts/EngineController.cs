using UnityEngine;

namespace FlightSim
{
    public class EngineController : MonoBehaviour
    {
        #region Variables
        public PropellerController Propeller;
        public float MaxPower = 3000f;
        public float MaxRPM = 2400f;
        private float currentRPM;
        public float CurrentRPM
        {
            get { return currentRPM; }
        }
        public AudioSource EngineSound;
        #endregion

        void Start()
        {
            EngineSound.volume = 0.5f;
            EngineSound.pitch = 1f;
        }
        void Update(){}
        
        #region Custom Methods
        public Vector3 CalculateForce(float throttle)
        {
            EngineSound.volume = 0.5f + throttle;
            EngineSound.pitch = 1f + throttle;

            currentRPM = MaxRPM * throttle;
            if(Propeller)
            {
                Propeller.HandlePropeller(currentRPM, throttle);
            }
            //float power = MaxPower * Mathf.Clamp01(throttle);
            float power = MaxPower * throttle;
            Vector3 vectorForce = transform.forward * power;

            return vectorForce;
        }
        #endregion
    }
}
