using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private float waitDissolveTime = 0.4f;
    private float dissolveProportion = 0.0f;
    private Renderer rend;

    private void Start() {
        rend = GetComponent<Renderer>();
        Debug.Log(rend.material);
        dissolveProportion -= waitDissolveTime;
        //MeshFilter mf = GetComponent<MeshFilter>();
        //mf.mesh.SetIndices(mf.mesh.GetIndices(0), MeshTopology.LineStrip,0);
    }

    private void Update() {
        dissolveProportion += Time.deltaTime * 0.7f;
        rend.material.SetFloat("_DissolveProportion",dissolveProportion);
    }
}
