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
    private AcceptMenuButtonInput _buttonInput = null;
    //�X�e�[�W�ԍ�
    public eStageStage stageNum { get; private set; } = eStageStage.Invalid;

    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        stageNum = eStageStage.Invalid;
        while (stageNum == eStageStage.Invalid) {
            await _buttonInput.AcceptInput();
            await UniTask.DelayFrame(1);
        }
        await _buttonInput.Teardown();
        await FadeManager.instance.FadeOut();
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
    }
    /// <summary>
    /// �`���[�g���A���X�e�[�W�I��
    /// </summary>
    public void SelectTutorialStage() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Tutorial;
    }
    /// <summary>
    /// �X�e�[�W1�I��
    /// </summary>
    public void SelectStage1() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Stage1;
    }
    /// <summary>
    /// �X�e�[�W2�I��
    /// </summary>
    public void SelectStage2() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Stage2;
    }
    /// <summary>
    /// �X�e�[�W3�I��
    /// </summary>
    public void SelectStage3() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Stage3;
    }
    public void ReturnTitle() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Max;
    }
}
