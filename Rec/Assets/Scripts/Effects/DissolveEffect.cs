using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    ///<summary>
    ///発光する角の色(HDRで設定)
    ///</summary>
    [ColorUsage(true, true), SerializeField] private Color edgeColor;
    [Range(0f,2f), SerializeField] private float waitDissolveTime = 0.4f;
    private float dissolveTime;
    private float timer = 0.0f;
    private Renderer _renderer;

    private void Start() {
        _renderer = this.GetComponent<Renderer>();
        _renderer.material.SetColor("_EdgeColor", edgeColor);
        timer -= waitDissolveTime;
        dissolveTime = this.GetComponent<ParticleSystem>().main.startLifetimeMultiplier - waitDissolveTime;
        //Debug.Log(GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        //MeshFilter mf = GetComponent<MeshFilter>();
        //mf.mesh.SetIndices(mf.mesh.GetIndices(0), MeshTopology.LineStrip,0);
    }

    private void Update() {
        timer += Time.deltaTime;
        _renderer.material.SetFloat("_DissolveProportion",toDissolveProportion(timer));
    }

    private float toDissolveProportion(float time){
        float proportion = 0.0f;
        if(time >= 0){
            proportion = time / dissolveTime;
        }

        return proportion;
    }



    
}
