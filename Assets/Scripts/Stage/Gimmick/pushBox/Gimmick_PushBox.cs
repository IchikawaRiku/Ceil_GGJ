using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_PushBox : GimmickBase {
    // 押されたときの速度
    [SerializeField]
    private float pushForce = 5f;
    // Rigitbody
    private Rigidbody rb;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        // Rigitbodyの取得
        rb = GetComponent<Rigidbody>();

        // 回転を固定化
        rb.constraints = RigidbodyConstraints.FreezeRotation |
                         RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezePositionZ;
    }

    /// <summary>
    /// 押されている間の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) {
        // Playerタグとの接触時のみ反応
        if (!collision.collider.CompareTag("Player")) return;

        // 衝突点の法線を取得
        ContactPoint contact = collision.contacts[0];
        Vector3 normal = contact.normal;

        // X軸のみ動くようにする
        // X軸のある程度の補間を掛けて押しやすくする
        float xDot = Vector3.Dot(normal, Vector3.left);
        if (Mathf.Abs(xDot) < 0.7f) return;

        // X軸方向に速度を与える
        float direction = Mathf.Sign(Vector3.Dot(collision.transform.position - transform.position, Vector3.right));
        rb.velocity = new Vector3(direction * pushForce, 0f, 0f);
    }

    /// <summary>
    /// 離れたときの処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) {
        // プレイヤーとの接触終了時に停止
        if (collision.collider.CompareTag("Player")) {
            rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {

    }
}
