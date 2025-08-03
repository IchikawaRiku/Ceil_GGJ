using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuGameOver : MenuBase {
    // 最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    // ボタンの入力受付
    private AcceptMenuButtonInput _buttonInput = null;
    //メニュー開閉フラグ
    private bool _isClose = false;
    //ステージリトライフラグ
    private bool _isRetryStage = false;

    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        _isClose = false;
        _isRetryStage = false;
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        while (!_isClose) {
            await _buttonInput.AcceptInput();
            await UniTask.DelayFrame(1);
        }
        await _buttonInput.Teardown();
        await FadeManager.instance.FadeOut();
        await Close();
        if (_isRetryStage) {
            await StageManager.instance.RetryCurrentStage();
            UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
        } else {
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
        }
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        _isClose = true;
    }
    /// <summary>
    /// メニュー開閉フラグ、ステージリトライフラグの変更
    /// </summary>
    public void RetryCurrentStage() {
        _isClose = true;
        _isRetryStage = true;
    }
}
