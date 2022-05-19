using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class ButterflyAnimationController : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private ParticleSystem[] scalesParticles = new ParticleSystem[4];

    [SerializeField] private GameObject deathParticle;

    // Materialに関する設定-------------------------------------------
    [SerializeField] private List<GameObject> meshDataObjectList = new List<GameObject>();
    private List<SkinnedMeshRenderer> _skinnedMeshRendererList = new List<SkinnedMeshRenderer>();
    
    [SerializeField] private Material[] scalesMatelials = new Material[4];
    
    Material[] _wireMats;
    Material[] _bodyMats;
    Material[] _scalesMats;
    
    private List<Shader> _wireMatShaderList = new List<Shader>();
    private List<Shader> _bodyMatShaderList = new List<Shader>();
    private Shader[] _scalesMatShaders;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [ColorUsage(true, true)]
    public Color emissionColor;

    // 鱗粉の色
    [ColorUsage(true, true), SerializeField]
    private Color scalesNormalColor;

    [ColorUsage(true, true), SerializeField]
    private Color scalesAttackColor;

    public float myIntensity;

    [Range(0.0f, 1.0f)] public float disolvePropotion = 0;
    // ------------------------------------------------------------

    // アニメーションに関する設定---------------------------------------
    private Animator _animator;
    private bool _isAttack1 = false;
    private bool _isAttack2 = false;
    private readonly float scalesDoTime = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        // 子オブジェクトにあるMeshDataタグがついているオブジェクトを取得 書き方は蝶専用になっているので要検討
        meshDataObjectList = GetChildrenList(this.gameObject);
        _skinnedMeshRendererList = GetComponentSkinnedMeshRendererList(meshDataObjectList);

        // meshDataObjectListに含まれるButterflyWireマテリアルとButterflyBodyマテリアルを各々インスタンス化して独立させる.
        // 同様の措置はクラゲたちでも行っている。KurageAnimationController.cs 参考
        foreach (var smr in _skinnedMeshRendererList)
        {
            //wireMaterialList.Add(smr.materials[0]);
            //bodyMatelialList.Add(smr.materials[1]);
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
        


        _animator = GetComponent<Animator>();

        /*
        foreach (var particle in scalesParticles)
        {
            particle.Stop();
            Debug.Log(particle + "Stopped.");
        }
        */


        int sPLength = scalesParticles.Length;

        _scalesMats = new Material[sPLength];
        _scalesMatShaders = new Shader[sPLength];


        for (int i = 0; i < sPLength; i++)
        {
            scalesMatelials[i] = scalesParticles[i].GetComponent<ParticleSystemRenderer>().material;
            Debug.Log(scalesMatelials[i]);
            _scalesMatShaders[i] = scalesMatelials[i].shader;
            _scalesMats[i] = new Material(_scalesMatShaders[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
         _wireMat.SetColor("_Color", emissionColor * myIntensity);
        _wireMat.SetFloat("_DissolveProportion", disolvePropotion);
        _bodyMat.SetFloat("_DissolveProportion", disolvePropotion);
         */

        for (int i = 0; i < _wireMatShaderList.Count; i++)
        {
            _skinnedMeshRendererList[i].materials[0].SetColor("_Color", emissionColor * myIntensity);
            _skinnedMeshRendererList[i].materials[0].SetFloat("_DissolveProportion", disolvePropotion);
            _skinnedMeshRendererList[i].materials[1].SetFloat("_DissolveProportion", disolvePropotion);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Space)) OnDie();


        if (_isAttack1)
        {
        }
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


    public void PlayAttackAnimation1()
    {
        if (!_isAttack1)
        {
            _isAttack1 = true;
            _animator.SetBool("isAttack1", _isAttack1);

            foreach (var particle in scalesParticles)
            {
                particle.Play();
                Debug.Log(particle + "Played.");
            }
        }
    }

    public void FinishAttackAnimation1()
    {
        if (_isAttack1)
        {
            _isAttack1 = false;
            _animator.SetBool("isAttack1", _isAttack1);

            foreach (var particle in scalesParticles)
            {
                particle.Stop();
            }
        }
    }

    public void PlayAttackAnimation2()
    {
        if (!_isAttack2)
        {
            _isAttack2 = true;
            _animator.SetBool("isAttack2", _isAttack2);

            for (int i = 0; i < scalesParticles.Length; i++)
            {
                scalesParticles[i].Play();
                ChangeScalesParticles(scalesParticles[i].gameObject, scalesMatelials[i], 50, scalesAttackColor);
            }
        }
    }

    public void FinishAttackAnimation2()
    {
        if (_isAttack2)
        {
            _isAttack2 = false;
            _animator.SetBool("isAttack2", _isAttack2);

            foreach (var particle in scalesParticles)
            {
                particle.Stop();
            }
        }
    }


    private void ChangeScalesParticles(GameObject particle, Material material, float emission, Color color)
    {
        var seq = DOTween.Sequence();

        var emissionModule = particle.GetComponent<ParticleSystem>().emission;
        emissionModule.rateOverDistance = emission;
        //material.SetColor(EmissionColor, color);

        seq.Append(material.DOColor(color, EmissionColor, scalesDoTime).SetEase(Ease.Linear));

        seq.Play();
        Debug.Log(seq);
    }
    
    public void OnDie()
    {
        Invoke(nameof(PlayDeathParticle), 2f);
        DOTween.To(
            () => disolvePropotion,
            (value) => disolvePropotion = value,
            1.0f,
            3.0f
        );
    }

    void PlayDeathParticle()
    {
        deathParticle.SetActive(true);
        deathParticle.GetComponent<ParticleSystem>().Play();

        foreach (var scalesParticle in scalesParticles)
        {
            scalesParticle.transform.SetParent(null);
            scalesParticle.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}