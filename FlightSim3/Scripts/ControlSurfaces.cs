using UnityEngine;
using UnityEngine.Animations;

namespace FlightSim
{
    public enum ControlSurfaceType
    {
        Elevator,
        Rudder,
        Flap,
        L_Aileron,
        R_Aileron,
        L_Controller,
        R_Controller
    }
    public class ControlSurfaces : MonoBehaviour
    {
        #region Variables
        public ControlSurfaceType type;
        public Transform ControlSurfaceMesh; //動かすオブジェクト
        private Vector3 origAngles; //イニシャル角度
        private float maxAngle = 30f; //最大角度
        private float wantedAngle; //目標角度
        private float lastWantedAngle = 0f; //前回目標角度
        private bool rotSurface = false; //角度を更新する
        private Vector3 origMoves; //イニシャル位置
        private float maxMove = 0.1f; //最大可動距離
        private float wantedMove; //目標位置
        private float lastWantedMove = 0f; //前回目標位置
        private bool movSurface = false; //位置を更新する
        private float smoothSpeed = 5f; //ゆっくり動かす程度
        #endregion

        #region Builtin Methods
        void Start()
        {
            origAngles = ControlSurfaceMesh.transform.localEulerAngles;
            origMoves = ControlSurfaceMesh.transform.localPosition;
        }

        void Update()
        {
            if(rotSurface)
            {
                Vector3 localAxis;
                switch (type) //回転軸を設定
                {
                    case ControlSurfaceType.Elevator:
                        localAxis = Vector3.left;
                        break;
                    case ControlSurfaceType.Rudder:
                        localAxis = Vector3.down;
                        break;
                    case ControlSurfaceType.Flap:
                        localAxis = Vector3.back;
                        break;
                    case ControlSurfaceType.L_Aileron:
                        localAxis = Vector3.forward;
                        break;
                    case ControlSurfaceType.R_Aileron:
                        localAxis = Vector3.back;
                        break;
                    case ControlSurfaceType.L_Controller:
                        localAxis = Vector3.forward;
                        break;
                    case ControlSurfaceType.R_Controller:
                        localAxis = Vector3.forward;
                        break;
                    default:
                        localAxis = Vector3.forward;
                        break;
                }
                
                Quaternion targetRotation = Quaternion.Euler(localAxis * wantedAngle);
                ControlSurfaceMesh.localRotation = Quaternion.Slerp(
                    ControlSurfaceMesh.localRotation,
                    Quaternion.Euler(origAngles) * targetRotation,
                    Time.deltaTime * smoothSpeed
                );

                if (Quaternion.Angle(ControlSurfaceMesh.localRotation, targetRotation) < 0.1f)
                    rotSurface = false;
            }
            if(movSurface)
            {
                Vector3 pos = ControlSurfaceMesh.localPosition;
                pos.z = origMoves.z + wantedMove;
                ControlSurfaceMesh.localPosition = pos;
                movSurface = false;
            }
        }
        #endregion

        #region Custom Methods
        public void HandleControlSurface(InputController input)
        {
            float inputValue = 0f;
            float inputValue2 = 0f;
            //inputValue = input.Pitch;
            switch(type)
            {
                case ControlSurfaceType.Rudder:
                    inputValue = input.Yaw;
                    break;
                case ControlSurfaceType.Elevator:
                    inputValue = input.Pitch;
                    break;
                case ControlSurfaceType.Flap:
                    inputValue = input.Flap;
                    break;
                case ControlSurfaceType.L_Aileron:
                    inputValue = input.Roll;
                    break;
                case ControlSurfaceType.R_Aileron:
                    inputValue = input.Roll;
                    break;
                case ControlSurfaceType.L_Controller:
                    inputValue = input.Roll;
                    inputValue2 = input.Pitch;
                    break;
                case ControlSurfaceType.R_Controller:
                    inputValue = input.Roll;
                    inputValue2 = input.Pitch;
                    break;
                default:
                    break;
            }
            wantedAngle = maxAngle * inputValue;
            if(Mathf.Abs(wantedAngle - lastWantedAngle) > 0.1f)
            {
                //入力が変化したら動かす
                rotSurface = true;
            }
            wantedMove = maxMove * inputValue2;
            if (Mathf.Abs(wantedMove - lastWantedMove) > 0.005f)
            {
                //入力が変化したら動かす
                movSurface = true;
                lastWantedMove = wantedMove;
            }
        }
        #endregion
    }

}
