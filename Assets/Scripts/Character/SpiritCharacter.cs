/*
 *  @file   SpiritCharacter.cs
 *  @brief  幽霊のキャラクター
 *  @author Riku
 *  @date   2025/7/29
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static MainGameProcessor;

public class SpiritCharacter : CharacterBase {
	[SerializeField]
	private Material material;
	// スイッチを押せるか否か
	private bool canOnSwitch = false;
	// スイッチオンのアニメーション中
	private bool switchAnim = false;
	// フェードイン中
	private bool fadeIn = false;
	// 振り向き移動量
	float angleMoveValue = 0.0f;
	// スピードの倍率
	private const float _SPEED_LATE = 1.9f;
	// 戻ってくる時の補間比率
	private const float _RETURN_LATE = 0.08f;
	// 移動制限距離
	private const float _PLAYER_LEAVE_MAX = 8;
	// プレイヤーと交代する為の距離
	private const float _PLAYER_CHANGE_DISTANCE = 0.35f;
	// スイッチのタグ名
	private const string _SWITCH_TAG = "switch";

	/// <summary>
	/// 初期化
	/// </summary>
	public override async UniTask Initialize() {
		await base.Initialize();
		// 最初は透明にしておく
		Color color = material.color;
		color.a = 0;
		material.color = color;
	}

	/// <summary>
	/// 実行処理
	/// </summary>
	public override async UniTask Execute() {
		await base.Execute();
		// アニメーション中とフェード中はスキップ
		if (switchAnim || fadeIn) return;
		// 幽霊はスピードが速い
		moveSpeed = MOVE_SPEED_MAX * _SPEED_LATE;
		// 向き変更
		ChangeAngle();
		moveValue = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
		// 移動制限
		LeaveLimit();
		transform.position += moveValue;
		if (changeMove) {
			ReturnPosition();
			if (Vector3.Distance(transform.position, CharacterManager.instance.GetPlayerPosition()) < _PLAYER_CHANGE_DISTANCE)
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
		transform.position = Vector3.Lerp(transform.position, CharacterManager.instance.GetPlayerPosition(), _RETURN_LATE);
	}

	/// <summary>
	/// 出現フェード
	/// </summary>
	/// <returns></returns>
	public async UniTask SpritFadeIn() {
		fadeIn = true;
		while (material.color.a < 1) {
			Color color = material.color;
			color.a += 0.05f;
			material.color = color;
			transform.position -= transform.forward * 0.01f;
			transform.position += transform.up * 0.01f;
			await UniTask.Yield();
		}
		fadeIn = false;
	}

	/// <summary>
	/// 消滅フェード
	/// </summary>
	/// <returns></returns>
	public async UniTask SpritFadeOut() {
		Vector3 dir = CharacterManager.instance.GetPlayerPosition() - transform.position;
		while (material.color.a > 0) {
			Color color = material.color;
			color.a -= 0.05f;
			material.color = color;
			transform.position += dir.normalized * 0.02f;
			await UniTask.Yield();
		}
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
	///	幽体離脱の入力
	/// </summary>
	/// <param name="context"></param>
	public override void OnChangeSpirit(InputAction.CallbackContext context) {
		if (fadeIn) return;
		UniTask task = SoundManager.instance.PlaySE(5);
		base.OnChangeSpirit(context);
	}

	/// <summary>
	/// スイッチの入力
	/// </summary>
	/// <param name="context"></param>
	public async void OnSwitch(InputAction.CallbackContext context) {
		if (!canOnSwitch) return;
		switchAnim = true;
		// 振り向くまで待つ
		await TurnToSwitch();
		UniTask task = SoundManager.instance.PlaySE(3);
		anim.SetBool("switch", true);
		SwitchUtility.Press();
	}

	/// <summary>
	/// スイッチを押すために振り向く
	/// </summary>
	private async UniTask TurnToSwitch() {
		Vector3 rotation = transform.eulerAngles;
		// 左向き
		if (charaDir) {
			while (rotation.y <= 360) {
				angleMoveValue += 0.2f;
				rotation.y += angleMoveValue;
				transform.eulerAngles = rotation;
				await UniTask.Yield();
			}
		}
		// 右向き
		else {
			while (rotation.y >= 0) {
				angleMoveValue -= 0.2f;
				rotation.y += angleMoveValue;
				transform.eulerAngles = rotation;
				await UniTask.Yield();
			}
		}
		angleMoveValue = 0.0f;
	}

	/// <summary>
	/// 振り向きを戻す
	/// </summary>
	/// <returns></returns>
	private async UniTask TurnBack() {
		Vector3 rotation = transform.eulerAngles;
		// 左向き
		if (charaDir) {
			rotation.y = 359;
			while (rotation.y >= 270) {
				angleMoveValue -= 0.2f;
				rotation.y += angleMoveValue;
				transform.eulerAngles = rotation;
				await UniTask.DelayFrame(1);
			}
		}
		// 右向き
		else {
			rotation.y = 0;
			while (rotation.y <= 90) {
				angleMoveValue += 0.2f;
				rotation.y += angleMoveValue;
				transform.eulerAngles = rotation;
				await UniTask.DelayFrame(1);
			}
		}
		angleMoveValue = 0.0f;
		switchAnim = false;
	}
	/// <summary>
	/// スイッチアニメーションの終わり
	/// </summary>
	public void SwitchAnimationEnd() {
		anim.SetBool("switch", false);
		UniTask task = TurnBack();
	}

}
