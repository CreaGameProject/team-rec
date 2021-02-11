//エフェクトのテスト用に作ったスクリプトなのでHomingとは違います。（水谷）
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHoming : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject target;
    [SerializeField] private float homingStrength;
    [SerializeField] private float velocity;
    private void Start() {
        rb = this.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * 50f * velocity);
    }

    private void FixedUpdate() {
        rb.velocity = rb.velocity.normalized * velocity;
        rb.AddForce((target.transform.position - this.transform.position) * homingStrength);
    }


}
