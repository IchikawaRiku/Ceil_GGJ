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

    private Button[] buttonList;
    private Button currentButton;

    // 固定座標
    private Vector3 centerPos = new Vector3(0, -40, -200);
    private Vector3 upPos = new Vector3(0, 80, 0);
    private Vector3 downPos = new Vector3(0, -160, 0);

    private float centerScale = 1.1f;
    private float elseScale = 0.8f;

    private float animationSpeed = 0.2f; // 補間係数

    private CancellationTokenSource _token;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ButtonSelectMove(Button[] setButtonList) {
        buttonList = setButtonList;
    }
    /// <summary>
    /// 準備前処理
    /// </summary>
    public void Setup() {
        if (buttonList != null && buttonList.Length > 0) {
            currentButton = buttonList[0];
            ApplyLayoutInstant(currentButton);

            // アニメーションループ開始
            _token = new CancellationTokenSource();
            AnimateLoop(_token.Token).Forget();
        }
    }
    /// <summary>
    /// 選択ボタン変更
    /// </summary>
    public void Execute(Button selectButton) {
        if (selectButton == null || selectButton == currentButton)return;

        currentButton = selectButton;
    }
    /// <summary>
    /// 終了処理
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
            if (currentButton == null)break;

            int centerIndex = GetButtonIndex(currentButton);

            for (int i = 0; i < buttonList.Length; i++) {

                if (buttonList[i] == null)continue;

                Vector3 targetPos = GetPosFor(i - centerIndex);
                float targetScale = GetScaleFor(i - centerIndex);

                buttonList[i].transform.localPosition =
                    Vector3.Lerp(buttonList[i].transform.localPosition, targetPos, animationSpeed);

                buttonList[i].transform.localScale =
                    Vector3.Lerp(buttonList[i].transform.localScale, Vector3.one * targetScale, animationSpeed);
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
        for (int i = 0; i < buttonList.Length; i++) {
            if (buttonList[i] == null)　continue;
            buttonList[i].transform.localPosition = GetPosFor(i - centerIndex);
            buttonList[i].transform.localScale = Vector3.one * GetScaleFor(i - centerIndex);
        }
    }
    /// <summary>
    /// ボタンの位置に応じた座標の取得
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetPosFor(int offset) {
        if (offset == 0)
            return centerPos;
        if (offset == 1 || offset == -2)
            return downPos;
        if (offset == -1 || offset == 2)
            return upPos;
        return centerPos;
    }
    /// <summary>
    /// ボタンの位置に応じた拡大率の取得
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private float GetScaleFor(int offset) {
        if (offset == 0) {
            return centerScale;
        } 

        return elseScale;
    }
    /// <summary>
    /// ボタンの取得
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private int GetButtonIndex(Button button) {
        if (button == null)return 0;
        for (int i = 0; i < buttonList.Length; i++) {
            if (buttonList[i] == button)
                return i;
        }
        return 0;
    }
}