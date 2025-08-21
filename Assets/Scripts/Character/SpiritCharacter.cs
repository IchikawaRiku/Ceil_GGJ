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

using static MainGameProcessor;

public class SpiritCharacter : CharacterBase {
	// スイッチを押せるか否か
	private bool canOnSwitch = false;
	// スイッチオンのアニメーション中
	private bool switchAnim = false;
	// スピードの倍率
	private const float _SPEED_LATE = 1.9f;
	// 戻ってくる時の補間比率
	private const float _RETURN_LATE = 0.08f;
	// 移動制限距離
	private const float _PLAYER_LEAVE_MAX = 8;
	// プレイヤーと交代する為の距離
	private const float _PLAYER_CHANGE_DISTANCE = 0.1f;
	// スイッチのタグ名
	private const string _SWITCH_TAG = "switch";

	/// <summary>
	/// 初期化
	/// </summary>
	public override async UniTask Initialize() {
		await base.Initialize();
		moveSpeed = MOVE_SPEED_MAX * _SPEED_LATE;
	}

	/// <summary>
	/// 実行処理
	/// </summary>
	public override async UniTask Execute() {
		await base.Execute();
		// アニメーション中はスキップ
		if (switchAnim) return;
		// 向き変更
		ChangeAngle();
		moveValue = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
		// 移動制限
		LeaveLimit();
		transform.position += moveValue;
		if (changeMove) {
			ReturnPosition();
			if (Vector3.Distance(transform.position, CharacterManager.instance.GetPlayerPosition())
				< _PLAYER_CHANGE_DISTANCE) 
				changeMove = false;
		}
	}

	/// <summary>
	/// 移動制限
	/// </summary>
	private void LeaveLimit() {
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
		if ((movePosX > playerLeaveMaxX && moveValue.x > 0)
			|| (movePosX < playerLeaveMinX && moveValue.x < 0))
			moveValue.x = 0;

		if ((movePosY > playerLeaveMaxY && moveValue.y > 0)
			|| (movePosY < playerLeaveMinY && moveValue.y < 0))
			moveValue.y = 0;
	}

	/// <summary>
	/// 元の位置に戻る
	/// </summary>
	public void ReturnPosition() {
		//transform.position = CharacterManager.instance.GetPlayerPosition();
		transform.position = Vector3.Lerp(transform.position, CharacterManager.instance.GetPlayerPosition(), _RETURN_LATE);
	}

	/// <summary>
	/// 当たっているとき
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (GetGameReason() != eEndReason.Invalid) return;
		if (other.CompareTag(_SWITCH_TAG)) canOnSwitch = true;
		if (other.CompareTag(BULLET_TAG) && !changeMove) {
            UniTask task = SoundManager.instance.PlaySE(8);
            anim.Play("ghost_dissolve");
			EndGameReason(eEndReason.Dead);
			DisableInput();
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
	/// Inputのアクティブ化
	/// </summary>
	public override void EnableInput() {
		base.EnableInput();
		action = input.actions["SwitchOn"];
		action.started += OnSwitch;
		action.Enable();
	}

	/// <summary>
	/// Inputの非アクティブ化
	/// </summary>
	public override void DisableInput() {
		base.DisableInput();
		action = input.actions["SwitchOn"];
		action.started -= OnSwitch;
		action.Disable();
	}

	/// <summary>
	/// 移動入力
	/// </summary>
	/// <param name="context"></param>
	public override void OnMove(InputAction.CallbackContext context) {
		base.OnMove(context);
	}

    /// <summary>
    ///	移動入力
    /// </summary>
    /// <param name="context"></param>
    public override void OnChangeSpirit(InputAction.CallbackContext context) {
        UniTask task = SoundManager.instance.PlaySE(5);
        base.OnChangeSpirit(context);
	}

	/// <summary>
	/// スイッチの入力
	/// </summary>
	/// <param name="context"></param>
	public async void OnSwitch(InputAction.CallbackContext context) {
		if (!canOnSwitch) return;
		// 振り向くまで待つ
		await TurnToSwitch();
        UniTask task = SoundManager.instance.PlaySE(3);
		anim.SetBool("switch", true);
		switchAnim = true;
		SwitchUtility.Press();
	}

	/// <summary>
	/// スイッチを押すために振り向く
	/// </summary>
	private async UniTask TurnToSwitch() {
		Vector3 rotation = transform.eulerAngles;
		while (rotation.y > 0.1f || rotation.y < -0.1f) {
			rotation.y *= 0.8f;
			transform.eulerAngles = rotation;
			await UniTask.DelayFrame(1);
		}
		rotation.y = 0;
		transform.eulerAngles = rotation;
	}

	/// <summary>
	/// スイッチアニメーションの終わり
	/// </summary>
	public void SwitchAnimationEnd() {
		switchAnim = false;
		anim.SetBool("switch", false);
	}
}
