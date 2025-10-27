using UnityEngine;

namespace FlightSim
{
    public class WheelController : MonoBehaviour
    {
        #region System
        private WheelCollider wheelCol;
        public float maxBrakeTorque = 100f; //最大ブレーキ力
        public float maxSteerAngle = 20f; //最大ステアリング角度
        private float brakeTorque; //ブレーキトルク計算用
        #endregion
        
        void Start()
        {
            wheelCol = GetComponent<WheelCollider>();    
        }

        void Update(){}

        #region Custom Methods
        public void InitWheel()
        {
            if (wheelCol)
            {
                //軸負荷を0に設定
                wheelCol.motorTorque = 0.0000000000000000001f;
            }
        }
        
        public void HandleWheel(InputController input)
        {
            //ブレーキ
            if (input.Brake > 0.1f)
            {
                //徐々に変化させる
                brakeTorque = Mathf.Lerp(
                    brakeTorque,
                    input.Brake * maxBrakeTorque,
                    Time.deltaTime);
                wheelCol.brakeTorque = brakeTorque;
            }
            else
            {
                wheelCol.brakeTorque = 0f;
                wheelCol.motorTorque = 0.000000000001f;
            }
            //ステアリング
            if (gameObject.name == "FWheelCol")
            {
                wheelCol.steerAngle = input.Yaw * maxSteerAngle;
            }
        }        
        #endregion
    }
}
