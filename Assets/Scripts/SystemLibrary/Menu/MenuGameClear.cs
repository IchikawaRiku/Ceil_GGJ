using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGameClear : MenuBase {
    // �ŏ��ɑI�������{�^��
    [SerializeField]
    private Button _initSelectButton = null;
    //�{�^�����͎�t
    private AcceptMenuButtonInput _buttonInput = null;
    //�^�C�g���X�L�b�v�t���O
    public static bool isTitleSkip { get; private set; } = false;
    //���j���[�J�t���O
    private bool _isClose = false;
    //�X�e�[�W���g���C�t���O
    private bool _isRetryStage = false;

    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        _isClose = false;
        isTitleSkip = false;
        _isRetryStage = false;
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        while (!_isClose) {
            //�{�^�����͏���
            await _buttonInput.AcceptInput();
            await UniTask.DelayFrame(1);
        }
        await _buttonInput.Teardown();
        await FadeManager.instance.FadeOut();
        await Close();
        if (_isRetryStage) {
            await StageManager.instance.RetryCurrentStage();
            UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
        } else {
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
        }
    }
    /// <summary>
    /// ���j���[�J�t���O�A�^�C�g���X�L�b�v�t���O�̕ύX
    /// </summary>
    public void MenuCloseToStageSelect() {
        _isClose = true;
        isTitleSkip = true;
    }
    /// <summary>
    /// ���j���[�J�t���O�A�X�e�[�W���g���C�t���O�̕ύX
    /// </summary>
    public void RetryCurrentStage() {
        _isClose = true;
        _isRetryStage = true;
    }
}
