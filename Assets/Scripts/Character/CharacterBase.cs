/*
 *  @file   CharacterBase.cs
 *  @brief  �L�����N�^�[�̊��N���X
 *  @author Riku
 *  @date   2025/7/29
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBase : MonoBehaviour {
	// �ړ����x
	[SerializeField]
	protected float moveSpeed = 5f;
	// �ړ����x�����̔{��
	protected float speedDownLate = 0.5f;
	[SerializeField]
	// �ړ�����
	protected Vector2 moveInput = Vector2.zero;
	[SerializeField]
	// �ŏI�I�Ȉړ���
	protected Vector3 moveValue = Vector3.zero;
	// �ړ����x�̍ő�
	protected const float MOVE_SPEED_MAX = 5f;
	// �X�e�[�W�M�~�b�N�̒e�̃^�O
	protected const string _BULLET_TAG = "bullet";

	/// <summary>
	/// ������
	/// </summary>
	public virtual async UniTask Initialize() {
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// ���s����
	/// </summary>
	public virtual async UniTask Execute() {
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// �ړ��̓���
	/// </summary>
	/// <param name="context"></param>
	public void OnMove(InputAction.CallbackContext context) {
		moveInput = context.ReadValue<Vector2>();
	}

	/// <summary>
	/// �H�̗��E�̓���
	/// </summary>
	/// <param name="context"></param>
	public void OnChangeSpirit(InputAction.CallbackContext context) {
		if (!context.performed) return;
 		CharacterManager.instance.ChangeControlCharacter();
	}

}
