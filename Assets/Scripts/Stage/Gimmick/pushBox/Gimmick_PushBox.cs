using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_PushBox : GimmickBase {
    [SerializeField]
    private float pushSpeed = 2f;

    // 押しているフラグ
    private bool isPushing = false;

    // プレイヤーのTransform参照
    private Transform playerTransform;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        if (isPushing && playerTransform != null) {
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // 押す方向がX軸成分強めの時だけ移動（X方向にしか動かさない制限）
            if (Mathf.Abs(direction.x) > 0.7f) {
                float pushDir = Mathf.Sign(direction.x);
                transform.position += new Vector3(pushDir * pushSpeed * Time.deltaTime, 0, 0);
            }
        }
    }

    /// <summary>
    /// プレイヤーが押し続けている間
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) {
        if (!collision.collider.CompareTag("Player")) return;

        isPushing = true;
        playerTransform = collision.transform;
    }

    /// <summary>
    /// プレイヤーと離れたとき
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            isPushing = false;
            playerTransform = null;
        }
    }
}
