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

public class AcceptSettingsButtonInput : AcceptButtonBase{
    // 入力を保存する変数
    private Vector2 _prevInputDir = Vector2.zero;
    // 一度押されたか判別するフラグ
    private bool _isNeutral = false;
    //水平方向の入力
    private int _horizontalDir = 0; 
    //一度目の入力フラグ
    private bool _isFirstPress = false; 
    // InputAction
    private MyInput _inputAction = null;

    public AcceptSettingsButtonInput() {
        _inputAction = MyInputManager.inputAction;
    }
    /// <summary>
    /// 初期ボタンの設定と入力有効化
    /// </summary>
    /// <param name="setInitButton"></param>
    public override async UniTask Setup(Button setInitButton) {
        // 最初にSelectが外れたときの対策
        prevButton = setInitButton;
        EventSystem.current.SetSelectedGameObject(setInitButton.gameObject);
        _inputAction.UI.Enable();
        await UniTask.DelayFrame(1);
    }
    /// <summary>
    /// ボタンの受付処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask AcceptInput() {
        // 現在の入力の値の取得
        Vector2 currentInputDir = _inputAction.UI.Navigate.ReadValue<Vector2>();
        // 一度入力をやめたら、ニュートラル状態にする
        if (IsInputNeutral(currentInputDir)) return;
        // 選択オブジェクトからボタン情報を取得
        UpdateCurrentButton();
        // 左右方向のみの判定
        bool isSameHorizotal = IsSameHorizotal(currentInputDir, _prevInputDir);
        //入力が同じ方向で、かつ一度離された後ならボタンの処理実行
        if (_isNeutral && isSameHorizotal && currentButton == prevButton) {
            // 二回目の横入力以降なら実行
            if (currentButton.CompareTag("SettingVolumeButton")) {
                currentButton.onClick.Invoke();
                UniTask task = SoundManager.instance.PlaySE(0);
                await UniTask.Delay(200);
            }
        }
        //入力、ボタンの更新
        UpdateInputState(currentInputDir);
    }
    public override async UniTask Teardown() {
        _inputAction.UI.Disable();
        await UniTask.CompletedTask;
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
    /// 左右方向のみの入力取得(方向のみの取得のためMathf.Signを使用)
    /// </summary>
    /// <param name="currentDir"></param>
    /// <param name="prevDir"></param>
    /// <returns></returns>
    private bool IsSameHorizotal(Vector2 currentDir, Vector2 prevDir) {
        // InputSystemのVectorは左右入力時にも上下成分が出てしまうことがあったため、
        // 左右成分の方が強いときのみ左右として判定
        if (Mathf.Abs(currentDir.x) <= Mathf.Abs(currentDir.y)) {
            _horizontalDir = 0;
            _isFirstPress = false;
            return false;
        }
        int dir = (int)Mathf.Sign(currentDir.x); // -1 = 左, 1 = 右

        // 方向が変わったら初回無視フラグをリセット
        if (dir != _horizontalDir) {
            _horizontalDir = dir;
            _isFirstPress = false;
            return false;
        }

        // 同じ方向なら初回だけ無視
        if (!_isFirstPress) {
            _isFirstPress = true;
            return false;
        }

        // 2回目以降は true
        return true;
    }
    /// <summary>
    /// 前回の入力やボタン状態を更新
    /// </summary>
    /// <param name="inputDir"></param>
    private void UpdateInputState(Vector2 inputDir) {
        // 入力が新しい方向なら更新
        if (inputDir != _prevInputDir) {
            _prevInputDir = inputDir;
            // 横入力が出たときだけ中立解除
            if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.y)) {
                _isNeutral = false;
            }
        }
        // ボタン情報の更新
        if(currentButton != prevButton) {
            UniTask task = SoundManager.instance.PlaySE(0);
            prevButton = currentButton;
        }
    }
}
