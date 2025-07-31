using UnityEngine;

/// <summary>
/// 四角形のパスを移動する床
/// </summary>
public class SquarePathPlatform : GimmickBase {
    [SerializeField] private float sideLength = 2f;     // 各辺の長さ
    [SerializeField] private float moveSpeed = 1f;      // 移動速度

    private Vector3[] _waypoints;                       // 四角形の頂点
    private int _currentIndex = 0;                      // 現在の移動先インデックス
    private Vector3 _startPosition;                     // 初期位置

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize() {
        // 初期生成位置を記憶
        _startPosition = transform.position;

        // ウェイポイントを計算
        GenerateWaypoints();
    }

    /// <summary>
    /// 準備
    /// </summary>
    public override void SetUp() {
        // 生成位置を初期化
        transform.position = _startPosition;

        // ウェイポイントを再生成
        GenerateWaypoints();

        // 現在の移動インデックスをリセット
        _currentIndex = 0;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        // 現在の目標地点を取得
        Vector3 target = _waypoints[_currentIndex];
        float step = moveSpeed * Time.deltaTime;

        // 移動
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // 到達したら次のポイントへ
        if (Vector3.Distance(transform.position, target) < 0.01f) {
            _currentIndex = (_currentIndex + 1) % _waypoints.Length;
        }
    }

    /// <summary>
    /// 四角形のウェイポイントを生成する
    /// </summary>
    private void GenerateWaypoints() {
        _waypoints = new Vector3[4];
        _waypoints[0] = _startPosition;
        _waypoints[1] = _startPosition + Vector3.right * sideLength;
        _waypoints[2] = _waypoints[1] + Vector3.up * sideLength;
        _waypoints[3] = _waypoints[2] - Vector3.right * sideLength;
    }
}