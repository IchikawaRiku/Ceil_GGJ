using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class AcceptButtonBase {
    //���݂̑I���{�^��
    protected Button currentButton = null;
    //��O�̑I���{�^��
    protected Button prevButton = null;

    public virtual async UniTask Setup(Button setInitButton) {
        // UI���\�����ꂽ�Ƃ��A�ŏ��ɐݒ肳���{�^�����󂯎���Ă���
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        // �ŏ���Select���O�ꂽ�Ƃ��̑΍�Ƃ��āAprev�Ɏ󂯎�����{�^�����ŏ��ɓ���Ă���
        prevButton = setInitButton;
        await UniTask.DelayFrame(1);
    }
    public abstract UniTask AcceptInput();
    public virtual async UniTask Teardown() {
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// EventSystem�̌��݂̑I���I�u�W�F�N�g����{�^���̏E���A�ݒ�
    /// </summary>
    protected void UpdateCurrentButton() {
        // EventSystem�̌��݂̑I���I�u�W�F�N�g���擾
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;
        if (selectObject == null) {
            //EventSystem�̑I���I�u�W�F�N�g��prev�̃I�u�W�F�N�g��ݒ肷��
            EventSystem.current.SetSelectedGameObject(prevButton.gameObject);
            return;
        }
        //���݂̃{�^���̎擾
        currentButton = selectObject.GetComponent<Button>();
        if (currentButton == null && prevButton != null) {
            //prev��I����Ԃɂ���
            prevButton.Select();
            currentButton = prevButton;
        }
    }
}
