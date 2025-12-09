/*
 *  @file   AcceptMenuButtonInput.cs
 *  @brief  メニュー項目入力受付
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
    /// <summary>
    /// 入力受付
    /// </summary>
    /// <returns></returns>
    public override async UniTask AcceptInput() {
        // EventSystemの現在の選択オブジェクトを取得
        UpdateCurrentButton();
        // ボタン情報の更新
        if (currentButton != null) {
            if (currentButton != prevButton) {
                UniTask task = SoundManager.instance.PlaySE(0);
            }
            prevButton = currentButton;
        }
        await UniTask.CompletedTask;
    }
}
