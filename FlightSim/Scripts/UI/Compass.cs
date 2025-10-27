using UnityEngine;

namespace FlightSim
{
    public class Compass : MonoBehaviour
    {
        #region Variables
        public MainController Airplane;
        public RectTransform bgRect; //動かす画像
        #endregion

        #region Custom Methods
        public void HandleUI()
        {
            if(Airplane)
            {
                //Y軸回りの回転
                float yawAngle = Vector3.SignedAngle(
                    Vector3.forward,
                    Airplane.transform.forward,
                    Vector3.up);
                Quaternion yawRot = Quaternion.Euler(0f, 0f, yawAngle);
                bgRect.transform.rotation = yawRot;
                Vector3 wantedPosition = new Vector3(0f, 0f, 0f);
                bgRect.anchoredPosition = wantedPosition;
            }
        }
        #endregion
    }
}