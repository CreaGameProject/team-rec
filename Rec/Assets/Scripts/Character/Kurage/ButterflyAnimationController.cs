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
    [SerializeField] private Material wireMaterial;
    Material _wireMat;
    private Shader _wireMatShader;
    [SerializeField] private Material bodyMatelial;
    Material _bodyMat;
    private Shader _bodyMatShader;
    [SerializeField] private Material[] scalesMatelials = new Material[4];
    Material[] _scalesMats;
    private Shader[] _scalesMatShaders;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [ColorUsage(true, true), System.NonSerialized]
    public Color emissionColor;

    // 鱗粉の色
    [ColorUsage(true, true), SerializeField]
    private Color scalesNormalColor;
    [ColorUsage(true, true), SerializeField]
    private Color scalesAttackColor;

    public float myIntensity;

    public float disolvePropotion = 0;
    // ------------------------------------------------------------

    // アニメーションに関する設定---------------------------------------
    private Animator _animator;
    private bool _isAttack1 = false;
    private bool _isAttack2 = false;
    private readonly float scalesDoTime = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        // 4種のマテリアルがある為、このコードでは動かない
        /*
        _wireMatShader = wireMaterial.shader;
        _bodyMatShader = bodyMatelial.shader;
        _wireMat = new Material(_wireMatShader);
        _wireMat.color = wireMaterial.color;
        _bodyMat = new Material(_bodyMatShader);
        _bodyMat.color = bodyMatelial.color;
        body.GetComponent<SkinnedMeshRenderer>().materials[0] = _wireMat;
        _wireMat = body.GetComponent<SkinnedMeshRenderer>().materials[0];
        body.GetComponent<SkinnedMeshRenderer>().materials[1] = _bodyMat;
        _bodyMat = body.GetComponent<SkinnedMeshRenderer>().materials[1];
        emissionColor = wireMaterial.color;
        */

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

        //test
        if (Input.GetKeyDown(KeyCode.Space)) PlayAttackAnimation2();

        
        if (_isAttack1)
        {
            
        }
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

        seq.Append(material.DOColor(color,EmissionColor,scalesDoTime).SetEase(Ease.Linear));

        seq.Play();
        Debug.Log(seq);
    }
}