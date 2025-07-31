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

public class AcceptMenuButtonInput{
    //現在のボタン情報
    private Button _currentButton = null;
    //1フレーム前のボタン情報
    private Button _prevButton = null;

    public void Setup(Button setInitButton) {
        // UIが表示されたとき、最初に設定されるボタンを受け取っておく
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        // 最初にSelectが外れたときの対策として、prevに受け取ったボタンを最初に入れておく
        _prevButton = setInitButton;
    }

    public async UniTask AcceptInput() {
        // EventSystemの現在の選択オブジェクトを取得
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;
        if (selectObject == null) {
            //EventSystemの選択オブジェクトにprevのオブジェクトを設定する
            EventSystem.current.SetSelectedGameObject(_prevButton.gameObject);
            return;
        }
        // 選択オブジェクトからボタン情報を取得
        _currentButton = selectObject.GetComponent<Button>();
        if (_currentButton == null && _prevButton != null) {
            //prevを選択状態にする
            _prevButton.Select();
            _currentButton = _prevButton;
        }
        // ボタン情報の更新
        _prevButton = _currentButton;

        await UniTask.CompletedTask;
    }
}
