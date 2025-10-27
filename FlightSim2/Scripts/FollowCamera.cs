using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlightSim
{
    public class FollowCamera : MonoBehaviour
    {
        #region Variables
        public Transform target; //Airplane
        public float distance = 10f; //機体とカメラの水平距離
        public float height = 3f; //機体とカメラの高さの差
        private float smoothTime = 0.2f; //目標に近づく速度
        private Vector3 smoothVelocity; //内部で使用する変数
        #endregion

        #region Builtin Methods
        void Start(){}

        void FixedUpdate()
        {
            if(target)
            {
                HandleCamera();
            }
        }
        #endregion

        #region Custom Methods
        void HandleCamera()
        {
            // 新規カメラ位置
            //Vector3 up = new Vector3(0f,1f,0f);
            Vector3 newPosition = target.position 
                + (-target.forward * distance) + (Vector3.up * height);
            // 滑らかに到達
            transform.position = Vector3.SmoothDamp(
                transform.position,
                newPosition,
                ref smoothVelocity,
                smoothTime
            );
            //transform.position = newPosition;
            // 機体のほうを向く
            transform.LookAt(target);
        }
        
        #endregion
    }
}
