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

using static MainGameProcessor;

public class SpiritCharacter : CharacterBase {
	// �X�C�b�`�������邩�ۂ�
	private bool canOnSwitch = false;
	// �X�C�b�`�I���̃A�j���[�V������
	private bool switchAnim = false;
	// �X�s�[�h�̔{��
	private const float _SPEED_LATE = 1.9f;
	// �߂��Ă��鎞�̕�Ԕ䗦
	private const float _RETURN_LATE = 0.08f;
	// �ړ���������
	private const float _PLAYER_LEAVE_MAX = 8;
	// �v���C���[�ƌ�シ��ׂ̋���
	private const float _PLAYER_CHANGE_DISTANCE = 0.1f;
	// �X�C�b�`�̃^�O��
	private const string _SWITCH_TAG = "switch";

	/// <summary>
	/// ������
	/// </summary>
	public override async UniTask Initialize() {
		await base.Initialize();
		moveSpeed = MOVE_SPEED_MAX * _SPEED_LATE;
	}

	/// <summary>
	/// ���s����
	/// </summary>
	public override async UniTask Execute() {
		await base.Execute();
		// �A�j���[�V�������̓X�L�b�v
		if (switchAnim) return;
		// �����ύX
		ChangeAngle();
		moveValue = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
		// �ړ�����
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
	/// �ړ�����
	/// </summary>
	private void LeaveLimit() {
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
		if ((movePosX > playerLeaveMaxX && moveValue.x > 0)
			|| (movePosX < playerLeaveMinX && moveValue.x < 0))
			moveValue.x = 0;

		if ((movePosY > playerLeaveMaxY && moveValue.y > 0)
			|| (movePosY < playerLeaveMinY && moveValue.y < 0))
			moveValue.y = 0;
	}

	/// <summary>
	/// ���̈ʒu�ɖ߂�
	/// </summary>
	public void ReturnPosition() {
		//transform.position = CharacterManager.instance.GetPlayerPosition();
		transform.position = Vector3.Lerp(transform.position, CharacterManager.instance.GetPlayerPosition(), _RETURN_LATE);
	}

	/// <summary>
	/// �������Ă���Ƃ�
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
	/// ���ꂽ��
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other) {
		if (other.CompareTag(_SWITCH_TAG)) {
			canOnSwitch = false;
		}
	}

	/// <summary>
	/// Input�̃A�N�e�B�u��
	/// </summary>
	public override void EnableInput() {
		base.EnableInput();
		action = input.actions["SwitchOn"];
		action.started += OnSwitch;
		action.Enable();
	}

	/// <summary>
	/// Input�̔�A�N�e�B�u��
	/// </summary>
	public override void DisableInput() {
		base.DisableInput();
		action = input.actions["SwitchOn"];
		action.started -= OnSwitch;
		action.Disable();
	}

	/// <summary>
	/// �ړ�����
	/// </summary>
	/// <param name="context"></param>
	public override void OnMove(InputAction.CallbackContext context) {
		base.OnMove(context);
	}

    /// <summary>
    ///	�ړ�����
    /// </summary>
    /// <param name="context"></param>
    public override void OnChangeSpirit(InputAction.CallbackContext context) {
        UniTask task = SoundManager.instance.PlaySE(5);
        base.OnChangeSpirit(context);
	}

	/// <summary>
	/// �X�C�b�`�̓���
	/// </summary>
	/// <param name="context"></param>
	public async void OnSwitch(InputAction.CallbackContext context) {
		if (!canOnSwitch) return;
		// �U������܂ő҂�
		await TurnToSwitch();
        UniTask task = SoundManager.instance.PlaySE(3);
		anim.SetBool("switch", true);
		switchAnim = true;
		SwitchUtility.Press();
	}

	/// <summary>
	/// �X�C�b�`���������߂ɐU�����
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
	/// �X�C�b�`�A�j���[�V�����̏I���
	/// </summary>
	public void SwitchAnimationEnd() {
		switchAnim = false;
		anim.SetBool("switch", false);
	}
}
