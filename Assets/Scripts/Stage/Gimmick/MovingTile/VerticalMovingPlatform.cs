using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTask を使用するため

/// <summary>
/// 上下に移動する床
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;          // 床が移動する距離
    [SerializeField] private Vector3 moveSpeed = Vector3.zero; // 移動速度
    [SerializeField] private float waitTime = 2f;              // 折り返し地点の待機時間

    private Vector3 _startPosition;     // 床の初期位置
    private bool _movingUp = true;      // 現在の移動方向
    private bool _isWaiting = false;    // 待機中かどうか

    [SerializeField] private Rigidbody rigidBody = null; // 床自身の Rigidbody

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        // 初期位置を指定
        _startPosition = transform.position;
    }

    /// <summary>
    /// 準備処理
    /// </summary>
    public override void SetUp() {
        transform.position = _startPosition; // 床を初期位置に戻す
        _movingUp = true;                    // 初期状態は上方向へ移動
        _isWaiting = false;                  // 待機フラグをリセット
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        // 移動
        if (!_isWaiting) MovePlatform();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void MovePlatform() {
        float currentY = transform.position.y; // 現在の高さ
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y; // 上下の上限値
        float stepY = moveSpeed.y * Time.deltaTime * (_movingUp ? 1f : -1f); // 1フレームで移動する量

        float nextY = currentY + stepY; // 次の高さを計算
        rigidBody.MovePosition(new Vector3(transform.position.x, nextY, transform.position.z)); // 床を移動

        // 折り返し判定
        if (Mathf.Abs(targetY - nextY) > Mathf.Abs(targetY - currentY)) {
            // 待機してから折り返し
            WaitAndTurnAsync(targetY).Forget();
        }
    }

    /// <summary>
    /// 折り返し処理
    /// </summary>
    private async UniTaskVoid WaitAndTurnAsync(float targetY) {
        if (_isWaiting) return;
        // 待機中にする
        _isWaiting = true;

        // 床を目標位置で停止
        rigidBody.MovePosition(new Vector3(transform.position.x, targetY, transform.position.z));
        //rigidBody.velocity = Vector3.zero;

        // 待機
        await UniTask.Delay((int)(waitTime * 1000));
        // 移動方向を反転
        _movingUp = !_movingUp;
        // 待機終了
        _isWaiting = false;
    }

    /// <summary>
    /// 子オブジェクトに指定
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        // Rigidbody を持っているオブジェクトだけ対象にする
        if (other.attachedRigidbody != null) {
            other.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// 子オブジェクトから外す
    /// </summary>
    private void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody != null) {
            other.transform.SetParent(null);
        }
    }

    /// <summary>
    /// 片付け処理
    /// </summary>
    public override void Teardown() {
    
    }
}