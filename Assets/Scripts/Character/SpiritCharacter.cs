/*
 *  @file   SpiritCharacter.cs
 *  @brief  幽霊のキャラクター
 *  @author Riku
 *  @date   2025/7/29
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SpiritCharacter : CharacterBase {
	// スイッチを押せるか否か
	private bool canOnSwitch = false;
	// スピードの倍率
	private const float _SPEED_LATE = 1.9f;
	// 戻ってくる時の補間比率
	private const float _RETURN_LATE = 0.05f;
	// 移動制限距離
	private const float _PLAYER_LEAVE_MAX = 8;
	// スイッチのタグ名
	private const string _SWITCH_TAG = "switch";

	/// <summary>
	/// 初期化
	/// </summary>
	public override void Initialize() {
		base.Initialize();
		moveSpeed = MOVE_SPEED_MAX * _SPEED_LATE;
	}

	/// <summary>
	/// 実行処理
	/// </summary>
	public override void Execute() {
		base.Execute();
		moveValue = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
		// 移動制限
		LeaveLimit();
		transform.position += moveValue;
	}

	/// <summary>
	/// 移動制限
	/// </summary>
	private void LeaveLimit() {
		// 現在地
		Vector3 position = transform.position;
		// 移動予定地
		float movePosX = position.x + moveValue.x;
		float movePosY = position.y + moveValue.y;
		// プレイヤーから離れられる距離
		float playerLeaveMaxX = CharacterManager.instance.GetPlayerPosition().x + _PLAYER_LEAVE_MAX;
		float playerLeaveMinX = CharacterManager.instance.GetPlayerPosition().x - _PLAYER_LEAVE_MAX;
		float playerLeaveMaxY = CharacterManager.instance.GetPlayerPosition().y + _PLAYER_LEAVE_MAX;
		float playerLeaveMinY = CharacterManager.instance.GetPlayerPosition().y - _PLAYER_LEAVE_MAX;
		// 範囲外なら移動しない
		if (movePosX > playerLeaveMaxX || movePosX < playerLeaveMinX) {
			moveValue.x = 0;
		}
		if (movePosY > playerLeaveMaxY || movePosY < playerLeaveMinY) {
			moveValue.y = 0;
		}
	}

	/// <summary>
	/// 元の位置に戻る
	/// </summary>
	public void ReturnPosition() {
		transform.position = CharacterManager.instance.GetPlayerPosition();
		//transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, _RETURN_LATE);
	}

	/// <summary>
	/// 当たっているとき
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag(_SWITCH_TAG)) {
			canOnSwitch = true;
		}
		if (other.CompareTag(_BULLET_TAG)) {
			Debug.Log("死んだ");
		}
	}

	/// <summary>
	/// 離れた時
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other) {
		if (other.CompareTag(_SWITCH_TAG)) {
			canOnSwitch = false;
		}
	}

	/// <summary>
	/// スイッチの入力
	/// </summary>
	/// <param name="context"></param>
	public void OnSwitch(InputAction.CallbackContext context) {
		if (!context.performed || !canOnSwitch) return;
		Debug.Log("スイッチオン");
	}
}
