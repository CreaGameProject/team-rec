using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Material[] _scalesMats = new Material[4];
    private Shader[] _scalesMatShaders = new Shader[4];

    [ColorUsage(true, true), System.NonSerialized]
    public Color emissionColor;

    public float myIntensity;

    public float disolvePropotion = 0;
    // ------------------------------------------------------------

    // アニメーションに関する設定---------------------------------------
    private Animator _animator;
    private bool _isAttack1 = false;
    private bool _isAttack2 = false;

    // Start is called before the first frame update
    void Start()
    {

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
        
        

        _animator = GetComponent<Animator>();

        foreach (var particle in scalesParticles)
        {
            particle.Stop();
            Debug.Log(particle + "Stopped.");
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
        if (Input.GetKeyDown(KeyCode.Space)) PlayAttackAnimation1();

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
        if (!_isAttack1)
        {
            _isAttack1 = true;
            _animator.SetBool("isAttack2", _isAttack1);

            foreach (var particle in scalesParticles)
            {
                particle.Play();
                Debug.Log(particle + "Played.");
            }
        }
    }

    public void FinishAttackAnimation2()
    {
        if (_isAttack1)
        {
            _isAttack1 = false;
            _animator.SetBool("isAttack2", _isAttack1);

            foreach (var particle in scalesParticles)
            {
                particle.Stop();
            }
        }
    }


    private void ChangeScalesParticles(GameObject particle, float emission, Color color)
    {
        var emissionModule = particle.GetComponent<ParticleSystem>().emission;
        emissionModule.rateOverDistance = emission;
    }
}
