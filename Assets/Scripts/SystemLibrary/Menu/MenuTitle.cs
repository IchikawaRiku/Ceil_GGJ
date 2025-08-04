/*
 *  @file   MenuTitle.cs
 *  @brief  �^�C�g�����j���[
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTitle : MenuBase {
    // �ŏ��ɑI�������{�^��
    [SerializeField]
    private Button _initSelectButton = null;
    //�{�^�����͎�t
    private AcceptMenuButtonInput _buttonInput = null;
    //���j���[�J�t���O
    private bool _isClose = false;
    //�Q�[���I���t���O
    private bool _isGameEnd = false;
    //�ݒ�J�t���O
    private bool _isSelect = false;

    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        _isClose = false;
        _isGameEnd = false;
        _isSelect = false;
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        while (!_isClose) {
            await _buttonInput.AcceptInput();
            if (_isSelect) {
                await FadeManager.instance.FadeOut();
                await MenuManager.instance.Get<MenuSetting>().Open();
                await FadeManager.instance.FadeIn();
                await _buttonInput.Setup(_initSelectButton);
                _isSelect = false;
            }
            await UniTask.DelayFrame(1);
        }
        if (_isGameEnd) QuitApp();
        await FadeManager.instance.FadeOut();
        await Close();
    }
    /// <summary>
    /// ���j���[�J�t���O�̕ύX
    /// </summary>
    public void MenuClose() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _isClose = true;
    }
    public void ToMenuSetting() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _isSelect = true;
    }
    public void EndGame() {
        UniTask task = SoundManager.instance.PlaySE(1);
        _isClose = true;
        _isGameEnd = true;
    }
    private void QuitApp() {
#if UNITY_EDITOR
        // �G�f�B�^�[�̏ꍇ�͍Đ����[�h���~
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �r���h�ς݁iexe�j�̏ꍇ�͏I��
        Application.Quit();
#endif
    }
}
