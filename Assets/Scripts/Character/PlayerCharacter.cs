/*
 *  @file   PlayerCharacter.cs
 *  @brief  �v���C���[�̃L�����N�^�[
 *  @author Riku
 *  @date   2025/7/29
 */

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

		moveValue = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.position += moveValue;
		// �f�o�b�O�p
		//if (!Input.GetKey(KeyCode.U)) return;
		// �J�����̈ʒu���Z�b�g
		CameraManager.instance.SetPosition(transform.position);
	}

	/// <summary>
	/// �������Ă���Ƃ�
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag(_BULLET_TAG)) {
			Debug.Log("����");
		}
	}

}
