using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�C�b�`����������
/// </summary>
public static class SwitchUtility {
    // ���X�C�b�`���������Ԃ̃X�C�b�`
    private static Gimmick_Switch _currentSwitch;

    /// <summary>
    /// �������Ƃ��ł���X�C�b�`��o�^
    /// </summary>
    /// <param name="currentSwitch"></param>
    public static void Register(Gimmick_Switch currentSwitch) {
        _currentSwitch = currentSwitch;
    }

    /// <summary>
    /// �X�C�b�`�̏�Ԃ����ɖ߂�
    /// </summary>
    public static void Clear() {
        _currentSwitch = null;
    }

    /// <summary>
    /// �X�C�b�`������
    /// </summary>
    public static void Press() {
        _currentSwitch?.Press();  // �o�^����Ă���Ή���
    }
}
