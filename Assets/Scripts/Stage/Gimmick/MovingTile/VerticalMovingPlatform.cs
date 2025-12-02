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


    private Vector3 _startPosition;             // 床の初期位置
    private bool _movingUp = true;              // 現在の移動方向（上方向かどうか）
    private bool _isWaiting = false;            // 待機中かどうか
    private Vector3 _prevPos;                   // 前フレームの床の位置
    private Vector3 _velocity;                  // 床の移動量（プレイヤー補正用）
    private static int waitTimeNum = 1000;      // 待機時間に掛ける値
    private static float toleranceNum = 0.01f;  // プレイヤーの少し上

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        _startPosition = transform.position;
        // 床のコライダーがなければ取得
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
        // 下に移動する時だけ判定する
        if (!_movingUp) {
            // プレイヤーが下にいた場合
            if (CheckPlayerUnderPlatform()) {
                // 停止し、移動再開
                WaitAndTurnAsync(false).Forget();
                return;
            }
        }

        // 通常の移動
        VerticalMove();
    }

    private void VerticalMove() {
        // 現在の高さ
        float currentY = transform.position.y;

        // 上方向か下方向かで目標の位置を設定
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y;

        // MoveTowards で次の位置を計算
        float nextY = Mathf.MoveTowards(currentY, targetY, moveSpeed.y * Time.deltaTime);

        // 移動処理
        rigidBody.MovePosition(new Vector3(
            transform.position.x,
            nextY,
            transform.position.z
        ));

        // 目的地に到達したかどうか
        if (Mathf.Approximately(nextY, targetY)) {
            // 速度を0にする
            _velocity = Vector3.zero;
            // 待機処理へ
            WaitAndTurnAsync(true).Forget();
        }
    }

    /// <summary>
    /// 待機処理
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid WaitAndTurnAsync(bool setValue) {
        // すでに待機中なら処理しない
        if (_isWaiting) return;

        // 待機フラグON
        _isWaiting = true;

        // 指定時間待つ（移動停止）
        await UniTask.Delay((int)(waitTime * waitTimeNum));

        // 反転が必要な場合のみ方向反転
        if (setValue) {
            _movingUp = !_movingUp;
        }

        // 待機フラグ解除
        _isWaiting = false;
    }

    /// <summary>
    /// 上に乗ったプレイヤーの移動
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other) {
        // 対象がRigidbody を持っているか確認
        if (other.attachedRigidbody == null) return;

        // 乗れる対象か確認
        if ((attachableLayers.value & (1 << other.gameObject.layer)) == 0) return;

        // 対象のRigidBodyをキャッシュ
        Rigidbody rb = other.attachedRigidbody;

        float playerBottom = other.bounds.min.y;                // プレイヤーの足元の高さ
        float platformTop = platformCollider.bounds.max.y;      // 床の上面の高さ
        float tolerance = toleranceNum;                         // 上方向への誤差
        bool isOnTop = playerBottom >= platformTop - tolerance; // 床の上にいるかどうかのフラグ

        // 床の上にいない場合は追従させない
        if (!isOnTop) return;

        // プレイヤーを床の移動量だけ移動させる
        rb.MovePosition(rb.position + _velocity);
    }

    /// <summary>
    /// 床の下に判定を作る
    /// </summary>
    /// <returns></returns>
    private bool CheckPlayerUnderPlatform() {
        // 床のコライダーの bounds 取得
        Bounds b = platformCollider.bounds;

        // 判定領域の中心
        Vector3 center = new Vector3(
            b.center.x,
            b.min.y - 0.1f,
            b.center.z
        );

        // 判定領域のサイズ
        Vector3 halfExtents = new Vector3(
            b.extents.x,
            0.05f,
            b.extents.z
        );

        // 判定
        Collider[] hits = Physics.OverlapBox(
            center,
            halfExtents,
            platformCollider.transform.rotation,
            attachableLayers
        );

        return hits.Length > 0;
    }

    // 後片付け処理
    public override void Teardown() {
    }
}