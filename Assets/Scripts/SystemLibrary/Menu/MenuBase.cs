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

public class MenuBase : MonoBehaviour {
    //���j���[�I�u�W�F�N�g
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
}
