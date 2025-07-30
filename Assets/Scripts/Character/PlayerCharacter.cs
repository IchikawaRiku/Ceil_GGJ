using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : CharacterBase {

	/// <summary>
	/// ���s����
	/// </summary>
	public override void Execute() {
		base.Execute();
		// ����������(��)
		if (Input.GetKey(KeyCode.LeftShift)) {
			moveSpeed = MOVE_SPEED_MAX * speedDownLate;
		}
		else {
			moveSpeed = MOVE_SPEED_MAX;
		}

		Vector3 movePos = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.Translate(movePos);
		// �J�����̈ʒu���Z�b�g
		CameraManager.instance.SetPosition(transform.position);
	}

}
