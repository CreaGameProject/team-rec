using UnityEngine;

namespace Effects
{
    public class DissolveEffect : MonoBehaviour
    {
        ///<summary>
        ///発光する角の色(HDRで設定)
        ///</summary>
        [ColorUsage(true, true), SerializeField] private Color edgeColor;
        [Range(0f,2f), SerializeField] private float waitDissolveTime = 0.4f;
        private float _dissolveTime;
        private float _timer = 0.0f;
        private Renderer _renderer;
        private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");
        private static readonly int DissolveProportion = Shader.PropertyToID("_DissolveProportion");

        private void Start() {
            _renderer = this.GetComponent<Renderer>();
            _renderer.material.SetColor(EdgeColor, edgeColor);
            _timer -= waitDissolveTime;
            _dissolveTime = this.GetComponent<ParticleSystem>().main.startLifetimeMultiplier - waitDissolveTime;
        }

        private void Update() {
            _timer += Time.deltaTime;
            _renderer.material.SetFloat(DissolveProportion,ToDissolveProportion(_timer));
        }

        private float ToDissolveProportion(float time){
            float proportion = 0.0f;
            if(time >= 0){
                proportion = time / _dissolveTime;
            }

            return proportion;
        }
    }
}
