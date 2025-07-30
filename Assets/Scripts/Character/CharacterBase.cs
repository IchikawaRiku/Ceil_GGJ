using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBase : MonoBehaviour {
	// ˆÚ“®‘¬“x
	protected float moveSpeed = 5f;
	// ˆÚ“®‘¬“xŒ¸­‚Ì”{—¦
	protected float speedDownLate = 0.5f;
	// ˆÚ“®“ü—Í
	protected Vector2 moveInput = Vector2.zero;
	// ˆÚ“®‘¬“x‚ÌÅ‘å
	protected const float MOVE_SPEED_MAX = 5f;

	public virtual void Initialize() {

	}

	/// <summary>
	/// Àsˆ—
	/// </summary>
	public virtual void Execute() {
		
	}

	/// <summary>
	/// ˆÚ“®‚Ì“ü—Í
	/// </summary>
	/// <param name="context"></param>
	public void OnMove(InputAction.CallbackContext context) {
		moveInput = context.ReadValue<Vector2>();
	}

	/// <summary>
	/// —H‘Ì—£’E‚Ì“ü—Í
	/// </summary>
	/// <param name="context"></param>
	public void OnChangeSpirit(InputAction.CallbackContext context) {
		if (!context.performed) return;
		CharacterManager.instance.ChangeControlCharacter();
	}
}
