/*
 *  @file   MenuBase.cs
 *  @brief  メニューの基底クラス
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour {
    //メニューオブジェクト
    [SerializeField]
    private Transform _menuRoot = null;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize() {
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// 開く
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Open() {
        _menuRoot.gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// 閉じる
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Close() {
        _menuRoot.gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
}
