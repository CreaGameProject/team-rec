using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//製作者:水谷
public class KurageAnimationController : MonoBehaviour
{
    [SerializeField] private GameObject body;
    
    [SerializeField] private GameObject deathParticle;
    // Materialに関する設定-------------------------------------------
    [SerializeField] private Material wireMaterial;
    Material _wireMat;
    private Shader _wireMatShader;
    [SerializeField] private Material bodyMatelial;
    Material _bodyMat;
    private Shader _bodyMatShader;
    [ColorUsage(true, true), System.NonSerialized] public Color emissionColor;
    public float myIntensity;
    public float disolvePropotion = 0;
    // ------------------------------------------------------------

    // アニメーションに関する設定---------------------------------------
    private Animator _animator;
    private bool _isAttack = false;

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
    }

    void Update()
    {
        _wireMat.SetColor("_Color", emissionColor * myIntensity);
        _wireMat.SetFloat("_DissolveProportion", disolvePropotion);
        _bodyMat.SetFloat("_DissolveProportion", disolvePropotion);
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
}
