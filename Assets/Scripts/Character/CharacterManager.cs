using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour {
	// ���g�ւ̎Q��
	public static CharacterManager instance { get; private set; } = null;
	[SerializeField]
	private Transform _useRoot = null;
	[SerializeField]
	private Transform _unuseRoot = null;
	// ��������I�u�W�F�N�g�̃I���W�i��
	[SerializeField]
	private PlayerCharacter _playerOrigin = null;
	[SerializeField]
	private SpiritCharacter _spiritOrigin = null;

	// ���������I�u�W�F�N�g
	private PlayerCharacter _usePlayer = null;
	private SpiritCharacter _useSpirit = null;
	//���g�p��Ԃ̃v���C���[
	private PlayerCharacter _unusePlayer = null;
	//���g�p��Ԃ̗H��
	private SpiritCharacter _unuseSpirit = null;
	// ���쒆�̃L����
	public CharacterBase controlCharacter = null;

	private void Start () {
		instance = this;
		//�v���C���[�̐���
		_unusePlayer = Instantiate(_playerOrigin, _unuseRoot);
		_unuseSpirit = Instantiate(_spiritOrigin, _unuseRoot);
		// ��������̓v���C���[
		controlCharacter = _usePlayer;
		// �H�̂̓��͂͂Ƃ�Ȃ�
		_unuseSpirit.enabled = false;
		_unuseSpirit.Initialize();
	}

	private void Update () {
		// ����L�����̎��s����
		controlCharacter.Execute();
		if (controlCharacter != _useSpirit) {
			_unuseSpirit.ReturnPosition();
		}
	}

	/// <summary>
	/// ����L�����N�^�[�̃`�F���W
	/// </summary>
	public void ChangeControlCharacter() {
		if (controlCharacter == _usePlayer) {
			//�H��𐶐�
			UseSpirit();
			// �R���g���[����H�̂ɂ���
			controlCharacter = _useSpirit;
			// �v���C���[�̓��͂�؂�
			_usePlayer.enabled = false;
			// �H�̂̓��͂��Ƃ�
			_useSpirit.enabled = true;
		}
		else if (controlCharacter == _useSpirit) {
			//�H��𖢎g�p��
			UnuseSpirit();
			// �R���g���[�����v���C���[�ɂ���
			controlCharacter = _usePlayer;
			// �H�̂̓��͂�؂�
			_unuseSpirit.enabled = false;
			// �v���C���[�̓��͂��Ƃ�
			_usePlayer.enabled = true;
		}
	}
	/// <summary>
	/// �v���C���[�̐���
	/// </summary>
	public void UsePlayer() {
		_usePlayer = _unusePlayer;
		_unusePlayer = null;
		_usePlayer.transform.SetParent(_useRoot);
	}
	/// <summary>
	/// �v���C���[�̖��g�p��
	/// </summary>
	public void UnusePlayer() {
		_unusePlayer = _usePlayer;
		_usePlayer = null;
		_unusePlayer.transform.SetParent(_unuseRoot);
	}
	/// <summary>
	/// �H��̐���
	/// </summary>
	public void UseSpirit() {
		_useSpirit = _unuseSpirit;
		_unuseSpirit = null;
		_useSpirit.transform.SetParent(_useRoot);
	}
	/// <summary>
	/// �H��̖��g�p��
	/// </summary>
	public void UnuseSpirit() {
		_unuseSpirit = _useSpirit;
		_useSpirit = null;
		_unuseSpirit.transform.SetParent(_unuseRoot);
	}
}
