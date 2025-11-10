using UnityEngine;
using UnityEngine.InputSystem;

namespace FlightSim
{
    public class InputController : MonoBehaviour
    {
        #region System
        private Vector2 rStickInput; //R-Stick Inputを受け取る
        private Vector2 lStickInput; //L-Stick Inputを受け取る
        private int flapInput; //Flap Inputを受け取る
        private float brakeInput; //Brake Inputを受け取る
        #endregion

        #region Variables
        public float Pitch = 0f; //ピッチ軸
        public float Roll = 0f; //ロール軸
        public float Yaw = 0f; //ヨー軸
        public float Throttle = 0f; //スロットル
        public float Flap = 0f; //フラップ
        public float Brake = 0f; //ブレーキ
        public float RateThrottle; //レート制御スロットル
        private float throttleRate = 1f; //変化率
        public float Trim = 0f;
        public bool isAutoRoll = false;
        public bool isAutoLevel = false;
        public float targetHeight = 0f;
        private Rigidbody rb;
        #endregion

        #region Builtin Methods
        //RStickのコールバック（入力受け取り）
        public void OnRStick(InputAction.CallbackContext ctx)
        {
            rStickInput = ctx.ReadValue<Vector2>();
            //Debug.Log("X= " + rStickInput.x + "Y= " + rStickInput.y);
        }

        //LStickのコールバック（入力受け取り）
        public void OnLStick(InputAction.CallbackContext ctx)
        {
            lStickInput = ctx.ReadValue<Vector2>();
        }

        //FlapDownのコールバック（入力受け取り）
        public void OnFlapDown(InputAction.CallbackContext ctx)
        {
            if (!ctx.canceled) return;
            flapInput += 1;
            if (flapInput > 3) flapInput = 3; //3段階
            
        }

        //FlapUpのコールバック（入力受け取り）
        public void OnFlapUp(InputAction.CallbackContext ctx)
        {
            if (!ctx.canceled) return;
            flapInput -= 1;
            if (flapInput < 0) flapInput = 0;
        }

        //Brakeのコールバック（入力受け取り）
        public void OnBrake(InputAction.CallbackContext ctx)
        {
            brakeInput = ctx.ReadValue<float>();
        }

        public void OnTrimDown(InputAction.CallbackContext ctx)
        {
            //Debug.Log("ok");
            if (!ctx.canceled) return;
            Trim += 0.1f;
            if (Trim > 0.5f) Trim = 0.5f;
        }

        public void OnTrimUp(InputAction.CallbackContext ctx)
        {
            if (!ctx.canceled) return;
            Trim -= 0.1f;
            if (Trim < -0.5f) Trim = -0.5f;
        }
        
        //AutoRollのコールバック（入力受け取り）
        public void OnAutoRoll(InputAction.CallbackContext ctx)
        {
            if (!ctx.canceled) return;
            isAutoRoll = !isAutoRoll;
        }

        //AutoLevelのコールバック（入力受け取り）
        public void OnAutoLevel(InputAction.CallbackContext ctx)
        {
            if (!ctx.canceled) return;
            isAutoLevel = !isAutoLevel;
            targetHeight = rb.transform.position.y;
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>(); //Rigidbodyを取得
        }

        void Update()
        {
            HandleInput();
        }
        #endregion

        #region Custom Methods
        //Input入力を各変数に格納
        void HandleInput()
        {
            Pitch = lStickInput.y + Trim;
            Roll = lStickInput.x;
            Yaw = rStickInput.x;
            Throttle = rStickInput.y;
            Flap = -(float)flapInput / 3f; //0-1の範囲にする
            Brake = brakeInput;
            CalcRateThrottle();
        }

        void CalcRateThrottle()
        {
            RateThrottle = RateThrottle + (Throttle * throttleRate * Time.deltaTime);
            RateThrottle = Mathf.Clamp01(RateThrottle);
        }
        #endregion
    }
}

