using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStageSelect : MenuBase {
    //ボタンの配列
    [SerializeField]
    private Button[] _buttonList = null;
    //最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    [SerializeField]
    private MoonMove _moon = null;
    [SerializeField]
    private CloudMove _cloud = null;

    //ステージ番号
    public eStageStage stageNum { get; private set; } = eStageStage.Invalid;

    //ボタン操作入力処理
    private AcceptMenuButtonInput _buttonInput = null;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        _buttonInput = new AcceptMenuButtonInput();
        _moon?.Initialize();
        _cloud?.Initialize();
    }
    /// <summary>
    /// 開く
    /// </summary>
    /// <returns></returns>
    public override async UniTask Open() {
        await base.Open();
        stageNum = eStageStage.Invalid;
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        await SetPushButtonState(_buttonList, true);
        _moon?.Setup();
        _cloud?.Setup();
        UniTask moonMoveTask = _moon.Execute();
        UniTask cloudMoveTask = _cloud.Execute();
        while (stageNum == eStageStage.Invalid) {
            await _buttonInput.AcceptInput();
            await UniTask.DelayFrame(1);
        }
        await _buttonInput.Teardown();
        await SetPushButtonState(_buttonList, false);
        await FadeManager.instance.FadeOut();
        _moon?.Teardown();
        _cloud?.Teardown();
        await Close();
    }
    /// <summary>
    /// チュートリアルステージ選択
    /// </summary>
    public void SelectTutorialStage() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Tutorial;
    }
    /// <summary>
    /// ステージ1選択
    /// </summary>
    public void SelectStage1() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Stage1;
    }
    /// <summary>
    /// ステージ2選択
    /// </summary>
    public void SelectStage2() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Stage2;
    }
    /// <summary>
    /// ステージ3選択
    /// </summary>
    public void SelectStage3() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Stage3;
    }
    /// <summary>
    /// タイトル画面へ戻る
    /// </summary>
    public void ReturnTitle() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageStage.Max;
    }
}
