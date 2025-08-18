using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTask を使用するため

/// <summary>
/// 上方向に移動し、初期位置に戻る上下移動床（端で待機してから折り返す）
/// 子オブジェクト化方式で上に乗ったオブジェクトを一緒に動かす
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;   // 床が移動する距離（初期位置からの高さ）
    [SerializeField] private Vector3 moveSpeed = Vector3.zero; // 移動速度（Y軸のみ使用）
    [SerializeField] private float waitTime = 2f;       // 折り返し地点での待機時間（秒）

    private Vector3 _startPosition;     // 床の初期位置
    private bool _movingUp = true;      // 現在の移動方向（true=上昇, false=下降）
    private bool _isWaiting = false;    // 現在待機中かどうか

    [SerializeField] private Rigidbody rigidBody = null; // 床自身の Rigidbody

    /// <summary>
    /// 初期化処理（ゲーム開始時などに呼ばれる）
    /// </summary>
    public override void Initialize() {
        _startPosition = transform.position; // 初期位置を記録
    }

    /// <summary>
    /// リセット処理（再スタート時などに呼ばれる）
    /// </summary>
    public override void SetUp() {
        transform.position = _startPosition; // 床を初期位置に戻す
        _movingUp = true;                    // 初期状態は上方向へ移動
        _isWaiting = false;                  // 待機フラグをリセット
    }

    /// <summary>
    /// 毎フレーム呼ばれる処理
    /// </summary>
    protected override void OnUpdate() {
        if (!_isWaiting) { // 待機中でなければ移動処理を実行
            MovePlatform();
        }
    }

    /// <summary>
    /// 床を上下に動かす処理
    /// </summary>
    private void MovePlatform() {
        float currentY = transform.position.y; // 現在の高さ
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y; // 目標高さ（上端 or 下端）
        float stepY = moveSpeed.y * Time.deltaTime * (_movingUp ? 1f : -1f); // 1フレームで移動する量

        float nextY = currentY + stepY; // 次の高さを計算
        rigidBody.MovePosition(new Vector3(transform.position.x, nextY, transform.position.z)); // 床を移動

        // 折り返し判定（目標地点を通り過ぎたら待機処理へ）
        if (Mathf.Abs(targetY - nextY) > Mathf.Abs(targetY - currentY)) {
            WaitAndTurnAsync(targetY).Forget(); // UniTaskで待機処理を非同期実行
        }
    }

    /// <summary>
    /// 折り返し地点で一定時間待ってから方向を反転する処理
    /// </summary>
    private async UniTaskVoid WaitAndTurnAsync(float targetY) {
        if (_isWaiting) return; // すでに待機中なら何もしない

        _isWaiting = true; // 待機フラグをオン

        // 床を目標位置にピッタリ補正して止める
        rigidBody.MovePosition(new Vector3(transform.position.x, targetY, transform.position.z));
        rigidBody.velocity = Vector3.zero; // 念のため速度もリセット

        // 指定時間待機（シーン終了時に自動キャンセルされる）
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime),
                            cancellationToken: this.GetCancellationTokenOnDestroy());

        // 移動方向を反転
        _movingUp = !_movingUp;
        _isWaiting = false; // 待機終了
    }

    /// <summary>
    /// 床にオブジェクトが乗ったときの処理
    /// → 子オブジェクト化して一緒に動かす
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        // Rigidbody を持っているオブジェクトだけ対象にする
        if (other.attachedRigidbody != null) {
            other.transform.SetParent(transform); // 床の子にすることで一緒に動く
        }
    }

    /// <summary>
    /// 床からオブジェクトが離れたときの処理
    /// → 子オブジェクト関係を解除
    /// </summary>
    private void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody != null) {
            other.transform.SetParent(null); // 親子関係を解除
        }
    }

    /// <summary>
    /// 終了処理（リセット時やシーン終了時に呼ばれる）
    /// </summary>
    public override void Teardown() {
        // 特に子オブジェクト解除処理は不要（OnTriggerExit で外れるため）
    }
}