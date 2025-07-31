using UnityEngine;

/// <summary>
/// 上方向に移動し、初期位置に戻る上下移動床
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;   // 上方向に移動する距離
    [SerializeField] private float moveSpeed = 1f;      // 移動速度

    private Vector3 _startPosition;                     // 初期生成位置を保存
    private bool _movingUp = true;                      // 上方向に移動中かどうか

    /// <summary>
    /// 初期化処理（初期生成位置を記録）
    /// </summary>
    public override void Initialize() {
        _startPosition = transform.position;
    }

    /// <summary>
    /// 準備処理（位置と状態を初期化）
    /// </summary>
    public override void SetUp() {
        transform.position = _startPosition;
        _movingUp = true;
    }

    /// <summary>
    /// 毎フレームの更新処理
    /// </summary>
    protected override void OnUpdate() {
        float step = moveSpeed * Time.deltaTime;
        Vector3 target;

        // 上昇中か下降中かで目標地点を設定
        if (_movingUp) {
            target = _startPosition + Vector3.up * moveDistance;
        }
        else {
            target = _startPosition;
        }

        // 現在位置を目標地点に向かって移動
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // 目標地点に到達したら方向を反転
        if (Vector3.Distance(transform.position, target) < 0.01f) {
            _movingUp = !_movingUp;
        }
    }
}