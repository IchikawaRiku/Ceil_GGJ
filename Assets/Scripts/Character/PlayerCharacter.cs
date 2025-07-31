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

public class PlayerCharacter : CharacterBase {
	private Rigidbody _rig = null;
	// �W�����v��
	private float _jumpPower = 5f;

	public override async UniTask Initialize() {
		await base.Initialize();
		_rig = GetComponent<Rigidbody>();
	}

	/// <summary>
	/// ���s����
	/// </summary>
	public override async UniTask Execute() {
		await base.Execute();

		moveValue = new Vector3(moveInput.x, 0f, 0f) * moveSpeed * Time.deltaTime;
		transform.position += moveValue;
		// �J�����̈ʒu���Z�b�g
		CameraManager.instance.SetPosition(transform.position);
		// �f�o�b�O�p
		if (Input.GetKeyDown(KeyCode.U)) {
			OnJump();
		}

		prevPos = transform.position;
	}

	/// <summary>
	/// �n�ʐG�ꔻ��
	/// </summary>
	private bool GetTouchGround() {
		if (prevPos.y == transform.position.y) return false;
		return true;
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

	/// <summary>
	/// �W�����v�̓���
	/// </summary>
	private void OnJump() {
		if (GetTouchGround()) return;
		_rig.velocity = Vector3.up * _jumpPower;
	}
}
