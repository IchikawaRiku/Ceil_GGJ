/*
 *  @file   ButtonSelectMove.cs
 *  @brief  ボタンの移動
 *  @author Seki
 *  @date   2025/12/1
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class ButtonSelectMove {
    // ボタンリスト
    private Button[] _buttonList;
    // 現在のボタン
    private Button _currentButton;

    // 固定座標
    private Vector3 _centerPos;
    private Vector3 _upPos;
    private Vector3 _downPos;
    // 固定スケール
    private float _CENTER_SCALE = 1.1f;
    private float _ELSE_SCALE = 0.8f;
    // 補間係数
    private float _ANIM_SPEED = 0.2f;

    private CancellationTokenSource _token;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ButtonSelectMove(Button[] setButtonList) {
        _centerPos = new Vector3(0, -55, -200);
        _upPos = new Vector3(0, 45, 0);
        _downPos = new Vector3(0, -155, 0);
        _buttonList = setButtonList;
    }
    /// <summary>
    /// 準備前処理
    /// </summary>
    public void Setup() {
        if (_buttonList != null && _buttonList.Length > 0) {
            _currentButton = _buttonList[0];
            ApplyLayoutInstant(_currentButton);

            // アニメーションループ開始
            _token = new CancellationTokenSource();
            AnimateLoop(_token.Token).Forget();
        }
    }
    /// <summary>
    /// 選択ボタン変更
    /// </summary>
    public void Execute(Button selectButton) {
        if (selectButton == null || selectButton == _currentButton)return;

        _currentButton = selectButton;
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    public void Teardown() {
        _token?.Cancel();
        _token = null;
    }
    /// <summary>
    /// フレームごとのアニメーション
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTask AnimateLoop(CancellationToken token) {
        while (!token.IsCancellationRequested) {
            if (_currentButton == null)break;

            int centerIndex = GetButtonIndex(_currentButton);

            for (int i = 0; i < _buttonList.Length; i++) {
                if (_buttonList[i] == null)continue;

                Vector3 targetPos = GetPosFor(i - centerIndex);
                float targetScale = GetScaleFor(i - centerIndex);
                // 移動アニメーション
                _buttonList[i].transform.localPosition =
                    Vector3.Lerp(_buttonList[i].transform.localPosition, targetPos, _ANIM_SPEED);
                // スケール移動
                _buttonList[i].transform.localScale =
                    Vector3.Lerp(_buttonList[i].transform.localScale, Vector3.one * targetScale, _ANIM_SPEED);
            }
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, cancellationToken: token);
        }
    }
    /// <summary>
    /// ボタンの位置とスケールを即座に適用する処理
    /// </summary>
    /// <param name="center"></param>
    private void ApplyLayoutInstant(Button center) {
        if (center == null)return;

        int centerIndex = GetButtonIndex(center);
        for (int i = 0; i < _buttonList.Length; i++) {
            if (_buttonList[i] == null)　continue;
            _buttonList[i].transform.localPosition = GetPosFor(i - centerIndex);
            _buttonList[i].transform.localScale = Vector3.one * GetScaleFor(i - centerIndex);
        }
    }
    /// <summary>
    /// ボタンの位置に応じた座標の取得
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetPosFor(int offset) {
        if (offset == 0) return _centerPos;
        if (offset == 1 || offset == -2) return _downPos;
        if (offset == -1 || offset == 2) return _upPos;

        return _centerPos;
    }
    /// <summary>
    /// ボタンの位置に応じた拡大率の取得
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private float GetScaleFor(int offset) {
        if (offset == 0) {
            return _CENTER_SCALE;
        } 

        return _ELSE_SCALE;
    }
    /// <summary>
    /// ボタンの取得
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private int GetButtonIndex(Button button) {
        if (button == null)return 0;
        for (int i = 0; i < _buttonList.Length; i++) {
            if (_buttonList[i] == button)
                return i;
        }
        return 0;
    }
}