/*
 *  @file   CharacterBase.cs
 *  @brief  キャラクターの基底クラス
 *  @author Riku
 *  @date   2025/7/29
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBase : MonoBehaviour { 
	protected Rigidbody rig = null;
	[SerializeField]
	protected Animator anim = null;
	// 移動速度
	[SerializeField]
	protected float moveSpeed = 5f;
	// 移動速度減少の倍率
	protected float speedDownLate = 0.5f;
	[SerializeField]
	// 移動入力
	protected Vector2 moveInput = Vector2.zero;
	[SerializeField]
	// 最終的な移動量
	protected Vector3 moveValue = Vector3.zero;
	// 移動速度の最大
	protected const float MOVE_SPEED_MAX = 5f;
	// 進行方向の向き
	private const float _DIRECTION_ANGLE = 90;
    // ステージギミックの弾のタグ
    protected const string _BULLET_TAG = "bullet";

	/// <summary>
	/// 初期化
	/// </summary>
	public virtual async UniTask Initialize() {
		rig = GetComponent<Rigidbody>();
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// 実行処理
	/// </summary>
	public virtual async UniTask Execute() {
		// カメラの位置をセット
		CameraManager.instance.SetPosition(transform.position);
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// 移動の入力
	/// </summary>
	/// <param name="context"></param>
	public virtual void OnMove(InputAction.CallbackContext context) {
		moveInput = context.ReadValue<Vector2>();
		Vector3 rotation = transform.eulerAngles;
        if (moveInput.x > 0) rotation.y = _DIRECTION_ANGLE;
        else if (moveInput.x < 0) rotation.y = -_DIRECTION_ANGLE;
		transform.eulerAngles = rotation;
	}

	/// <summary>
	/// 幽体離脱の入力
	/// </summary>
	/// <param name="context"></param>
	public void OnChangeSpirit(InputAction.CallbackContext context) {
		if (!context.performed) return;
 		CharacterManager.instance.ChangeControlCharacter();
    }

	/// <summary>
	/// 片付け
	/// </summary>
	public void Teardown() {
		moveSpeed = MOVE_SPEED_MAX;
		rig.velocity = Vector3.zero;
		moveInput = Vector2.zero;
		moveValue = Vector3.zero;
	}

}
