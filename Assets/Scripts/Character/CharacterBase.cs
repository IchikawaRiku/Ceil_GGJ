/*
 *  @file   CharacterBase.cs
 *  @brief  キャラクターの基底クラス
 *  @author Riku
 *  @date   2025/7/29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBase : MonoBehaviour {
	// 移動速度
	protected float moveSpeed = 5f;
	// 移動速度減少の倍率
	protected float speedDownLate = 0.5f;
	// 移動入力
	protected Vector2 moveInput = Vector2.zero;
	// 移動速度の最大
	protected const float MOVE_SPEED_MAX = 5f;

	/// <summary>
	/// 初期化
	/// </summary>
	public virtual void Initialize() {

	}

	/// <summary>
	/// 実行処理
	/// </summary>
	public virtual void Execute() {
		
	}

	/// <summary>
	/// 移動の入力
	/// </summary>
	/// <param name="context"></param>
	public void OnMove(InputAction.CallbackContext context) {
		moveInput = context.ReadValue<Vector2>();
	}

	/// <summary>
	/// 幽体離脱の入力
	/// </summary>
	/// <param name="context"></param>
	public void OnChangeSpirit(InputAction.CallbackContext context) {
		if (!context.performed) return;
		CharacterManager.instance.ChangeControlCharacter();
	}
}
