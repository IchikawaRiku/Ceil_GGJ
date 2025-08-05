/*
 *  @file   MenuBase.cs
 *  @brief  ���j���[�̊��N���X
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static CommonModule;

public class MenuBase : MonoBehaviour {
    // ���j���[�I�u�W�F�N�g
    [SerializeField]
    private Transform _menuRoot = null;

    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize() {
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// �J��
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Open() {
        _menuRoot.gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Close() {
        _menuRoot.gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// �{�^���̏�Ԃ�ݒ肷��i�����邩�����Ȃ����j
    /// </summary>
    /// <param name="setButton"></param>
    /// <param name="setFlag"></param>
    /// <returns></returns>
    protected async UniTask SetPushButtonState(Button[] setButton, bool setFlag) {
        if(IsEmpty(setButton)) return;
        await UniTask.DelayFrame(5);
        for (int i = 0, max = setButton.Length; i < max; i++) {
            setButton[i].interactable = setFlag;
        }
        await UniTask.CompletedTask;
    }
}
