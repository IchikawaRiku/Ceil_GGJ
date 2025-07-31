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

		prevPos = transform.position;
	}

	/// <summary>
	/// 地面触れ判定
	/// </summary>
	private bool GetTouchGround() {
		if (prevPos.y == transform.position.y) return false;
		return true;
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
		if (GetTouchGround()) return;
		_rig.velocity = Vector3.up * _jumpPower;
	}
}
