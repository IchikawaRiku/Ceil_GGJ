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

public class PlayerCharacter : CharacterBase {
	private Rigidbody _rig = null;
	// ジャンプ力
	private float _jumpPower = 5f;
	// プレイヤーの座標から足元座標までの距離
	private const float _FEET_DISTANCE = 0.5f;
	// 地面判定用の半径
	private const float _FEET_RADIUS = 0.2f;

	public override async UniTask Initialize() {
		await base.Initialize();
		_rig = GetComponent<Rigidbody>();
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
	}

	/// <summary>
	/// 地面触れ判定
	/// </summary>
	private bool GetTouchGround(Vector3 position) {
		position.y -= _FEET_DISTANCE;
		return Physics.CheckSphere(position, _FEET_RADIUS);		
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

	/// <summary>
	/// ジャンプの入力
	/// </summary>
	private void OnJump() {
		if (GetTouchGround(transform.position)) return;
		_rig.velocity = Vector3.up * _jumpPower;
	}
}
