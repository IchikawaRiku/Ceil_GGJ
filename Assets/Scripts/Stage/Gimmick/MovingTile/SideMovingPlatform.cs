/*
 *  @file   SideMovingPlatform
 *  @brief  一方向に進み続ける床
 *  @author oorui
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 一方向に進み続ける床
/// </summary>
public class SideMovingPlatform : GimmickBase, IBreakReceiver {

    [SerializeField] private float moveDistance = 0;            // 移動距離
    [SerializeField] private Vector3 moveSpeed = Vector3.zero;  // 移動速度
    [SerializeField] private GameObject targetPos = null;       // 移動先の座標
    [SerializeField] private Rigidbody rigidBody = null;        // 自身のRigitBody
    private bool isMoving = false;      // 移動開始合図を受け取るフラグ
    private Vector3 _startPosition;     // 初期位置

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        // 初期位置を保存
        _startPosition = transform.position;

        // RigidBodyがついていなければ付ける
        if (rigidBody == null) {
            rigidBody = GetComponent<Rigidbody>();
        }
        if (rigidBody == null) {
            // RigidBodyを追加
            rigidBody = gameObject.AddComponent<Rigidbody>();
        }

        rigidBody.useGravity = false;   // 重力むこう 
        rigidBody.isKinematic = true;   // 物理演算無効

    }

    /// <summary>
    /// 準備
    /// </summary>
    public override void SetUp() {
        // 床を初期位置に戻す
        transform.position = _startPosition;
        isMoving = false;
    }



    /// <summary>
    /// 更新処理
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void OnUpdate() {
        // 移動処理
        MovePlatform();
    }

    /// <summary>
    /// 床の移動処理
    /// </summary>
    private void MovePlatform() {
        // 移動開始フラグが false なら移動しない
        if (!isMoving) return;
        // 対象座標オブジェクトが存在しなければ移動しない
        if (targetPos == null) return;

        // 現在の位置と目標の位置を更新
        Vector3 target = targetPos.transform.position;
        Vector3 current = transform.position;

        // 移動
        transform.position =
            Vector3.MoveTowards(current, target, moveSpeed.magnitude * Time.deltaTime);

        // 目標地点に到達したら停止
        if (Vector3.Distance(current, target) < 0.01f) {
            isMoving = false;
        }
    }

    /// <summary>
    /// 移動フラグの変更
    /// </summary>
    public void ChangeMoveFrag() {
        isMoving = true; // 移動開始
    }

    /// <summary>
    /// 壊れた通知を受け取る
    /// </summary>
    public void OnBreak() {
        ChangeMoveFrag();
    }
}
