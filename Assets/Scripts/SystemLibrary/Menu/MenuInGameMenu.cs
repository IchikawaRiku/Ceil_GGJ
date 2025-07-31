using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGameMenu : MenuBase {
    // 最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    // InputAction
    private MyInput _inputAction = null;
    // メニューを閉じるか判別するフラグ
    private bool _isClose = false;
    // 設定画面を開くか判別するフラグ
    private bool _isSettingsOpen = false;
    // タイトルパートへ戻るか判別するフラグ
    private bool _isReturnTitle = false;

    private AcceptMenuButtonInput _acceptMenuButton = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        _inputAction = MyInputManager.inputAction;
        _acceptMenuButton = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        _inputAction.Player.Pause.Enable();
        _acceptMenuButton.Setup(_initSelectButton);
        // 入力を取るために1フレーム待つ
        await UniTask.DelayFrame(1);
        while (true) {
            //ボタン入力処理
            await _acceptMenuButton.AcceptInput();
            //Escapeで閉じる
            if (_isClose || _inputAction.Player.Pause.IsPressed()) break;
            //SettingフラグでSetting画面へ遷移
            if (_isSettingsOpen) {
                await MenuManager.instance.Get<MenuSetting>().Open();
                _isSettingsOpen = false;
                break;
            }
            //returnフラグでタイトル画面へ遷移
            if (_isReturnTitle) {
                await FadeManager.instance.FadeOut();
                UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
                _isReturnTitle = false;
                break;
            }
            await UniTask.DelayFrame(1);
        }
        _isClose = false;
        _inputAction.Player.Pause.Disable();
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
