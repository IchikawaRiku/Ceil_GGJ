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
    /// ��������
    /// </summary>
    /// <param name="setButton"></param>
    public void Setup(Button setButton) {
        // UI���\�����ꂽ�Ƃ��A�ŏ��ɐݒ肳���{�^�����󂯎���Ă���
        EventSystem.current.SetSelectedGameObject(setButton.gameObject);
        // �ŏ���Select���O�ꂽ�Ƃ��̑΍�Ƃ��āAprev�Ɏ󂯎�����{�^�����ŏ��ɓ���Ă���
        _prevButton = setButton;
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
        if (currentInputDir == Vector2.zero) {
            _isNeutral = true;
            return;
        }
        // EventSystem�̌��݂̑I���I�u�W�F�N�g���擾
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;
        if (selectObject == null) {
            //EventSystem�̑I���I�u�W�F�N�g��prev�̃I�u�W�F�N�g��ݒ肷��
            EventSystem.current.SetSelectedGameObject(_prevButton.gameObject);
            return;
        }
        // �I���I�u�W�F�N�g����{�^�������擾
        _currentButton = selectObject.GetComponent<Button>();
        if (_currentButton == null && _prevButton != null) {
            //prev��I����Ԃɂ���
            _prevButton.Select();
            _currentButton = _prevButton;
        }
        // ���E�����݂̂̔���
        bool isSame = IsSameHorizotal(currentInputDir, _prevInputDir);
        //���͂����������ŁA����x�����ꂽ��Ȃ�{�^���̏������s
        if (_isNeutral && isSame && _currentButton == _prevButton) {
            _currentButton.onClick.Invoke();
            await UniTask.Delay(200);
        }
        // ���͂��V���������Ȃ�X�V
        if (currentInputDir != _prevInputDir) {
            _prevInputDir = currentInputDir;
            _isNeutral = false;
        }
        // �{�^�����̍X�V
        _prevButton = _currentButton;
    }
    public void Teardown() {
        inputAction.UI.Disable();
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
}
