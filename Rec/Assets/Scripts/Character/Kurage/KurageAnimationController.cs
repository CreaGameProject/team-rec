using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//製作者:水谷
public class KurageAnimationController : MonoBehaviour
{
    [SerializeField] private GameObject body;

    [SerializeField] private Material wireMaterial;
    Material _wireMat;
    private Shader _wireMatShader;
    [SerializeField] private Material bodyMatelial;
    Material _bodyMat;
    private Shader _bodyMatShader;

    [ColorUsage(true, true), System.NonSerialized] public Color emissionColor;

    public float myIntensity;

    private Animator _animator;
    private bool _isAttack = false;

    [SerializeField] private GameObject deathParticle;

    void Start()
    {
        _wireMatShader = wireMaterial.shader;
        _bodyMatShader = bodyMatelial.shader;
        _wireMat = new Material(_wireMatShader);
        _wireMat.color = wireMaterial.color;
        _bodyMat = new Material(_bodyMatShader);
        _bodyMat.color = bodyMatelial.color;
        body.GetComponent<SkinnedMeshRenderer>().materials[0] = _wireMat;
        body.GetComponent<SkinnedMeshRenderer>().materials[1] = _bodyMat;

        emissionColor = wireMaterial.color;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAttackAnimation();
        }

        _wireMat.SetColor("_Color", emissionColor * myIntensity);
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
        Invoke("PlayDeathParticle", 1.0f);
    }

    void PlayDeathParticle()
    {
        Debug.Log("deathParticle");
        deathParticle.GetComponent<ParticleSystem>().Play();
    }
}
