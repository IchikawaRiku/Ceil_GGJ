using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 左右に移動する床
/// </summary>
public class HorizontalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;          // 移動距離
    [SerializeField] private Vector3 moveSpeed = Vector3.zero; // 移動速度
    [SerializeField] private float waitTime = 2f;              // 待機時間

    [SerializeField] private LayerMask attachableLayers;       // 子オブジェクトにするLayer

    [SerializeField] private Rigidbody rigidBody = null;       // 自身のRigitBody
    [SerializeField] private PlatformDetector detector;        // 床上検知用

    private Vector3 _startPosition;                            // 初期位置
    private bool _movingRight = true;                          // 移動方向
    private bool _isWaiting = false;                           // 待機中かどうか

    private HashSet<Transform> childrenOnPlatform = new();     // 床に乗っているオブジェクトを管理

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        // 初期位置を保存
        _startPosition = transform.position;

        // Detectorのイベント処理
        if (detector != null) {
            detector.OnDetectedEnter += AttachToPlatform;
            detector.OnDetectedExit += DetachFromPlatform;
        }
    }

    /// <summary>
    /// 準備
    /// </summary>
    public override void SetUp() {
        // 床を初期位置に戻す
        transform.position = _startPosition;

        // 移動状態リセット
        _movingRight = true;
        _isWaiting = false;

        // Detectorのイベント処理
        if (detector != null) {
            detector.OnDetectedEnter += AttachToPlatform;
            detector.OnDetectedExit += DetachFromPlatform;
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        // 待機中でなければ移動処理
        if (!_isWaiting) MovePlatform();
    }

    /// <summary>
    /// 床の移動処理
    /// </summary>
    private void MovePlatform() {
        // 移動先を指定
        float targetX = _movingRight ? _startPosition.x + moveDistance : _startPosition.x;
        float direction = _movingRight ? 1f : -1f;

        // 移動
        rigidBody.velocity = new Vector3(moveSpeed.x * direction, 0f, 0f);

        // 折り返し判定
        if ((_movingRight && transform.position.x >= targetX) ||
            (!_movingRight && transform.position.x <= targetX)) {
            // 折り返し処理を実行
            WaitAndTurnAsync(targetX).Forget();
        }
    }

    /// <summary>
    /// 折り返し時の待機処理
    /// </summary>
    private async UniTaskVoid WaitAndTurnAsync(float targetX) {
        // すでに待機中なら処理しない
        if (_isWaiting) return;
        _isWaiting = true;

        // 移動を停止して位置を補正
        rigidBody.velocity = Vector3.zero;
        rigidBody.MovePosition(new Vector3(targetX, transform.position.y, transform.position.z));

        // 指定時間待機
        await UniTask.Delay((int)(waitTime * 1000));

        // 移動方向を反転
        _movingRight = !_movingRight;

        // 待機終了
        _isWaiting = false;
    }

    /// <summary>
    /// 床にオブジェクトを乗せる処理
    /// </summary>
    private void AttachToPlatform(Rigidbody rb) {
        if (rb != null) {
            // 指定した Layer のみ処理
            if ((attachableLayers.value & (1 << rb.gameObject.layer)) != 0) {
                rb.transform.SetParent(transform, true);
                childrenOnPlatform.Add(rb.transform);    // 管理リストに追加
            }
        }
    }

    /// <summary>
    /// 床からオブジェクトを外す処理
    /// </summary>
    private void DetachFromPlatform(Rigidbody rb) {
        if (rb != null && childrenOnPlatform.Contains(rb.transform)) {
            rb.transform.SetParent(null, true);         // 子オブジェクト解除
            childrenOnPlatform.Remove(rb.transform);    // 管理リストから削除
        }
    }

    /// <summary>
    /// コライダー侵入時の処理
    /// </summary>
    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        AttachToPlatform(rb);
    }

    /// <summary>
    /// コライダー退出時の処理
    /// </summary>
    void OnTriggerExit(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        DetachFromPlatform(rb);
    }

    /// <summary>
    /// 終了時の片付け処理
    /// </summary>
    public override void Teardown() {
        // 床に乗っていたオブジェクトをすべて解除
        foreach (var child in childrenOnPlatform) {
            if (child != null) child.SetParent(null, true);
        }
        childrenOnPlatform.Clear();
    }
}