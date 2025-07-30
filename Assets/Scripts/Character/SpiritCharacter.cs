using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritCharacter : CharacterBase {
	// スピードの倍率
	private const float _SPEED_LATE = 1.9f;
	// 戻ってくる時の補間比率
	private const float _RETURN_LATE = 0.05f;

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
		Vector3 movePos = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
		transform.Translate(movePos);
	}

	/// <summary>
	/// 元の位置に戻る
	/// </summary>
	public void ReturnPosition() {
		transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, _RETURN_LATE);
	}
}
