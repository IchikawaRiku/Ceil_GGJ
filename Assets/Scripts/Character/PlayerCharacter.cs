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

		Vector3 movePos = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.Translate(movePos);
		// カメラの位置をセット
		CameraManager.instance.SetPosition(transform.position);
	}

}
