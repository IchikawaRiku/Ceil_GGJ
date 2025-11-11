/*
 *  @file   MenuGameClear.cs
 *  @brief  ゲームクリアメニュー
 *  @author Seki
 *  @date   2025/8/1
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGameClear : MenuBase {
    //ボタンの配列
    [SerializeField]
    private Button[] _buttonList = null;
    // 最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    //ボタン入力受付
    private AcceptMenuButtonInput _buttonInput = null;
    //タイトルスキップフラグ
    public static bool isTitleSkip { get; private set; } = false;
    //メニュー開閉フラグ
    private bool _isClose = false;
    //ステージリトライフラグ
    private bool _isRetryStage = false;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
    }
    /// <summary>
    /// 開く
    /// </summary>
    /// <returns></returns>
    public override async UniTask Open() {
        await base.Open();
        _isClose = false;
        isTitleSkip = false;
        _isRetryStage = false;
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        await SetPushButtonState(_buttonList, true);
        //ボタン入力処理
        while (!_isClose) {
            await _buttonInput.AcceptInput();
            await UniTask.DelayFrame(1);
        }
        // ボタンの片付け処理
        await _buttonInput.Teardown();
        // ボタンの状態をリセットする
        await SetPushButtonState(_buttonList, false);
        await FadeManager.instance.FadeOut();
        await Close();
    }
    /// <summary>
    /// 閉じる
    /// </summary>
    /// <returns></returns>
    public override async UniTask Close() {
        await base.Close();
        // フラグによってシーンを変える
        if (_isRetryStage) {
            await StageManager.instance.RetryCurrentStage();
            UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
        } else {
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
            isTitleSkip = false;
        }
    }
    /// <summary>
    /// メニュー開閉フラグ、タイトルスキップフラグの変更
    /// </summary>
    public void MenuCloseToStageSelect() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _isClose = true;
        isTitleSkip = true;
    }
    /// <summary>
    /// メニュー開閉フラグ、ステージリトライフラグの変更
    /// </summary>
    public void RetryCurrentStage() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _isClose = true;
        _isRetryStage = true;
    }
}
