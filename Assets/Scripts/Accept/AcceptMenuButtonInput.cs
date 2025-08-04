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
    public override async UniTask Setup(Button setInitButton) {
        // UIが表示されたとき、最初に設定されるボタンを受け取っておく
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        // 最初にSelectが外れたときの対策として、prevに受け取ったボタンを最初に入れておく
        prevButton = setInitButton;
        await UniTask.DelayFrame(1);
    }
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
