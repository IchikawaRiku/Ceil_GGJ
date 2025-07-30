/*
 *  @file   PlayerCharacter.cs
 *  @brief  プレイヤーのキャラクター
 *  @author Riku
 *  @date   2025/7/29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : CharacterBase {

	/// <summary>
	/// 実行処理
	/// </summary>
	public override void Execute() {
		base.Execute();
		// 箱押し判定(仮)
		if (Input.GetKey(KeyCode.LeftShift)) {
			moveSpeed = MOVE_SPEED_MAX * speedDownLate;
		}
		else {
			moveSpeed = MOVE_SPEED_MAX;
		}

		moveValue = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.position += moveValue;
		// デバッグ用
		//if (!Input.GetKey(KeyCode.U)) return;
		// カメラの位置をセット
		CameraManager.instance.SetPosition(transform.position);
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

}
