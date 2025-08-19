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

using static MainGameProcessor;

public class PlayerCharacter : CharacterBase {
	//Animator animator;

	// オブジェクトのレイヤー
	private LayerMask _objectLayer;
	// ジャンプ力
	private float _jumpPower = 5f;
	// 地面判定用の半径
	private const float _FEET_RADIUS = 0.05f;
	// 壁判定用の箱サイズ
	private readonly Vector3 _WALL_SIZE = new Vector3(0.1f, 0.5f, 1);

	public override async UniTask Initialize() {
		await base.Initialize();
		_objectLayer = LayerMask.GetMask("Jump");
		//animator = GetComponent<Animator>();
	}

	/// <summary>
	/// 実行処理
	/// </summary>
	public override async UniTask Execute() {
		await base.Execute();
		// 向き変更
		if (!anim.GetBool("change")) ChangeAngle();
		// 移動処理
		moveValue = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.position += moveValue;
		if (moveValue.x != 0 && GetTouchGround()) {
			anim.SetBool("run", true);
			// 壁当たり判定
			if (GetTouchWall()) {
				anim.SetBool("push", true);
			}
		}
		else {
			anim.SetBool("run", false);
			anim.SetBool("push", false);
		}
		// ジャンプアニメーション
		if (GetTouchGround()) anim.SetBool("jump", false);
        else anim.SetBool("jump", true);
	}
	
	private void OnDrawGizmos() {
		Vector3 position = transform.position;
		if (transform.eulerAngles.y == 90) position.x += 0.4f;
		else position.x -= 0.4f;
		position.y += 0.7f;
		bool hit = Physics.CheckBox(position, _WALL_SIZE, Quaternion.identity, _objectLayer);
		Color hitColor = Color.red;
		Color noHitColor = Color.green;
		Gizmos.color = hit ? hitColor : noHitColor;
		Gizmos.DrawWireCube(position, _WALL_SIZE);
	}

	/// <summary>
	/// 地面触れ判定
	/// </summary>
	private bool GetTouchGround() {
		Vector3 position = transform.position;
		return Physics.CheckSphere(position, _FEET_RADIUS, _objectLayer);		
	}

	/// <summary>
	/// 壁触れ判定
	/// </summary>
	private bool GetTouchWall() {
		Vector3 position = transform.position;
		if (transform.eulerAngles.y == 90) position.x += 0.4f;
		else position.x -= 0.4f;
		position.y += 0.7f;
		return Physics.CheckBox(position, _WALL_SIZE, Quaternion.identity, _objectLayer);
	}

	/// <summary>
	/// 当たっているとき
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
        if (GetGameReason() != eEndReason.Invalid) return;
        if (other.CompareTag(BULLET_TAG)) {
            UniTask task = SoundManager.instance.PlaySE(8);
            anim.Play("boy_down_back");
			EndGameReason(eEndReason.Dead);
			DisableInput();
		}
	}

	/// <summary>
	/// Inputのアクティブ化
	/// </summary>
	public override void EnableInput() {
		base.EnableInput();
		action = input.actions["Jump"];
		action.started += OnJump;
		action.Enable();
	}

	/// <summary>
	/// Inputの非アクティブ化
	/// </summary>
	public override void DisableInput() {
		base.DisableInput();
		action = input.actions["Jump"];
		action.started -= OnJump;
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
    /// 幽体離脱の入力
    /// </summary>
    /// <param name="context"></param>
    public override void OnChangeSpirit(InputAction.CallbackContext context) {
        UniTask task = SoundManager.instance.PlaySE(5);
        base.OnChangeSpirit(context);
    }

    /// <summary>
    /// ジャンプの入力
    /// </summary>
    public void OnJump(InputAction.CallbackContext context) {
		if (!GetTouchGround()) return;
        UniTask task = SoundManager.instance.PlaySE(9);
        rig.velocity = Vector3.up * _jumpPower;
	}

    /// <summary>
    /// 幽体離脱のアニメーションの終わり
    /// </summary>
    public void ChangeAnimationEnd() {
        changeMove = false;
    }
}
