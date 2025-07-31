/*
 *  @file   AcceptMenuButtonInput.cs
 *  @brief  ���j���[���ړ��͎�t
 *  @author Seki
 *  @date   2025/7/31
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AcceptMenuButtonInput{
    //���݂̃{�^�����
    private Button _currentButton = null;
    //1�t���[���O�̃{�^�����
    private Button _prevButton = null;

    public void Setup(Button setInitButton) {
        // UI���\�����ꂽ�Ƃ��A�ŏ��ɐݒ肳���{�^�����󂯎���Ă���
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        // �ŏ���Select���O�ꂽ�Ƃ��̑΍�Ƃ��āAprev�Ɏ󂯎�����{�^�����ŏ��ɓ���Ă���
        _prevButton = setInitButton;
    }

    public async UniTask AcceptInput() {
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
        // �{�^�����̍X�V
        _prevButton = _currentButton;

        await UniTask.CompletedTask;
    }
}
