/*
 *  @file   SpiritCharacter.cs
 *  @brief  �H��̃L�����N�^�[
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
	// �X�C�b�`�������邩�ۂ�
	private bool canOnSwitch = false;
	// �X�s�[�h�̔{��
	private const float _SPEED_LATE = 1.9f;
	// �߂��Ă��鎞�̕�Ԕ䗦
	private const float _RETURN_LATE = 0.05f;
	// �ړ���������
	private const float _PLAYER_LEAVE_MAX = 8;
	// �X�C�b�`�̃^�O��
	private const string _SWITCH_TAG = "switch";

	/// <summary>
	/// ������
	/// </summary>
	public override void Initialize() {
		base.Initialize();
		moveSpeed = MOVE_SPEED_MAX * _SPEED_LATE;
	}

	/// <summary>
	/// ���s����
	/// </summary>
	public override void Execute() {
		base.Execute();
		moveValue = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
		// �ړ�����
		LeaveLimit();
		transform.position += moveValue;
	}

	/// <summary>
	/// �ړ�����
	/// </summary>
	private void LeaveLimit() {
		// ���ݒn
		Vector3 position = transform.position;
		// �ړ��\��n
		float movePosX = position.x + moveValue.x;
		float movePosY = position.y + moveValue.y;
		// �v���C���[���痣����鋗��
		float playerLeaveMaxX = CharacterManager.instance.GetPlayerPosition().x + _PLAYER_LEAVE_MAX;
		float playerLeaveMinX = CharacterManager.instance.GetPlayerPosition().x - _PLAYER_LEAVE_MAX;
		float playerLeaveMaxY = CharacterManager.instance.GetPlayerPosition().y + _PLAYER_LEAVE_MAX;
		float playerLeaveMinY = CharacterManager.instance.GetPlayerPosition().y - _PLAYER_LEAVE_MAX;
		// �͈͊O�Ȃ�ړ����Ȃ�
		if (movePosX > playerLeaveMaxX || movePosX < playerLeaveMinX) {
			moveValue.x = 0;
		}
		if (movePosY > playerLeaveMaxY || movePosY < playerLeaveMinY) {
			moveValue.y = 0;
		}
	}

	/// <summary>
	/// ���̈ʒu�ɖ߂�
	/// </summary>
	public void ReturnPosition() {
		transform.position = CharacterManager.instance.GetPlayerPosition();
		//transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, _RETURN_LATE);
	}

	/// <summary>
	/// �������Ă���Ƃ�
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag(_SWITCH_TAG)) {
			canOnSwitch = true;
		}
		if (other.CompareTag(_BULLET_TAG)) {
			Debug.Log("����");
		}
	}

	/// <summary>
	/// ���ꂽ��
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other) {
		if (other.CompareTag(_SWITCH_TAG)) {
			canOnSwitch = false;
		}
	}

	/// <summary>
	/// �X�C�b�`�̓���
	/// </summary>
	/// <param name="context"></param>
	public void OnSwitch(InputAction.CallbackContext context) {
		if (!context.performed || !canOnSwitch) return;
		Debug.Log("�X�C�b�`�I��");
	}
}
