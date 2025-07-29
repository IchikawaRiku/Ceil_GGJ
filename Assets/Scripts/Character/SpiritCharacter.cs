using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritCharacter : CharacterBase {
	public override void Execute() {
		base.Execute();
		Vector3 movePos = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
		transform.Translate(movePos);
	}
}
