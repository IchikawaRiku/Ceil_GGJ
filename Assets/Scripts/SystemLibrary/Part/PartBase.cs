/*
 *  @file   PartBase.cs
 *  @brief  �Q�[���p�[�g�̊��
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartBase : MonoBehaviour {
    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// �g�p�O��������
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Setup() {
        gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// ���s����
    /// </summary>
    /// <returns></returns>
    public abstract UniTask Execute();
    /// <summary>
    /// �Еt������
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Teardown() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
}
