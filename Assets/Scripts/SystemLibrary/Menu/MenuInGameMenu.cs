using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInGameMenu : MenuBase {
    // ���j���[����邩���ʂ���t���O
    private bool _isClose = false;
    // �ݒ��ʂ��J�������ʂ���t���O
    private bool _isSettingsOpen = false;
    // �^�C�g���p�[�g�֖߂邩���ʂ���t���O
    private bool _isReturnTitle = false;

    public override async UniTask Open() {
        await base.Open();
        // ���͂���邽�߂�1�t���[���҂�
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
    /// ���j���[�J�t���O�̕ύX
    /// </summary>
    public void MenuClose() {
        _isClose = true;
    }
    /// <summary>
    /// �ݒ��ʂ��J���t���O�ύX
    /// </summary>
    public void SettingMenuOpen() {
        _isSettingsOpen = true;
    }
    /// <summary>
    /// �^�C�g���p�[�g�ɖ߂�t���O�̕ύX
    /// </summary>
    public void ReturnTitle() {
        _isReturnTitle = true;
    }
}
