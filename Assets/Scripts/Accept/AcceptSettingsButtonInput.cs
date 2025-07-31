/*
 *  @file   AcceptSettingsButtonInput.cs
 *  @brief  設定メニューボタンの入力受付
 *  @author Seki
 *  @date   2025/7/31
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AcceptSettingsButtonInput {
    //入力を保存する変数
    private Vector2 _prevInputDir = Vector2.zero;
    //一度押されたか判別するフラグ
    private bool _isNeutral = false;
    //現在のボタン情報
    private Button _currentButton = null;
    //1フレーム前のボタン情報
    private Button _prevButton = null;
    //InputAction
    private DefaultInputActions inputAction = null;

    public void Initialize() {
        inputAction = new DefaultInputActions();
    }
    /// <summary>
    /// 準備処理
    /// </summary>
    /// <param name="setButton"></param>
    public void Setup(Button setButton) {
        // UIが表示されたとき、最初に設定されるボタンを受け取っておく
        EventSystem.current.SetSelectedGameObject(setButton.gameObject);
        // 最初にSelectが外れたときの対策として、prevに受け取ったボタンを最初に入れておく
        _prevButton = setButton;
        inputAction.UI.Enable();
    }
    /// <summary>
    /// ボタンの受付処理
    /// </summary>
    /// <returns></returns>
    public async UniTask AcceptInput() {
        // 現在の入力の値の取得
        Vector2 currentInputDir = inputAction.UI.Navigate.ReadValue<Vector2>();
        // 一度入力をやめたら、ニュートラル状態にする
        if (currentInputDir == Vector2.zero) {
            _isNeutral = true;
            return;
        }
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
        // 左右方向のみの判定
        bool isSame = IsSameHorizotal(currentInputDir, _prevInputDir);
        //入力が同じ方向で、かつ一度離された後ならボタンの処理実行
        if (_isNeutral && isSame && _currentButton == _prevButton) {
            _currentButton.onClick.Invoke();
            await UniTask.Delay(200);
        }
        // 入力が新しい方向なら更新
        if (currentInputDir != _prevInputDir) {
            _prevInputDir = currentInputDir;
            _isNeutral = false;
        }
        // ボタン情報の更新
        _prevButton = _currentButton;
    }
    public void Teardown() {
        inputAction.UI.Disable();
    }
    /// <summary>
    /// 左右方向のみの入力取得(方向のみの取得のためMathf.Signを使用)
    /// </summary>
    /// <param name="currentDir"></param>
    /// <param name="prevDir"></param>
    /// <returns></returns>
    private bool IsSameHorizotal(Vector2 currentDir, Vector2 prevDir) {
        // InputSystemのVectorは左右入力時にも上下成分が出てしまうことがあったため、
        // 左右成分の方が強いときのみ左右として判定
        if (Mathf.Abs(currentDir.x) <= Mathf.Abs(currentDir.y)) return false;

        return Mathf.Sign(currentDir.x) != 0 &&
            Mathf.Sign(currentDir.x) == Mathf.Sign(prevDir.x);
    }
}
