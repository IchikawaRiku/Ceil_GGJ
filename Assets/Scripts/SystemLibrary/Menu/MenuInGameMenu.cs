using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGameMenu : MenuBase {
    //�{�^���̔z��
    [SerializeField]
    private Button[] _buttonList = null;
    // �ŏ��ɑI�������{�^��
    [SerializeField]
    private Button _initSelectButton = null;
    // InputAction
    private MyInput _inputAction = null;
    //�Z���N�g���j���[
    private eMenuSelect _menuSelect = eMenuSelect.Invalid;
    //�{�^�����͏���
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
            //Escape�ŕ���
            if (_inputAction.Player.Pause.WasPressedThisFrame()) break;
            //�{�^�����͏���
            await _buttonInput.AcceptInput();
            //Setting�t���O��Setting��ʂ֑J��
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
    /// ���j���[�J�t���O�̕ύX
    /// </summary>
    public void MenuClose() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _menuSelect = eMenuSelect.CloseMenu;
    }
    /// <summary>
    /// �ݒ��ʂ��J���t���O�ύX
    /// </summary>
    public void SettingMenuOpen() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _menuSelect = eMenuSelect.Settings;
    }
    /// <summary>
    /// �^�C�g���p�[�g�ɖ߂�t���O�̕ύX
    /// </summary>
    public void ReturnTitle() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _menuSelect = eMenuSelect.ReturnTitle;
        MainGameProcessor.EndGameReason(eEndReason.Return);
    }
}
