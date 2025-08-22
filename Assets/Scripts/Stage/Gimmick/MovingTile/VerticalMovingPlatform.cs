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

    private Vector3 _startPosition;     // 床の初期位置
    private bool _movingUp = true;      // 現在の移動方向（上方向かどうか）
    private bool _isWaiting = false;    // 待機中かどうか
    private Vector3 _prevPos;           // 前フレームの床の位置
    private Vector3 _velocity;          // 床の移動量（プレイヤー補正用）

    // 初期化処理
    public override void Initialize() {
        _startPosition = transform.position;
    }

    // 準備処理
    public override void SetUp() {
        transform.position = _startPosition;
        _movingUp = true;
        _isWaiting = false;
        _prevPos = transform.position;
        _velocity = Vector3.zero;
    }

    protected override void OnUpdate() {
    }

    // 更新処理
    private void FixedUpdate() {
        Vector3 prevPosition = transform.position;

        if (!_isWaiting) {
            MovePlatform(); // 床を移動させる
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

    // 床の移動処理
    private void MovePlatform() {
        float currentY = transform.position.y;
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y;

        // MoveTowards で滑らかに移動
        float nextY = Mathf.MoveTowards(currentY, targetY, moveSpeed.y * Time.deltaTime);
        rigidBody.MovePosition(new Vector3(transform.position.x, nextY, transform.position.z));

        // 目標位置に到達したら待機して方向を切り替える
        if (Mathf.Approximately(nextY, targetY)) {
            WaitAndTurnAsync().Forget();
        }
    }

    // 折り返し地点での待機処理
    private async UniTaskVoid WaitAndTurnAsync() {
        if (_isWaiting) return;

        _isWaiting = true;
        await UniTask.Delay((int)(waitTime * 1000)); // 待機時間

        _movingUp = !_movingUp; // 移動方向を反転
        _isWaiting = false;
    }

    // 床の上にいるプレイヤーを移動量に合わせて追従させる
    private void OnTriggerStay(Collider other) {
        if (other.attachedRigidbody != null && ((attachableLayers.value & (1 << other.gameObject.layer)) > 0)) {
            Rigidbody rb = other.attachedRigidbody;
            rb.MovePosition(rb.position + _velocity);
        }
    }

    // 床から離れたときの処理
    private void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody != null && ((attachableLayers.value & (1 << other.gameObject.layer)) > 0)) {
            // other.transform.SetParent(null);
        }
    }

    // 後片付け処理
    public override void Teardown() {
    }
}