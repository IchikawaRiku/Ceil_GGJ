/*
 *  @file   CharacterManager.cs
 *  @brief  �L�����N�^�[�̊Ǘ��N���X
 *  @author Riku
 *  @date   2025/7/29
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    public CharacterBase controlCharacter { get; private set; } = null;

    // InputSystem
    //private PlayerInput _playerInput;
    //private PlayerInput _spiritInput;

    public async UniTask Initialize() {
        instance = this;
        //�v���C���[�̐���
        _unusePlayer = Instantiate(_playerOrigin, _unuseRoot);
        _unuseSpirit = Instantiate(_spiritOrigin, _unuseRoot);
        await _unuseSpirit.Initialize();
        await _unusePlayer.Initialize();
        
		await UniTask.CompletedTask;
    }

    /// <summary>
    /// ���s����
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        if (controlCharacter == null) return;
		// ����L�����̎��s����
		await controlCharacter.Execute();
        if (controlCharacter != _useSpirit) {
            _unuseSpirit.ReturnPosition();
        }
		await UniTask.CompletedTask;
    }

    /// <summary>
    /// ����L�����N�^�[�̃`�F���W
    /// </summary>
    public async UniTask ChangeControlCharacter() {
        if (controlCharacter == _usePlayer) {
            //�H��𐶐�
            UseSpirit();
            // �v���C���[�̓��͂�؂�
            controlCharacter.DisableInput();
			// �R���g���[����H��ɂ���
			controlCharacter = _useSpirit;
            // �H��̓��͂��Ƃ�
            controlCharacter.EnableInput();
			// �A�j���[�V�����Đ�
			_usePlayer.anim.SetBool("change", true);

		}
        else if (controlCharacter == _useSpirit) {
            //�H��𖢎g�p��
            UnuseSpirit();
            // �H��̓��͂�؂�
            controlCharacter.DisableInput();
			// �R���g���[�����v���C���[�ɂ���
			controlCharacter = _usePlayer;
			// �v���C���[�̓��͂��Ƃ�
			controlCharacter.EnableInput();
            // �A�j���[�V�����I��
            _usePlayer.anim.SetBool("change", false);
		}
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// �v���C���[�̐���
    /// </summary>
    public void UsePlayer() {
        _usePlayer = _unusePlayer;
        _unusePlayer = null;
        _usePlayer.transform.SetParent(_useRoot);
        if (controlCharacter == _usePlayer) return;
		// �R���g���[�����v���C���[�ɂ���
		controlCharacter = _usePlayer;
	}
    /// <summary>
    /// �v���C���[�̖��g�p��
    /// </summary>
    public void UnusePlayer() {
		if (_usePlayer == null) return;
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
        if (_useSpirit == null) return;
        _unuseSpirit = _useSpirit;
        _useSpirit = null;
        _unuseSpirit.transform.SetParent(_unuseRoot);
	}

    /// <summary>
    /// �v���C���[�̈ʒu�擾
    /// </summary>
    public Vector3 GetPlayerPosition() {
        if (_usePlayer == null) return Vector3.zero;
        Vector3 position = _usePlayer.transform.position;
        return position;
    }

    /// <summary>
    /// �v���C���[�̈ʒu�̃Z�b�g
    /// </summary>
    /// <param name="positoin"></param>
    public void SetPlayerPosition(Vector3 positoin) {
        _usePlayer.transform.position = positoin;
    }

    /// <summary>
    /// �H��̈ʒu�擾
    /// </summary>
    public Vector3 GetSpiritPosition() {
        if (_useSpirit == null) return Vector3.zero;
        Vector3 position = _useSpirit.transform.position;
        return position;
    }
    
    /// <summary>
    /// �Еt��
    /// </summary>
    public void Teardown() {
        _usePlayer.Teardown();
        if (_useSpirit == null) _unuseSpirit.Teardown();
        else _useSpirit.Teardown();
        UnusePlayer();
        UnuseSpirit();
        
    }

}
