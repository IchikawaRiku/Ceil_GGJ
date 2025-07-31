using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInGameMenu : MenuBase {
    // メニューを閉じるか判別するフラグ
    private bool _isClose = false;
    // 設定画面を開くか判別するフラグ
    private bool _isSettingsOpen = false;
    // タイトルパートへ戻るか判別するフラグ
    private bool _isReturnTitle = false;

    public override async UniTask Open() {
        await base.Open();
        // 入力を取るために1フレーム待つ
        await UniTask.DelayFrame(1);
        while (true) {
            if (_isClose || Input.GetKeyDown(KeyCode.Escape)) break;

            if (_isSettingsOpen) {
                await MenuManager.instance.Get<MenuSetting>().Open();
                _isSettingsOpen = false;
            }

            if (_isReturnTitle) {
                await FadeManager.instance.FadeOut();
                UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
                _isReturnTitle = false;
                break;
            }
            await UniTask.DelayFrame(1);
        }
        _isClose = false;
        await Close();
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        _isClose = true;
    }
    /// <summary>
    /// 設定画面を開くフラグ変更
    /// </summary>
    public void SettingMenuOpen() {
        _isSettingsOpen = true;
    }
    /// <summary>
    /// タイトルパートに戻るフラグの変更
    /// </summary>
    public void ReturnTitle() {
        _isReturnTitle = true;
    }
}
