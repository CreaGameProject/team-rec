using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テントウムシ（Ladybug）の子オブジェクトのコンポーネントとして実装
/// </summary>
public class LadybugsBullet : MonoBehaviour
{
    public const int AttackPoint = 10;

    public void Explosion()
    {
        // 爆破エフェクトなど
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);
    }
}
