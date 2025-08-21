using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGameMenu : MenuBase {
    //ボタンの配列
    [SerializeField]
    private Button[] _buttonList = null;
    // 最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    // InputAction
    private MyInput _inputAction = null;
    //セレクトメニュー
    private eMenuSelect _menuSelect = eMenuSelect.Invalid;
    //ボタン入力処理
    private AcceptMenuButtonInput _buttonInput = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        _inputAction = MyInputManager.inputAction;
        _buttonInput = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        _menuSelect = eMenuSelect.Invalid;
        _inputAction.Player.Pause.Enable();
        await _buttonInput.Setup(_initSelectButton);
        await SetPushButtonState(_buttonList, true);
        while (_menuSelect == eMenuSelect.Invalid) {
            //Escapeで閉じる
            if (_inputAction.Player.Pause.WasPressedThisFrame()) break;
            //ボタン入力処理
            await _buttonInput.AcceptInput();
            //SettingフラグでSetting画面へ遷移
            await UniTask.DelayFrame(1);
        }
        _inputAction.Player.Pause.Disable();
        await _buttonInput.Teardown();
        await SetPushButtonState(_buttonList, true);
        await Close();
        if (_menuSelect == eMenuSelect.Settings) {
            await FadeManager.instance.FadeOut();
            await MenuManager.instance.Get<MenuSetting>().Open();
            await FadeManager.instance.FadeIn();
        }
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _menuSelect = eMenuSelect.CloseMenu;
    }
    /// <summary>
    /// 設定画面を開くフラグ変更
    /// </summary>
    public void SettingMenuOpen() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _menuSelect = eMenuSelect.Settings;
    }
    /// <summary>
    /// タイトルパートに戻るフラグの変更
    /// </summary>
    public void ReturnTitle() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _menuSelect = eMenuSelect.ReturnTitle;
        MainGameProcessor.EndGameReason(eEndReason.Return);
    }
}
