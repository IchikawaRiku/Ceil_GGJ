/*
 *  @file   AcceptSettingsButtonInput.cs
 *  @brief  �ݒ胁�j���[�{�^���̓��͎�t
 *  @author Seki
 *  @date   2025/7/31
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AcceptSettingsButtonInput : AcceptButtonBase{
    // ���͂�ۑ�����ϐ�
    private Vector2 _prevInputDir = Vector2.zero;
    // ��x�����ꂽ�����ʂ���t���O
    private bool _isNeutral = false;
    //���������̓���
    private int _horizontalDir = 0; 
    //��x�ڂ̓��̓t���O
    private bool _isFirstPress = false; 
    // InputAction
    private MyInput _inputAction = null;

    public AcceptSettingsButtonInput() {
        _inputAction = MyInputManager.inputAction;
    }
    /// <summary>
    /// �����{�^���̐ݒ�Ɠ��͗L����
    /// </summary>
    /// <param name="setInitButton"></param>
    public override async UniTask Setup(Button setInitButton) {
        // �ŏ���Select���O�ꂽ�Ƃ��̑΍�
        prevButton = setInitButton;
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        _inputAction.UI.Enable();
        await UniTask.DelayFrame(1);
    }
    /// <summary>
    /// �{�^���̎�t����
    /// </summary>
    /// <returns></returns>
    public override async UniTask AcceptInput() {
        // ���݂̓��͂̒l�̎擾
        Vector2 currentInputDir = _inputAction.UI.Navigate.ReadValue<Vector2>();
        // ��x���͂���߂���A�j���[�g������Ԃɂ���
        if (IsInputNeutral(currentInputDir)) return;
        // �I���I�u�W�F�N�g����{�^�������擾
        UpdateCurrentButton();
        // ���E�����݂̂̔���
        bool isSameHorizotal = IsSameHorizotal(currentInputDir, _prevInputDir);
        //���͂����������ŁA����x�����ꂽ��Ȃ�{�^���̏������s
        if (_isNeutral && isSameHorizotal && currentButton == prevButton) {
            // ���ڂ̉����͈ȍ~�Ȃ���s
            if (currentButton.CompareTag("SettingVolumeButton")) {
                currentButton.onClick.Invoke();
                UniTask task = SoundManager.instance.PlaySE(0);
                await UniTask.Delay(200);
            }
        }
        //���́A�{�^���̍X�V
        UpdateInputState(currentInputDir);
    }
    public override async UniTask Teardown() {
        _inputAction.UI.Disable();
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// �f�o�C�X���̓[��������
    /// </summary>
    /// <param name="inputDir"></param>
    /// <returns></returns>
    private bool IsInputNeutral(Vector2 inputDir) {
        if (inputDir == Vector2.zero) {
            _isNeutral = true;
            return true;
        }
        return false;
    }
    /// <summary>
    /// ���E�����݂̂̓��͎擾(�����݂̂̎擾�̂���Mathf.Sign���g�p)
    /// </summary>
    /// <param name="currentDir"></param>
    /// <param name="prevDir"></param>
    /// <returns></returns>
    private bool IsSameHorizotal(Vector2 currentDir, Vector2 prevDir) {
        // InputSystem��Vector�͍��E���͎��ɂ��㉺�������o�Ă��܂����Ƃ����������߁A
        // ���E�����̕��������Ƃ��̂ݍ��E�Ƃ��Ĕ���
        if (Mathf.Abs(currentDir.x) <= Mathf.Abs(currentDir.y)) {
            _horizontalDir = 0;
            _isFirstPress = false;
            return false;
        }
        int dir = (int)Mathf.Sign(currentDir.x); // -1 = ��, 1 = �E

        // �������ς�����珉�񖳎��t���O�����Z�b�g
        if (dir != _horizontalDir) {
            _horizontalDir = dir;
            _isFirstPress = false;
            return false;
        }

        // ���������Ȃ珉�񂾂�����
        if (!_isFirstPress) {
            _isFirstPress = true;
            return false;
        }

        // 2��ڈȍ~�� true
        return true;
    }
    /// <summary>
    /// �O��̓��͂�{�^����Ԃ��X�V
    /// </summary>
    /// <param name="inputDir"></param>
    private void UpdateInputState(Vector2 inputDir) {
        // ���͂��V���������Ȃ�X�V
        if (inputDir != _prevInputDir) {
            _prevInputDir = inputDir;
            // �����͂��o���Ƃ�������������
            if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.y)) {
                _isNeutral = false;
            }
        }
        // �{�^�����̍X�V
        if(currentButton != prevButton) {
            UniTask task = SoundManager.instance.PlaySE(0);
            prevButton = currentButton;
        }
    }
}
