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
            currentButton.onClick.Invoke();
            await UniTask.Delay(200);
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
        if (Mathf.Abs(currentDir.x) <= Mathf.Abs(currentDir.y)) return false;

        return Mathf.Sign(currentDir.x) != 0 &&
            Mathf.Sign(currentDir.x) == Mathf.Sign(prevDir.x);
    }
    /// <summary>
    /// �O��̓��͂�{�^����Ԃ��X�V
    /// </summary>
    /// <param name="inputDir"></param>
    private void UpdateInputState(Vector2 inputDir) {
        // ���͂��V���������Ȃ�X�V
        if (inputDir != _prevInputDir) {
            _prevInputDir = inputDir;
            _isNeutral = false;
        }
        // �{�^�����̍X�V
        prevButton = currentButton;
    }
}
