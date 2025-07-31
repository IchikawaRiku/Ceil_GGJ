using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スイッチギミック：押されると近くのギミックを停止する
/// </summary>
public class Gimmick_Switch : GimmickBase {
    [SerializeField] private float disableRadius; // 停止範囲の半径
    private bool _isPressed = false;                    // 押されたかどうかのフラグ

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        base.Initialize();
        _isPressed = false;
    }

    /// <summary>
    /// 毎回初期化
    /// </summary>
    public override void SetUp() {
        // 押されたかどうか
        _isPressed = false;
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        // デバッグ用の実装
        if (Input.GetKeyDown(KeyCode.Y)) {
            Press();
        }
    }

    /// <summary>
    /// 外部からスイッチが押されたことを通知する
    /// </summary>
    public void Press() {
        // すでに押されているか
        if (_isPressed) return;
        _isPressed = true;

        // 範囲内の全Colliderを取得
        Collider[] hits = Physics.OverlapSphere(transform.position, disableRadius);

        foreach (var hit in hits) {
            // IDisablable を実装しているギミックを検索
            IDisablable target = hit.GetComponent<IDisablable>();
            if (target != null) {
                target.Disable(); // 停止処理を呼び出す
            }
        }

    }

    /// <summary>
    /// ギズモで停止範囲を可視化（Sceneビュー用）
    /// </summary>
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, disableRadius);
    }


    /// <summary>
    /// プレイヤーがエリア内にいるとき
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // スイッチを押される対象に登録
            SwitchUtility.Register(this);
        }
    }

    /// <summary>
    /// プレイヤーが離れたとき
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            // プレイヤーが離れたら解除
            SwitchUtility.Clear();
        }
    }
}