using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_PushBox : GimmickBase {
    [SerializeField]
    private float pushSpeed = 2f;  // 箱を押すスピード

    [SerializeField]
    private float groundCheckDistance = 0.1f;  // 地面チェック用のRaycast距離

    private bool isPushing = false;  // プレイヤーが押しているか
    private Transform playerTransform;  // プレイヤーのTransform参照
    private Vector3 boxPosition;  // 初期位置保存用
    private Rigidbody rb;  // Rigidbodyキャッシュ
    private Vector3 lastPlayerPos;  // 前フレームのプレイヤー位置

    private bool isGrounded = true;  // 地面に接しているかどうか

    // 初期化処理
    public override void Initialize() {
        boxPosition = transform.position;  // 初期位置を保存
        rb = GetComponent<Rigidbody>();    // Rigidbody取得
        if (rb == null) {
        }
    }

    // 準備処理
    public override void SetUp() {
        transform.position = boxPosition;  // 箱を初期位置に戻す
        isPushing = false;
        playerTransform = null;
        lastPlayerPos = Vector3.zero;

        // Rigidbodyの速度をリセット
        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // 今は使われていない
    protected override void OnUpdate() {
    }

    // 物理処理用（固定時間ごとに呼ばれる）
    private void FixedUpdate() {
        CheckGround();  // 地面判定を毎回行う

        if (!isGrounded) {
            // 地面がなければ落下に任せる（Rigidbodyの重力で自然落下）
            return;
        }

        // プレイヤーが押していて、参照が有効で、Rigidbodyがある場合
        if (isPushing && playerTransform != null && rb != null) {
            // 前フレームのプレイヤー位置が未初期化なら初期化だけして抜ける
            if (lastPlayerPos == Vector3.zero) {
                lastPlayerPos = playerTransform.position;
                return;
            }

            // プレイヤーの移動量を取得
            Vector3 playerDelta = playerTransform.position - lastPlayerPos;
            float moveX = playerDelta.x * pushSpeed;  // 押す方向と量

            // 箱の新しい位置を計算
            Vector3 nextPos = rb.position + new Vector3(moveX, 0, 0);

            // RigidbodyをMovePositionで移動
            rb.MovePosition(nextPos);

            // プレイヤー位置を更新
            lastPlayerPos = playerTransform.position;
        }
        else {
            // 押していないときは場所をリセット
            lastPlayerPos = Vector3.zero;
        }
    }

    /// <summary>
    /// 地面に接地しているかどうか、RayCastで調べる
    /// </summary>
    private void CheckGround() {
        Vector3 origin = transform.position + Vector3.up * 0.1f;  // 少し上からRayを飛ばす
        Ray ray = new Ray(origin, Vector3.down);

        // Raycastで下方向にコライダーがあるか判定
        isGrounded = Physics.Raycast(ray, groundCheckDistance + 0.1f);

    }

    /// <summary>
    /// プレイヤーが箱と接地していたら
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) {
        if (!collision.collider.CompareTag("Player")) return;
        // 押している状態にする
        isPushing = true;
        playerTransform = collision.transform;
    }

    /// <summary>
    /// プレイヤーが箱から離れたら
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            isPushing = false;
            playerTransform = null;
        }
    }

    // 箱をリセットする
    public void ResetBox() {
        // 初期位置に戻す
        transform.position = boxPosition;
        // フラグを初期化
        isPushing = false;
        playerTransform = null;
        // プレイヤーの最後の場所を初期化
        lastPlayerPos = Vector3.zero;

        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}