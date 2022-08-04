using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalAnimationController : MonoBehaviour, IAnimatable
{
    [Header("GameObjectに関する設定")]
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject deathParticle;

    
    [Header("Materialに関する設定")]
    [SerializeField] private List<GameObject> meshDataObjectList = new List<GameObject>();
    private List<SkinnedMeshRenderer> _skinnedMeshRendererList = new List<SkinnedMeshRenderer>();
    
    Material[] _wireMats;
    Material[] _bodyMats;
    
    private List<Shader> _wireMatShaderList = new List<Shader>();
    private List<Shader> _bodyMatShaderList = new List<Shader>();
    
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    [ColorUsage(true, true)] public Color emissionColor;
    public float myIntensity;
    public float disolvePropotion = 0;
    
    
    // アニメーションに関する設定---------------------------------------
    private Animator _animator;
    private bool _isAttack = false;
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    
    private void Start()
    {
        meshDataObjectList = GetChildrenList(this.gameObject);
        _skinnedMeshRendererList = GetComponentSkinnedMeshRendererList(meshDataObjectList);
        
        // meshDataObjectListに含まれるBeeWireマテリアルとBeeBodyマテリアルを各々インスタンス化して独立させる.
        // 同様の措置はクラゲたちでも行っている。KurageAnimationController.cs 参考
        foreach (var smr in _skinnedMeshRendererList)
        {
            var materials = smr.materials;
            Shader wireMatShader = materials[0].shader;
            Shader bodyMatShader = materials[1].shader;
            _wireMatShaderList.Add(wireMatShader);
            _bodyMatShaderList.Add(bodyMatShader);
            Material wireMat = new Material(wireMatShader);
            Material bodyMat = new Material(bodyMatShader);
            wireMat.color = materials[0].color;
            bodyMat.color = materials[1].color;
            materials[0] = wireMat;
            materials[1] = bodyMat;
        }
        
        for (int i = 0; i < _wireMatShaderList.Count; i++)
        {
            _skinnedMeshRendererList[i].materials[0].SetColor("_Color", emissionColor * myIntensity);
            _skinnedMeshRendererList[i].materials[0].SetFloat("_DissolveProportion", disolvePropotion);
            _skinnedMeshRendererList[i].materials[1].SetFloat("_DissolveProportion", disolvePropotion);
            // Debug.Log(_skinnedMeshRendererList[i].gameObject.name + "は、_skinnedMeshRendererListの" + i + "番目です.");
        }
        
        // アニメーターの取得
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        for (int i = 0; i < _wireMatShaderList.Count; i++)
        {
            _skinnedMeshRendererList[i].materials[0].SetFloat("_DissolveProportion", disolvePropotion);
            _skinnedMeshRendererList[i].materials[1].SetFloat("_DissolveProportion", disolvePropotion);
            
        }
    }

    public void OnAttack(int index)
    {
        switch (index)
        {
            case 0:
                PlayAttackAnimation();
                break;
            default:
                Debug.LogError("範囲より大きい数字が指定されています。0を指定してください。");
                break;
        }
    }

    public void PlayAttackAnimation()
    {
        if (!_isAttack)
        {
            _isAttack = true;
            _animator.SetBool("isAttack", _isAttack);
        }
    }

    public void FinishAttackAnimation()
    {
        if (_isAttack)
        {
            _isAttack = false;
            _animator.SetBool("isAttack", _isAttack);
        }
    }
    
    public void OnDie()
    {
        Invoke("PlayDeathParticle", 1f);
        DOTween.To(
            () => disolvePropotion,
            (value) => disolvePropotion = value,
            1.0f,
            1.5f
        );
    }

    void PlayDeathParticle()
    {
        deathParticle.SetActive(true);
        deathParticle.GetComponent<ParticleSystem>().Play();
        //Debug.Log(_bodyMat.GetFloat("_DissolveProportion"));
    }
    
    List<GameObject> GetChildrenList(GameObject obj)
    {
        List<GameObject> list = new List<GameObject>();
        Transform children = obj.GetComponentInChildren<Transform>();
        if (children.childCount == 0)
        {
            Debug.LogError(this.gameObject.name + "には子オブジェクトがない為、子オブジェクトを取得できませんでした.");
        }

        foreach (Transform ob in children)
        {
            if (ob.gameObject.CompareTag("MeshData"))
            {
                list.Add(ob.gameObject);
            }
        }

        return list;
    }
    
    List<SkinnedMeshRenderer> GetComponentSkinnedMeshRendererList(List<GameObject> objects)
    {
        List<SkinnedMeshRenderer> list = new List<SkinnedMeshRenderer>();
        foreach (var obj in objects)
        {
            SkinnedMeshRenderer smr = obj.GetComponent<SkinnedMeshRenderer>();
            list.Add(smr);
        }

        return list;
    }
}
