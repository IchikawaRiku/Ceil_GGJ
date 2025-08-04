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

public class AcceptMenuButtonInput : AcceptButtonBase{
    public override async UniTask Setup(Button setInitButton) {
        // UI���\�����ꂽ�Ƃ��A�ŏ��ɐݒ肳���{�^�����󂯎���Ă���
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        // �ŏ���Select���O�ꂽ�Ƃ��̑΍�Ƃ��āAprev�Ɏ󂯎�����{�^�����ŏ��ɓ���Ă���
        prevButton = setInitButton;
        await UniTask.DelayFrame(1);
    }
    public override async UniTask AcceptInput() {
        // EventSystem�̌��݂̑I���I�u�W�F�N�g���擾
        UpdateCurrentButton();
        // �{�^�����̍X�V
        if (currentButton != null) {
            if (currentButton != prevButton) {
                UniTask task = SoundManager.instance.PlaySE(0);
            }
            prevButton = currentButton;
        }
        await UniTask.CompletedTask;
    }
}
