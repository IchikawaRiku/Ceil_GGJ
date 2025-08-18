using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 左右方向に移動する床
/// </summary>
public class HorizontalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;               // 右方向に移動する距離
    [SerializeField] private Vector3 moveSpeed = Vector3.zero;      // 移動速度 (X軸のみ使用)

    private Vector3 _startPosition;                                 // 初期位置
    private bool _movingRight = true;                               // 現在の移動方向
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
        _movingRight = true;
   
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
    /// プラットフォームを左右に動かす処理
    /// </summary>
    private void MovePlatform() {
        float currentX = transform.position.x;
        float targetX = _movingRight ? _startPosition.x + moveDistance : _startPosition.x;
        float stepX = moveSpeed.x * Time.deltaTime * (_movingRight ? 1f : -1f);

        float nextX = currentX + stepX;
        rigidBody.MovePosition(new Vector3(nextX, transform.position.y, transform.position.z));

        // 折り返し判定（目標地点を通り過ぎたら方向を反転）
        if (Mathf.Abs(targetX - nextX) > Mathf.Abs(targetX - currentX)) {
            _movingRight = !_movingRight;
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
        // リストから削除
        rigidBodys.Clear();


    }
}