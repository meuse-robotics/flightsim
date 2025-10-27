using UnityEngine;

namespace FlightSim
{
    public class Attitude : MonoBehaviour
    {
        #region Variables
        public MainController Airplane;
        public RectTransform bgRect;
        #endregion

        #region Custom Methods
        public void HandleUI()
        {
            if (Airplane)
            {
                //姿勢
                float bankAngle = Vector3.Dot(Airplane.transform.right, Vector3.up) * Mathf.Rad2Deg;
                float pitchAngle = Vector3.Dot(Airplane.transform.forward, Vector3.up) * Mathf.Rad2Deg;

                //画像を移動
                if (bgRect)
                {
                    Quaternion bankRot = Quaternion.Euler(0f, 0f, bankAngle);
                    bgRect.transform.localRotation = bankRot;

                    Vector3 wantedPosition = new Vector3(0f, -pitchAngle, 0f);
                    bgRect.anchoredPosition = wantedPosition;
                }
            }
        }
        #endregion
    }
}
