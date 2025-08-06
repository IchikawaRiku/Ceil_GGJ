/*
 *  @file   PlayerCharacter.cs
 *  @brief  �v���C���[�̃L�����N�^�[
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

	// �n�ʂ̃��C���[
	private LayerMask _groundLayer;
	// �W�����v��
	private float _jumpPower = 5f;
	// �v���C���[�̍��W���瑫�����W�܂ł̋���
	private const float _FEET_DISTANCE = 0f;
	// �n�ʔ���p�̔��a
	private const float _FEET_RADIUS = 0.05f;

	public override async UniTask Initialize() {
		await base.Initialize();
		_groundLayer = LayerMask.GetMask("Jump");
		//animator = GetComponent<Animator>();
	}

	/// <summary>
	/// ���s����
	/// </summary>
	public override async UniTask Execute() {
		await base.Execute();
		// �����ύX
		if (!anim.GetBool("change")) ChangeAngle();
		// �ړ�����
		moveValue = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.position += moveValue;
		if (moveValue.x != 0 && GetTouchGround()) anim.SetBool("run", true);
		else anim.SetBool("run", false);
		// �W�����v�A�j���[�V����
		if (GetTouchGround()) anim.SetBool("jump", false);
        else anim.SetBool("jump", true);
	}

	/*
	private void OnDrawGizmos() {
		Vector3 position = transform.position;
		position.y -= _FEET_DISTANCE;
		bool hit = Physics.CheckSphere(position, _FEET_RADIUS, _groundLayer);
		Color hitColor = Color.red;
		Color noHitColor = Color.green;
		Gizmos.color = hit ? hitColor : noHitColor;
		Gizmos.DrawWireSphere(position, _FEET_RADIUS);
	}
	*/

	/// <summary>
	/// �n�ʐG�ꔻ��
	/// </summary>
	private bool GetTouchGround() {
		Vector3 position = transform.position;
        position.y -= _FEET_DISTANCE;
		return Physics.CheckSphere(position, _FEET_RADIUS, _groundLayer);		
	}

	/// <summary>
	/// �������Ă���Ƃ�
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag(BULLET_TAG)) {
			anim.Play("boy_down_back");
			EndGameReason(eEndReason.Dead);
			DisableInput();
		}
	}

	/// <summary>
	/// Input�̃A�N�e�B�u��
	/// </summary>
	public override void EnableInput() {
		base.EnableInput();
		action = input.actions["Jump"];
		action.started += OnJump;
		action.Enable();
	}

	/// <summary>
	/// Input�̔�A�N�e�B�u��
	/// </summary>
	public override void DisableInput() {
		base.DisableInput();
		action = input.actions["Jump"];
		action.started -= OnJump;
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
	/// �W�����v�̓���
	/// </summary>
	public void OnJump(InputAction.CallbackContext context) {
		if (!GetTouchGround()) return;
		rig.velocity = Vector3.up * _jumpPower;
	}

    /// <summary>
    /// �H�̗��E�̃A�j���[�V�����̏I���
    /// </summary>
    public void ChangeAnimationEnd() {
        changeMove = false;
    }
}
