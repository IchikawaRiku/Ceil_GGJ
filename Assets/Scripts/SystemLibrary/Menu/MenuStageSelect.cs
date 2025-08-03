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
    private eStageStage _stageNum = eStageStage.Invalid;

    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        _stageNum = eStageStage.Invalid;
        while (_stageNum == eStageStage.Invalid) {
            await UniTask.DelayFrame(1);
        }
        await _buttonInput.Teardown();
<<<<<<< Updated upstream
        await StageManager.instance.TransitionStage(_stageNum);
        await FadeManager.instance.FadeOut();
=======
        await FadeManager.instance.FadeOut();
        //ステージ以外はタイトルに戻る
        if(_stageNum == eStageStage.Max) {
            await FadeManager.instance.FadeOut();
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
        } else {
            await StageManager.instance.TransitionStage(_stageNum);
        }
>>>>>>> Stashed changes
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
    }
    /// <summary>
    /// チュートリアルステージ選択
    /// </summary>
    public void SelectTutorialStage() {
        _stageNum = eStageStage.Tutorial;
    }
    /// <summary>
    /// ステージ1選択
    /// </summary>
    public void SelectStage1() {
        _stageNum = eStageStage.Stage1;
    }
    /// <summary>
    /// ステージ2選択
    /// </summary>
    public void SelectStage2() {
        _stageNum = eStageStage.Stage2;
    }
    /// <summary>
    /// ステージ3選択
    /// </summary>
    public void SelectStage3() {
        _stageNum = eStageStage.Stage3;
    }
    public void ReturnTitle() {
        _stageNum = eStageStage.Max;
    }
}
