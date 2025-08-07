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
using UnityEngine.UI;

using static CommonModule;

public class MenuBase : MonoBehaviour {
    // メニューオブジェクト
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
    /// <summary>
    /// ボタンの状態を設定する（押せるか押せないか）
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
