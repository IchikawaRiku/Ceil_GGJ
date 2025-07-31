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
    /// 初期ボタンの設定と入力有効化
    /// </summary>
    /// <param name="setInitButton"></param>
    public void Setup(Button setInitButton) {
        // 最初にSelectが外れたときの対策
        _prevButton = setInitButton;
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
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
        if (IsInputNeutral(currentInputDir)) return;
        // 選択オブジェクトからボタン情報を取得
        UpdateCurrentButton();
        // 左右方向のみの判定
        bool isSameHorizotal = IsSameHorizotal(currentInputDir, _prevInputDir);
        //入力が同じ方向で、かつ一度離された後ならボタンの処理実行
        if (_isNeutral && isSameHorizotal && _currentButton == _prevButton) {
            _currentButton.onClick.Invoke();
            await UniTask.Delay(200);
        }
        //入力、ボタンの更新
        UpdateInputState(currentInputDir);
    }
    public void Teardown() {
        inputAction.UI.Disable();
    }
    /// <summary>
    /// デバイス入力ゼロか判定
    /// </summary>
    /// <param name="inputDir"></param>
    /// <returns></returns>
    private bool IsInputNeutral(Vector2 inputDir) {
        if (inputDir == Vector2.zero) {
            _isNeutral = true;
            return true;
        }
        return false;
    }
    /// <summary>
    /// EventSystemの現在の選択オブジェクトからボタンの拾得、設定
    /// </summary>
    private void UpdateCurrentButton() {
        // EventSystemの現在の選択オブジェクトを取得
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;
        if (selectObject == null) {
            //EventSystemの選択オブジェクトにprevのオブジェクトを設定する
            EventSystem.current.SetSelectedGameObject(_prevButton.gameObject);
            return;
        }
        //現在のボタンの取得
        _currentButton = selectObject.GetComponent<Button>();
        if (_currentButton == null && _prevButton != null) {
            //prevを選択状態にする
            _prevButton.Select();
            _currentButton = _prevButton;
        }
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
    /// <summary>
    /// 前回の入力やボタン状態を更新
    /// </summary>
    /// <param name="inputDir"></param>
    private void UpdateInputState(Vector2 inputDir) {
        // 入力が新しい方向なら更新
        if (inputDir != _prevInputDir) {
            _prevInputDir = inputDir;
            _isNeutral = false;
        }
        // ボタン情報の更新
        _prevButton = _currentButton;
    }
}
