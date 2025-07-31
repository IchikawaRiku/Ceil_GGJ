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

public class AcceptSettingsButtonInput {
    //���͂�ۑ�����ϐ�
    private Vector2 _prevInputDir = Vector2.zero;
    //��x�����ꂽ�����ʂ���t���O
    private bool _isNeutral = false;
    //���݂̃{�^�����
    private Button _currentButton = null;
    //1�t���[���O�̃{�^�����
    private Button _prevButton = null;
    //InputAction
    private DefaultInputActions inputAction = null;

    public void Initialize() {
        inputAction = new DefaultInputActions();
    }
    /// <summary>
    /// �����{�^���̐ݒ�Ɠ��͗L����
    /// </summary>
    /// <param name="setInitButton"></param>
    public void Setup(Button setInitButton) {
        // �ŏ���Select���O�ꂽ�Ƃ��̑΍�
        _prevButton = setInitButton;
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        inputAction.UI.Enable();
    }
    /// <summary>
    /// �{�^���̎�t����
    /// </summary>
    /// <returns></returns>
    public async UniTask AcceptInput() {
        // ���݂̓��͂̒l�̎擾
        Vector2 currentInputDir = inputAction.UI.Navigate.ReadValue<Vector2>();
        // ��x���͂���߂���A�j���[�g������Ԃɂ���
        if (IsInputNeutral(currentInputDir)) return;
        // �I���I�u�W�F�N�g����{�^�������擾
        UpdateCurrentButton();
        // ���E�����݂̂̔���
        bool isSameHorizotal = IsSameHorizotal(currentInputDir, _prevInputDir);
        //���͂����������ŁA����x�����ꂽ��Ȃ�{�^���̏������s
        if (_isNeutral && isSameHorizotal && _currentButton == _prevButton) {
            _currentButton.onClick.Invoke();
            await UniTask.Delay(200);
        }
        //���́A�{�^���̍X�V
        UpdateInputState(currentInputDir);
    }
    public void Teardown() {
        inputAction.UI.Disable();
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
    /// EventSystem�̌��݂̑I���I�u�W�F�N�g����{�^���̏E���A�ݒ�
    /// </summary>
    private void UpdateCurrentButton() {
        // EventSystem�̌��݂̑I���I�u�W�F�N�g���擾
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;
        if (selectObject == null) {
            //EventSystem�̑I���I�u�W�F�N�g��prev�̃I�u�W�F�N�g��ݒ肷��
            EventSystem.current.SetSelectedGameObject(_prevButton.gameObject);
            return;
        }
        //���݂̃{�^���̎擾
        _currentButton = selectObject.GetComponent<Button>();
        if (_currentButton == null && _prevButton != null) {
            //prev��I����Ԃɂ���
            _prevButton.Select();
            _currentButton = _prevButton;
        }
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
        _prevButton = _currentButton;
    }
}
