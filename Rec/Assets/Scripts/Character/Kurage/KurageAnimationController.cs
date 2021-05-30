using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//製作者:水谷
public class KurageAnimationController : MonoBehaviour
{
    /// <summary>
    /// ワイヤーフレームのマテリアル
    /// </summary>
    [SerializeField]
    private Material wire_material;
    /// <summary>
    /// 光るワイヤーフレームのHDR
    /// </summary>
    [ColorUsage(true, true), SerializeField]
    private Color _emissionColor;

    /// <summary>
    /// HDRのintensity,光の強さ
    /// </summary>
    public float intensity = 3f;

    /// <summary>
    /// クラゲのアニメーター
    /// </summary>
    private Animator _animator;

    private bool _isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        _emissionColor = wire_material.color;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAttackAnimation();
        }

        wire_material.SetColor("_EmissionColor", _emissionColor * intensity);

    }

    /// <summary>
    /// 攻撃モーションの再生
    /// </summary>
    public void PlayAttackAnimation()
    {
        if (!_isAttack)
        {
            _isAttack = true;
            _animator.SetBool("isAttack", _isAttack);
        }
    }

    /// <summary>
    /// 攻撃モーションの終了
    /// </summary>
    public void FinishAttackAnimation()
    {
        if (_isAttack)
        {
            _isAttack = false;
            _animator.SetBool("isAttack", _isAttack);
        }
    }
}
