using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStageSelect : MenuBase {
    //最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    //ボタン操作入力処理
    private AcceptMenuButtonInput _buttonInput = null;
    //ステージ番号
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
    /// チュートリアルステージ選択
    /// </summary>
    public void SelectTutorialStage() {
        stageNum = eStageStage.Tutorial;
    }
    /// <summary>
    /// ステージ1選択
    /// </summary>
    public void SelectStage1() {
        stageNum = eStageStage.Stage1;
    }
    /// <summary>
    /// ステージ2選択
    /// </summary>
    public void SelectStage2() {
        stageNum = eStageStage.Stage2;
    }
    /// <summary>
    /// ステージ3選択
    /// </summary>
    public void SelectStage3() {
        stageNum = eStageStage.Stage3;
    }
    public void ReturnTitle() {
        stageNum = eStageStage.Max;
    }
}
