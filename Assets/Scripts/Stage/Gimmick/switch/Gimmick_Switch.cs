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
        // 特に使用しない
    }

    /// <summary>
    /// 外部からスイッチが押されたことを通知する
    /// </summary>
    public void Press() {
        // 押されているか
        if (_isPressed) return;
        // 押した判定にする
        _isPressed = true;

        // 範囲内の全Colliderを取得
        Collider[] hits = Physics.OverlapSphere(transform.position, disableRadius);

        // ぶんまわ～す
        foreach (var hit in hits) {
            // 停止対象
            if (hit.TryGetComponent(out IDisablable disablable)) {
                disablable.Disable();
            }

            // 破壊対象
            if (hit.TryGetComponent(out IDestroyable destroyable)) {
                destroyable.DestroyGimmick();
            }

            // 可視状態切替対象
            if (hit.TryGetComponent(out IVisibleToggleable toggleable)) {
                toggleable.ToggleVisibility();
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
        if (other.CompareTag("ghost")) {
            // スイッチを押される対象に登録
            SwitchUtility.Register(this);
        }
    }

    /// <summary>
    /// プレイヤーが離れたとき
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("ghost")) {
            // プレイヤーが離れたら解除
            SwitchUtility.Clear();
        }
    }
}