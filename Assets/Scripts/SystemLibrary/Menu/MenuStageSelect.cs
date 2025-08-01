using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStageSelect : MenuBase {
    //�ŏ��ɑI�������{�^��
    [SerializeField]
    private Button _initSelectButton = null;
    //�{�^��������͏���
    private AcceptMenuButtonInput _acceptButtonInput = null;
    //�X�e�[�W�ԍ�
    private eStageStage _stageNum = eStageStage.Invalid;

    public override async UniTask Initialize() {
        await base.Initialize();
        _acceptButtonInput = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        await _acceptButtonInput.Setup(_initSelectButton);
        _stageNum = eStageStage.Invalid;
        while (_stageNum == eStageStage.Invalid) {
            await UniTask.DelayFrame(1);
        }
        await _acceptButtonInput.Teardown();
        await FadeManager.instance.FadeOut();
        await StageManager.instance.TransitionStage(_stageNum);
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
    }
    /// <summary>
    /// �`���[�g���A���X�e�[�W�I��
    /// </summary>
    public void SelectTutorialStage() {
        _stageNum = eStageStage.Tutorial;
    }
    /// <summary>
    /// �X�e�[�W1�I��
    /// </summary>
    public void SelectStage1() {
        _stageNum = eStageStage.Stage1;
    }
    /// <summary>
    /// �X�e�[�W2�I��
    /// </summary>
    public void SelectStage2() {
        _stageNum = eStageStage.Stage2;
    }
    /// <summary>
    /// �X�e�[�W3�I��
    /// </summary>
    public void SelectStage3() {
        _stageNum = eStageStage.Stage3;
    }
}
