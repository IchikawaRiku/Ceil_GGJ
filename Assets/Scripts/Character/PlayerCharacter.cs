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
		moveValue = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.position += moveValue;
		// �f�o�b�O�p
		if (Input.GetKeyDown(KeyCode.U)) {
			OnJump();
		}
		if (GetTouchGround()) anim.SetBool("jump", false);
        else anim.SetBool("jump", true);
		Debug.Log(moveInput);
	}

	private void OnDrawGizmos() {
		Vector3 position = transform.position;
		position.y -= _FEET_DISTANCE;
		bool hit = Physics.CheckSphere(position, _FEET_RADIUS, _groundLayer);
		Color hitColor = Color.red;
		Color noHitColor = Color.green;
		Gizmos.color = hit ? hitColor : noHitColor;
		Gizmos.DrawWireSphere(position, _FEET_RADIUS);
	}

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
			Debug.Log("����");
		}
	}

	/// <summary>
	/// �ړ�����
	/// </summary>
	/// <param name="context"></param>
    public override void OnMove(InputAction.CallbackContext context) {
		base.OnMove(context);
        if (context.performed && GetTouchGround()) anim.SetBool("run", true);
        else anim.SetBool("run", false);
	}

	/// <summary>
	/// �W�����v�̓���
	/// </summary>
	private void OnJump() {
		if (!GetTouchGround()) return;
		rig.velocity = Vector3.up * _jumpPower;
	}
}
