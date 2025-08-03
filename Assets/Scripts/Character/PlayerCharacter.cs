/*
 *  @file   PlayerCharacter.cs
 *  @brief  プレイヤーのキャラクター
 *  @author Riku
 *  @date   2025/7/29
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class PlayerCharacter : CharacterBase {
	//Animator animator;

	// 地面のレイヤー
	private LayerMask _groundLayer;
	// ジャンプ力
	private float _jumpPower = 5f;
	// プレイヤーの座標から足元座標までの距離
	private const float _FEET_DISTANCE = 0f;
	// 地面判定用の半径
	private const float _FEET_RADIUS = 0.05f;

	public override async UniTask Initialize() {
		await base.Initialize();
		_groundLayer = LayerMask.GetMask("Jump");
		//animator = GetComponent<Animator>();
	}

	/// <summary>
	/// 実行処理
	/// </summary>
	public override async UniTask Execute() {
		await base.Execute();
		moveValue = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.position += moveValue;
		// カメラの位置をセット
		CameraManager.instance.SetPosition(transform.position);
		// デバッグ用
		if (Input.GetKeyDown(KeyCode.U)) {
			OnJump();
		}
		if (GetTouchGround()) anim.SetBool("jump", false);
        else anim.SetBool("jump", true);
	}

	private void OnDrawGizmos() {
		Vector3 position = transform.position;
		position.y -= _FEET_DISTANCE;
		bool hit = Physics.CheckSphere(position, _FEET_RADIUS, _groundLayer);
		Color hitColor = Color.red;
		Color noHitColor = Color.green;
		Gizmos.color = hit ? hitColor : noHitColor;
		Gizmos.DrawWireSphere(position, _FEET_RADIUS);
	}

	/// <summary>
	/// 地面触れ判定
	/// </summary>
	private bool GetTouchGround() {
		Vector3 position = transform.position;
        position.y -= _FEET_DISTANCE;
		return Physics.CheckSphere(position, _FEET_RADIUS, _groundLayer);		
	}

	/// <summary>
	/// 当たっているとき
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag(_BULLET_TAG)) {
			Debug.Log("死んだ");
		}
	}

    public override void OnMove(InputAction.CallbackContext context) {
        base.OnMove(context);
        if (context.started && GetTouchGround()) anim.SetBool("run", true);
        if (context.canceled) anim.SetBool("run", false);
    }

    /// <summary>
    /// ジャンプの入力
    /// </summary>
    private void OnJump() {
		if (!GetTouchGround()) return;
		rig.velocity = Vector3.up * _jumpPower;
	}
}
