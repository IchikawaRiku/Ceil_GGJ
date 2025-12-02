/*
 *  @file   MenuTitle.cs
 *  @brief  タイトルメニュー
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MenuTitle : MenuBase {
    //ボタンの配列
    [SerializeField]
    private Button[] _buttonList = null;
    // 最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    // 幽霊画像
    [SerializeField]
    private GhostMove _ghost = null;
    // 月画像
    [SerializeField]
    private MoonMove _moon = null;
    // 雲画像
    [SerializeField]
    private CloudMove _cloud = null;

    //ボタン入力受付
    private AcceptMenuButtonInput _buttonInput = null;
    //メニュー開閉フラグ
    private bool _isClose = false;
    //ゲーム終了フラグ
    private bool _isGameEnd = false;
    //設定開閉フラグ
    private bool _isSelect = false;

    private CancellationToken _token;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
        _ghost?.Initialize(new Vector3(0, 120, 0), new Vector3(150, 140, 0));
        _moon?.Initialize();
        _cloud?.Initialize();
    }
    /// <summary>
    /// 開く
    /// </summary>
    /// <returns></returns>
    public override async UniTask Open() {
        _token = this.GetCancellationTokenOnDestroy();
        await base.Open();
        _isClose = false;
        _isGameEnd = false;
        _isSelect = false;
        _ghost?.Setup();
        _moon?.Setup();
        _cloud?.Setup();
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        await SetPushButtonState(_buttonList, true);
        UniTask ghostMoveTask = _ghost.Execute();
        UniTask moonMoveTask = _moon.Execute();
        UniTask cloudMoveTask = _cloud.Execute();
        while (!_isClose) {
            await _buttonInput.AcceptInput();
            if (_isSelect) {
                await SetPushButtonState(_buttonList, false);
                await FadeManager.instance.FadeOut();
                await MenuManager.instance.Get<MenuSetting>().Open();
                await FadeManager.instance.FadeIn();
                await _buttonInput.Setup(_initSelectButton);
                _isSelect = false;
                await SetPushButtonState(_buttonList, true);
            }
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
        await SetPushButtonState(_buttonList, false);
        if (_isGameEnd) QuitApp();
        await FadeManager.instance.FadeOut();
        _ghost?.Teardown();
        _moon?.Teardown();
        _cloud?.Teardown();
        await Close();
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _isClose = true;
    }
    /// <summary>
    /// 設定メニュー移行
    /// </summary>
    public void ToMenuSetting() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _isSelect = true;
    }
    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    public void EndGame() {
        //UniTask task = SoundManager.instance.PlaySE(1);
        _isClose = true;
        _isGameEnd = true;
    }
    /// <summary>
    /// アプリケーションの終了処理
    /// </summary>
    private void QuitApp() {
#if UNITY_EDITOR
        // エディターの場合は再生モードを停止
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルド済み（exe）の場合は終了
        Application.Quit();
#endif
    }
}
