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

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        _inputAction = MyInputManager.inputAction;
        _buttonInput = new AcceptMenuButtonInput();
    }
    /// <summary>
    /// 開く
    /// </summary>
    /// <returns></returns>
    public override async UniTask Open() {
        await base.Open();
        _menuSelect = eMenuSelect.Invalid;
        _inputAction.Player.Pause.Enable();
        // ボタン状態の設定
        await _buttonInput.Setup(_initSelectButton);
        await SetPushButtonState(_buttonList, true);
        while (_menuSelect == eMenuSelect.Invalid) {
            //Escapeで閉じる
            if (_inputAction.Player.Pause.WasPressedThisFrame()) break;
            await _buttonInput.AcceptInput();

            await UniTask.DelayFrame(1);
        }
        _inputAction.Player.Pause.Disable();
        // ボタンの片付け処理
        await _buttonInput.Teardown();
        // ボタンの状態をリセットする
        await SetPushButtonState(_buttonList, false);
        await Close();
    }
    /// <summary>
    /// 閉じる
    /// </summary>
    /// <returns></returns>
    public override async UniTask Close() {
        await base.Close();
        // 設定メニューを開く
        if (_menuSelect == eMenuSelect.Settings) await OpenSettingMenu();
    }
    /// <summary>
    /// 設定メニューを開く
    /// </summary>
    /// <returns></returns>
    public async UniTask OpenSettingMenu() {
        await FadeManager.instance.FadeOut();
        await MenuManager.instance.Get<MenuSetting>().Open();
        await FadeManager.instance.FadeIn();
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
