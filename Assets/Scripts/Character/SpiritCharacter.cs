using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritCharacter : CharacterBase {
	// �X�s�[�h�̔{��
	private const float _SPEED_LATE = 1.9f;
	// �߂��Ă��鎞�̕�Ԕ䗦
	private const float _RETURN_LATE = 0.05f;

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
		Vector3 movePos = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
		transform.Translate(movePos);
	}

	/// <summary>
	/// ���̈ʒu�ɖ߂�
	/// </summary>
	public void ReturnPosition() {
		transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, _RETURN_LATE);
	}
}
