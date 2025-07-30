using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スイッチギミック：押されると近くのギミックを停止する
/// </summary>
public class Gimmick_Switch : GimmickBase {
    [SerializeField] private float disableRadius = 10f; // 停止範囲の半径
    private bool _isPressed = false;                    // 押されたかどうかのフラグ

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        base.Initialize();
        _isPressed = false;
    }

    /// <summary>
    /// 更新処理：今回は使用しない
    /// </summary>
    protected override void OnUpdate() { }

    /// <summary>
    /// 外部からスイッチが押されたことを通知する
    /// </summary>
    public void Press() {
        if (_isPressed) return; // 既に押されていたら無視
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

        Debug.Log($"{gameObject.name} が押され、近くのギミックが停止されました");
    }

    /// <summary>
    /// ギズモで停止範囲を可視化（Sceneビュー用）
    /// </summary>
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, disableRadius);
    }
}