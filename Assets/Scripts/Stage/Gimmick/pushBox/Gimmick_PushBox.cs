using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_PushBox : GimmickBase {
    [SerializeField]
    private float pushSpeed = 2f;

    // 押しているフラグ
    private bool isPushing = false;

    // プレイヤーのTransform
    private Transform playerTransform;

    // 初期位置保存用
    private Vector3 boxPosition;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        // 初期生成位置を記憶
        boxPosition = transform.position;
    }

    /// <summary>
    /// 使用前準備
    /// </summary>
    public override void SetUp() {
        // 生成位置、各フラグを初期化
        transform.position = boxPosition;
        isPushing = false;
        playerTransform = null;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        if (isPushing && playerTransform != null) {
            // プレイヤーと箱の距離を正規化して代入
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

    /// <summary>
    /// 箱の位置を初期化
    /// </summary>
    public void ResetBox() {
        transform.position = boxPosition;
        isPushing = false;
        playerTransform = null;

        // 必要なら物理特性もリセット（Rigidbody付きの場合）
        var rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
