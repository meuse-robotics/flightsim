using UnityEngine;

namespace FlightSim
{
    public class PropellerController : MonoBehaviour
    {
        #region Variables
        private MeshRenderer meshRenderer; //オブジェクトの描画を担当
        private Material[] mats; //使われているマテリアル
        //public Material blurredMat;
        #endregion

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if(meshRenderer)
            {
                mats = meshRenderer.materials; //マテリアル（複数）を取得
            }
        }

        void Update(){}

        #region Custom Methods
        public void HandlePropeller(float rpm, float throttle)
        {
            // degrees per frame
            float dpf = (rpm * 360f / 60f) * Time.deltaTime;
            transform.Rotate(Vector3.forward, dpf);

            if(mats.Length > 1)
            {
                Color c = mats[1].color; //2番目のマテリアル
                c.a = 1f - throttle / 2f; //透明化（最大で0.5）
                mats[1].color = c; //再設定
                meshRenderer.materials = mats;

                /*c = blurredMat.color;
                if(throttle > 0.1f) c.a = 1f;
                else c.a = 0f;
                blurredMat.color = c;*/
            }
        }
        #endregion
    }

}

