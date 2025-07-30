using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBase : MonoBehaviour {
	// �ړ����x
	protected float moveSpeed = 5f;
	// �ړ����x�����̔{��
	protected float speedDownLate = 0.5f;
	// �ړ�����
	protected Vector2 moveInput = Vector2.zero;
	// �ړ����x�̍ő�
	protected const float MOVE_SPEED_MAX = 5f;

	public virtual void Initialize() {

	}

	/// <summary>
	/// ���s����
	/// </summary>
	public virtual void Execute() {
		
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
