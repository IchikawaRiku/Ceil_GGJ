/*
 *  @file   PartBase.cs
 *  @brief  ゲームパートの基底
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartBase : MonoBehaviour {
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// 使用前準備処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Setup() {
        gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// 実行処理
    /// </summary>
    /// <returns></returns>
    public abstract UniTask Execute();
    /// <summary>
    /// 片付け処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Teardown() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
}
