using Unity.VisualScripting;
using UnityEngine;

namespace FlightSim
{
    public class Properties : MonoBehaviour
    {
        #region Variables
        public float ForwardSpeed; //機体ローカルZ軸速度
        public float PitchCoef = 200f;
        public float RollCoef = 200f;
        public float YawCoef = 200f;
        private float speedCoef;
        private float maxSpeed = 65f;
        private Rigidbody rb;
        private InputController input;
        private float liftCoeff = 20f;//0.5f * 1.293f * 16f * 0.7f;
        private float flapLiftCoef = 10f;
        private float flapDampingCoef = 0.1f;
        private float initLinearDamping;
        private float angleOfAttack; //迎え角
        private float rollAngle;
        private float maxGroundDistance = 3f; //3m以下で地面効果
        private float wingspan = 11f; //翼の長さ
        private float groundEffectCoef = 16f; //係数
        private float groundEffect = 1f; //揚力を上げる効果
        public float staticCoef = 1f;
        public float rollServoCoef = 0.5f; // ロールサーボの強さ 追加
        public float rollServoDamping = 0.1f; // ロールサーボのダンピング　追加
        #endregion

        void Start(){}

        void Update(){}

        #region Custom Methosa
        public void InitProperties(Rigidbody rigidBody, InputController inputs)
        {
            rb = rigidBody;
            input = inputs;
            initLinearDamping = rb.linearDamping;
        }

        public void UpdateProperties()
        {
            if(rb)
            {
                CalculateSpeed();
                HandleGround();
                CalculateLift();
                CalculateDamping();
                HandlePitch();
                HandleRoll();
                HandleYaw();
                HandleBank();
                AlignToVelocity();
            }
        }

        void CalculateSpeed()
        {
            //rbのワールド座標系での速度をローカル座標系に変換
            Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
            //マイナス→0
            ForwardSpeed = Mathf.Max(0f, localVelocity.z);
        }

        void HandleGround()
        {
            RaycastHit hit;
            Ray ray = new Ray(rb.transform.position, Vector3.down);
            if(Physics.Raycast(ray, out hit))
            {
                float height = hit.distance;
                //rayを発射してgroundに当たったら
                if(hit.transform.tag == "ground" && height < maxGroundDistance)
                {
                    groundEffect = 1f / 
                        (1f + groundEffectCoef * Mathf.Pow(height / wingspan, 2f));
                }
            }
        }

        void CalculateLift()
        {
            // 迎え角を計算
            angleOfAttack = Vector3.Dot(rb.linearVelocity.normalized, transform.forward);
            angleOfAttack *= angleOfAttack;
            // 揚力を計算
            float liftPower = Mathf.Pow(ForwardSpeed, 2)
                * (liftCoeff + flapLiftCoef * (-input.Flap));
            //Debug.Log(liftPower);
            // Y軸方向
            Vector3 liftForce = transform.up * liftPower * angleOfAttack * groundEffect;
            rb.AddForce(liftForce);
        }

        void CalculateDamping()
        {
            rb.linearDamping = initLinearDamping + flapDampingCoef * (-input.Flap);
        }

        void HandlePitch()
        {
            // 速度が遅いときは効き目を落とす
            speedCoef = ForwardSpeed / maxSpeed;
            // 入力に比例したトルク
            Vector3 pitchTorque = input.Pitch * PitchCoef * transform.right * speedCoef;
            // 静安定トルク
            Vector3 staticTorque = -staticCoef * speedCoef * transform.right;
            rb.AddTorque(pitchTorque + staticTorque);
        }

        void HandleRoll()
        {
            //rollAngleを得る
            Vector3 flatRight = transform.right;
            flatRight.y = 0f;
            flatRight = flatRight.normalized;
            rollAngle = Vector3.SignedAngle(transform.right, flatRight, transform.forward);

            if (!input.isAutoRoll)
            {
                // 通常のロール制御
                Vector3 rollTorque = -input.Roll * RollCoef * transform.forward * speedCoef;
                rb.AddTorque(rollTorque);
            }
            else
            {
                // サーボ機能：現在のロール角度を0に近づける　追加
                float servoStrength = -rollAngle * rollServoCoef; // ロール角度に応じた補正力
                float currentRollRate = Vector3.Dot(rb.angularVelocity, transform.forward); // 現在のロール回転速度
                float servoDamping = -currentRollRate * rollServoDamping; // ダンピング項

                Vector3 servoTorque = (servoStrength + servoDamping) * transform.forward * speedCoef;

                rb.AddTorque(servoTorque);
            }
        }

        void HandleYaw()
        {
            //speedCoef = ForwardSpeed / maxSpeed;
            Vector3 yawTorque = input.Yaw * YawCoef * transform.up * speedCoef;
            rb.AddTorque(yawTorque);
        }

        void HandleBank()
        {
            //−90°〜＋90°の範囲で正規化（0〜1に変換）
            float rollNormalized = Mathf.InverseLerp(-90f, 90f, rollAngle);
            //-1〜+1 にマッピング
            float bankStrength = Mathf.Lerp(-1f, 1f, rollNormalized);
            //Yaw軸のトルク
            Vector3 bankTorque = bankStrength * YawCoef * transform.up;
            rb.AddTorque(bankTorque);            
        }

        void AlignToVelocity()
        {
            if (rb.linearVelocity.magnitude > 1f)
            {
                //飛行方向補正
                Vector3 updateVelocity = Vector3.Lerp(
                    //現在の進行方向（物理シミュレーション上の実際の動き）
                    rb.linearVelocity,
                    //機体の「前向きに飛ぶべき理想の速度」
                    transform.forward * ForwardSpeed,
                    //補間量
                    ForwardSpeed * angleOfAttack * Time.deltaTime);
                rb.linearVelocity = updateVelocity;

                //回転補正
                Quaternion updatedRotation = Quaternion.Slerp(
                    //現在の姿勢
                    rb.rotation,
                    //進行方向を向く姿勢
                    Quaternion.LookRotation(
                        rb.linearVelocity.normalized,
                        transform.up),
                    Time.deltaTime);

                rb.MoveRotation(updatedRotation);
            }
        }
        #endregion
        
    }
}