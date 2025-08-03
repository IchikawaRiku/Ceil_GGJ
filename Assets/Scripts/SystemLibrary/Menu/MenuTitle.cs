/*
 *  @file   MenuTitle.cs
 *  @brief  タイトルメニュー
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTitle : MenuBase {
    // 最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    //ボタン入力受付
    private AcceptMenuButtonInput _buttonInput = null;
    //メニュー開閉フラグ
    private bool _isClose = false;
    //ゲーム終了フラグ
    private bool _isGameEnd = false;

    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        _isClose = false;
        _isGameEnd = false;
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        while (!_isClose) {
            await _buttonInput.AcceptInput();
            await UniTask.DelayFrame(1);
        }
        if (_isGameEnd) QuitApp();
        await FadeManager.instance.FadeOut();
        await Close();
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        _isClose = true;
    }
    public void EndGame() {
        _isClose = true;
        _isGameEnd = true;
    }
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
