/*
 *  @file   VerticalMovingPlatform.cs
 *  @brief  上下移動する床
 *  @author oorui
 */

using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


/// <summary>
/// 上下に移動する床
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;          // 床が移動する距離
    [SerializeField] private Vector3 moveSpeed = Vector3.zero; // 床の移動速度
    [SerializeField] private float waitTime = 2f;              // 折り返し地点での待機時間
    [SerializeField] private LayerMask attachableLayers;       // 床に乗れるレイヤー
    [SerializeField] private Rigidbody rigidBody = null;       // 床自身の Rigidbody
    [SerializeField] private Collider platformCollider = null; // 床の Collider


    private Vector3 _startPosition;         // 床の初期位置
    private bool _movingUp = true;          // 現在の移動方向（上方向かどうか）
    private bool _isWaiting = false;        // 待機中かどうか
    private Vector3 _prevPos;               // 前フレームの床の位置
    private Vector3 _velocity;              // 床の移動量（プレイヤー補正用）
    private static int waitTimeNum = 1000;  // 待機時間に掛ける値

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        _startPosition = transform.position;
        if (platformCollider == null) {
            platformCollider = GetComponent<Collider>();
        }
    }

    /// <summary>
    /// 準備処理
    /// </summary>
    public override void SetUp() {
        // 初期位置をスタート位置に設定
        transform.position = _startPosition;
        _movingUp = true;
        _isWaiting = false;
        _prevPos = transform.position;
        _velocity = Vector3.zero;
    }

    protected override void OnUpdate() {
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void FixedUpdate() {
        // 1フレーム前の座標
        Vector3 prevPosition = transform.position;

        // 停止フラグが立っていなければ移動
        if (!_isWaiting) {
            MovePlatform();
        }

        // 床の移動量を計算
        Vector3 rawVelocity = transform.position - prevPosition;

        // 上方向の移動ではプレイヤーを押し上げない
        // 下方向ではそのまま追従させる
        if (_movingUp) {
            _velocity = new Vector3(rawVelocity.x, 0f, rawVelocity.z);
        }
        else {
            _velocity = rawVelocity;
        }

        _prevPos = transform.position;
    }

    /// <summary>
    /// 床の移動処理
    /// </summary>
    private void MovePlatform() {
        // 下方向に移動中にプレイヤーを検知する
        if (!_movingUp) {

            // OverlapBox を使って床の範囲内にいる対象を取得
            Collider[] hits = Physics.OverlapBox(
                platformCollider.bounds.center,
                platformCollider.bounds.extents,
                platformCollider.transform.rotation,
                attachableLayers
            );

            // 真下にプレイヤーがいれば移動を止める
            foreach (Collider hit in hits) {
                if (IsPlayerUnderPlatform(hit)) {
                    // 下方向にいかず、待機する
                    WaitAndTurnAsync().Forget();
                    return;
                }
            }
        }

        // 現在のY座標
        float currentY = transform.position.y;
        // プレイヤーの現在のY座標
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y;

        float nextY = Mathf.MoveTowards(currentY, targetY, moveSpeed.y * Time.deltaTime);

        rigidBody.MovePosition(new Vector3(
            transform.position.x,
            nextY,
            transform.position.z
        ));

        if (Mathf.Approximately(nextY, targetY)) {
            WaitAndTurnAsync().Forget();
        }
    }

    /// <summary>
    /// 待機処理
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid WaitAndTurnAsync() {
        if (_isWaiting) return;

        // フラグを立てる
        _isWaiting = true;
        // 待機時間
        await UniTask.Delay((int)(waitTime * waitTimeNum));

        // 移動方向を反転
        _movingUp = !_movingUp;
        _isWaiting = false;
    }

    /// <summary>
    /// 上に乗ったプレイヤーの移動
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other) {
        // Rigidbody を持っているか確認
        if (other.attachedRigidbody == null) return;

        // 乗れるレイヤーか確認
        if ((attachableLayers.value & (1 << other.gameObject.layer)) == 0) return;

        Rigidbody rb = other.attachedRigidbody;

        // プレイヤーの足元の高さ
        float playerBottom = other.bounds.min.y;

        // 床の上面の高さ
        float platformTop = platformCollider.bounds.max.y;

        // 上方向への誤差
        const float tolerance = 0.01f;

        // 床の上にいるかどうかのフラグ
        bool isOnTop = playerBottom >= platformTop - tolerance;

        // 床の上にいない場合は追従させない
        if (!isOnTop) return;

        // プレイヤーを床の移動量だけ移動させる
        rb.MovePosition(rb.position + _velocity);
    }

    /// <summary>
    /// 床の真下にプレイヤーがいるか判定する
    /// </summary>
    private bool IsPlayerUnderPlatform(Collider other) {
        // プレイヤーの頭の高さ（上端）
        float playerTop = other.bounds.max.y;

        // 床の底面（下端）
        float platformBottom = platformCollider.bounds.min.y;

        // 少し余裕を持たせるための誤差値
        const float tolerance = 0.01f;

        // プレイヤーの頭が床の底面より下にある → 床より下に存在
        return playerTop > platformBottom - tolerance;
    }

    // 床から離れたときの処理
    private void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody != null && ((attachableLayers.value & (1 << other.gameObject.layer)) > 0)) {

        }
    }

    // 後片付け処理
    public override void Teardown() {
    }
}