using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGameMenu : MenuBase {
    // �ŏ��ɑI�������{�^��
    [SerializeField]
    private Button _initSelectButton = null;
    // ���j���[����邩���ʂ���t���O
    private bool _isClose = false;
    // �ݒ��ʂ��J�������ʂ���t���O
    private bool _isSettingsOpen = false;
    // �^�C�g���p�[�g�֖߂邩���ʂ���t���O
    private bool _isReturnTitle = false;

    private AcceptMenuButtonInput _acceptMenuButton = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        _acceptMenuButton = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        _acceptMenuButton.Setup(_initSelectButton);
        // ���͂���邽�߂�1�t���[���҂�
        await UniTask.DelayFrame(1);
        while (true) {
            //�{�^�����͏���
            await _acceptMenuButton.AcceptInput();
            //Escape�ŕ���
            if (_isClose || Input.GetKeyDown(KeyCode.Escape)) break;
            //Setting�t���O��Setting��ʂ֑J��
            if (_isSettingsOpen) {
                await MenuManager.instance.Get<MenuSetting>().Open();
                _isSettingsOpen = false;
                break;
            }
            //return�t���O�Ń^�C�g����ʂ֑J��
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
