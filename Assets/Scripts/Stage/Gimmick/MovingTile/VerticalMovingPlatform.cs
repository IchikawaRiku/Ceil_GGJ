using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 上方向に移動し、初期位置に戻る上下移動床
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;               // 上方向に移動する距離
    [SerializeField] private Vector3 moveSpeed = Vector3.zero;      // 移動速度 (Y軸のみ使用)

    private Vector3 _startPosition;                                 // 初期位置
    private bool _movingUp = true;                                  // 現在の移動方向
    [SerializeField] private Rigidbody rigidBody = null;            // この床の Rigidbody
    private List<Rigidbody> rigidBodys = new();                     // 上に乗ったオブジェクトの Rigidbody
    [SerializeField] private PlatformDetector detector;             // 子オブジェクトのスクリプトをInspectorでセット

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        _startPosition = transform.position;

        if (detector != null) {
            detector.OnDetectedEnter += rb => {
                if (!rigidBodys.Contains(rb)) rigidBodys.Add(rb);
            };
            detector.OnDetectedExit += rb => {
                rigidBodys.Remove(rb);
            };
        }
    }

    /// <summary>
    /// セットアップ処理
    /// </summary>
    public override void SetUp() {
        transform.position = _startPosition;
        _movingUp = true;

        if (detector != null) {
            detector.OnDetectedEnter += rb => {
                if (!rigidBodys.Contains(rb)) rigidBodys.Add(rb);
            };
            detector.OnDetectedExit += rb => {
                rigidBodys.Remove(rb);
            };
        }
    }

    /// <summary>
    /// 毎フレーム更新処理
    /// </summary>
    protected override void OnUpdate() {
        MovePlatform();
    }

    private void FixedUpdate() {
        AddVelocity();
    }

    /// <summary>
    /// プラットフォームを上下に動かす処理
    /// </summary>
    private void MovePlatform() {
        float currentY = transform.position.y;
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y;
        float stepY = moveSpeed.y * Time.deltaTime * (_movingUp ? 1f : -1f);

        float nextY = currentY + stepY;
        rigidBody.MovePosition(new Vector3(transform.position.x, nextY, transform.position.z));

        // 折り返し判定（目標地点を通り過ぎたら方向を反転）
        if (Mathf.Abs(targetY - nextY) > Mathf.Abs(targetY - currentY)) {
            _movingUp = !_movingUp;
        }
    }

    /// <summary>
    /// トリガーに入ったオブジェクトのRigidbodyを登録
    /// </summary>
    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null && !rigidBodys.Contains(rb)) {
            rigidBodys.Add(rb);
        }
    }

    /// <summary>
    /// トリガーから出たオブジェクトのRigidbodyを削除
    /// </summary>
    void OnTriggerExit(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null) {
            rigidBodys.Remove(rb);
        }
    }

    /// <summary>
    /// 移動床の速度を上に乗っているオブジェクトに反映
    /// </summary>
    void AddVelocity() {
        if (rigidBody == null || rigidBody.velocity.sqrMagnitude <= 0.001f) return;

        Vector3 platformVelocity = rigidBody.velocity;

        foreach (var rb in rigidBodys) {
            if (rb != null) {
                Vector3 nextPos = rb.position + platformVelocity * Time.fixedDeltaTime;
                rb.MovePosition(nextPos);
            }
        }
    }

    /// <summary>
    /// 片付け処理
    /// </summary>
    public override void Teardown() {
        rigidBodys.Clear();
    }
}