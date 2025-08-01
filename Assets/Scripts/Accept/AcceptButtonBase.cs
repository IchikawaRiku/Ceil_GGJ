using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class AcceptButtonBase {
    //現在の選択ボタン
    protected Button currentButton = null;
    //一つ前の選択ボタン
    protected Button prevButton = null;

    public virtual async UniTask Setup(Button setInitButton) {
        // UIが表示されたとき、最初に設定されるボタンを受け取っておく
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        // 最初にSelectが外れたときの対策として、prevに受け取ったボタンを最初に入れておく
        prevButton = setInitButton;
        await UniTask.DelayFrame(1);
    }
    public abstract UniTask AcceptInput();
    public virtual async UniTask Teardown() {
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// EventSystemの現在の選択オブジェクトからボタンの拾得、設定
    /// </summary>
    protected void UpdateCurrentButton() {
        // EventSystemの現在の選択オブジェクトを取得
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;
        if (selectObject == null) {
            //EventSystemの選択オブジェクトにprevのオブジェクトを設定する
            EventSystem.current.SetSelectedGameObject(prevButton.gameObject);
            return;
        }
        //現在のボタンの取得
        currentButton = selectObject.GetComponent<Button>();
        if (currentButton == null && prevButton != null) {
            //prevを選択状態にする
            prevButton.Select();
            currentButton = prevButton;
        }
    }
}
