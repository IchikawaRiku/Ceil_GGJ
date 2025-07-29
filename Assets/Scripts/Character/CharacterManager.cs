using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour {
	// ���g�ւ̎Q��
	public static CharacterManager instance { get; private set; } = null;
	// ��������I�u�W�F�N�g�̃I���W�i��
	[SerializeField]
	private GameObject _playerOrigin = null;
	[SerializeField]
	private GameObject _spiritOrigin = null;

	// ���������I�u�W�F�N�g
	private GameObject _player = null;
	private GameObject _spirit = null;
	
	// ���쒆�̃L����
	public GameObject controlCharacter = null;

	private void Start () {
		instance = this;
		_player = Instantiate(_playerOrigin);
		_spirit = Instantiate(_spiritOrigin, _player.transform);
		// ��������̓v���C���[
		controlCharacter = _player;
		// �H�̂̓��͂͂Ƃ�Ȃ�
		_spirit.GetComponent<PlayerInput>().enabled = false;
	}

	private void Update () {
		// ����L�����̎��s����
		controlCharacter.GetComponent<CharacterBase>().Execute();
	}

	/// <summary>
	/// ����L�����N�^�[�̃`�F���W
	/// </summary>
	public void ChangeControlCharacter() {
		if (controlCharacter == _player) {
			// �v���C���[�̓��͂�؂�
			_player.GetComponent<PlayerInput>().enabled = false;
			// �R���g���[����H�̂ɂ���
			controlCharacter = _spirit;
			// �H�̂̓��͂��Ƃ�
			_spirit.GetComponent<PlayerInput>().enabled = true;
		}
		else if (controlCharacter == _spirit) {
			// �H�̂̓��͂�؂�
			_spirit.GetComponent<PlayerInput>().enabled = false;
			// �R���g���[�����v���C���[�ɂ���
			controlCharacter = _player;
			// �v���C���[�̓��͂��Ƃ�
			_player.GetComponent<PlayerInput>().enabled = true;
		}
	}
}
