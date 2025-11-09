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
            get { return currentAlt; }
        }
        public float AltitudeServoKp = 0.02f;    // 高度偏差に対するゲイン
        public float AltitudeServoKd = 0.5f;     // 垂直速度に対するダンピング（負の速度に比例してスロットルを増減）
        public float AltitudeServoMaxThrottleRate = 0.8f; // 1秒あたりのスロットル最大変化量
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
                if (Input.isAutoLevel)
                {
                    // 独自の高度サーボ（I項は使わない）。低い線形ダンピング環境でも暴れにくいPD風制御＋スロットルレート制限
                    float altError = Input.targetHeight - rb.transform.position.y;
                    float verticalSpeed = rb.linearVelocity.y;

                    // PD制御：高度偏差と垂直速度（降下/上昇）を使う
                    float control = AltitudeServoKp * altError + AltitudeServoKd * (-verticalSpeed);

                    // ベーススロットルを残しつつ制御量を反映
                    float throttleTarget = Mathf.Clamp01(0.5f + control);

                    // スロットル変化量を制限（1秒あたりの最大変化量を使う）
                    float maxDelta = AltitudeServoMaxThrottleRate * Time.deltaTime;
                    float appliedThrottle = Mathf.MoveTowards(Input.RateThrottle, throttleTarget, maxDelta);

                    // エンジン出力に適用
                    rb.AddForce(Engine.CalculateForce(appliedThrottle));

                    // UIや他のロジックと整合させるためにRateThrottleを更新
                    Input.RateThrottle = appliedThrottle;
                }
                else
                {
                    rb.AddForce(Engine.CalculateForce(Input.RateThrottle));
                }
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

