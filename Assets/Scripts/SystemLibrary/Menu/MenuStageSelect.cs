/*
 *  @file   MenuStageSelect.cs
 *  @brief  ステージセレクトメニュー
 *  @author Seki
 *  @date   2025/8/1
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStageSelect : MenuBase {
    // ボタンの配列
    [SerializeField]
    private Button[] _buttonList = null;
    // 最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    // 月画像
    [SerializeField]
    private MoonMove _moon = null;
    // 雲画像
    [SerializeField]
    private CloudMove _cloud = null;
    // ステージ画像リスト
    [SerializeField]
    private Sprite[] _spriteList = null;
    // ステージ画像
    [SerializeField]
    private Image _stageImage = null;
    [SerializeField]
    private Transform _moveButtonPos = null;
    
    // ステージ番号
    public eStageType stageNum { get; private set; } = eStageType.Invalid;

    // ボタン操作入力処理
    private AcceptMenuButtonInput _buttonInput = null;
    // ボタン移動制御クラス
    private ButtonSelectMove _buttonMove = null;
    // ステージ画像の色
    private Color _stageImageColor;

    private const float _MOVE_POS_X = -235;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        // 各クラスの初期化
        _buttonInput = new AcceptMenuButtonInput();
        _buttonMove = new ButtonSelectMove(new Button[] { _buttonList[2], _buttonList[3], _buttonList[4] });
        _moon?.Initialize();
        _cloud?.Initialize();
        // 画像アルファ値の設定
        _stageImageColor = _stageImage.color;
        _stageImageColor.a = 0.0f;
        _stageImage.color = _stageImageColor;
    }
    /// <summary>
    /// 開く
    /// </summary>
    /// <returns></returns>
    public override async UniTask Open() {
        await base.Open();
        stageNum = eStageType.Invalid;
        await FadeManager.instance.FadeIn();
        await _buttonInput.Setup(_initSelectButton);
        _buttonMove.Setup();
        await SetPushButtonState(_buttonList, true);
        _moon?.Setup();
        _cloud?.Setup();
        // 実行開始
        UniTask moonMoveTask = _moon.Execute();
        UniTask cloudMoveTask = _cloud.Execute();
        while (stageNum == eStageType.Invalid) {
            await _buttonInput.AcceptInput();
            _buttonMove.Execute(_buttonInput.GetCurrentButton());
            await UniTask.DelayFrame(1);
        }
        if(stageNum != eStageType.Max) {
            await MoveSelectButton(0.8f);
            await ShowStageImage();
        }
        await SetPushButtonState(_buttonList, false);
        await _buttonInput.Teardown();
        await FadeManager.instance.FadeOut();
        await Close();
    }
    /// <summary>
    /// 閉じる
    /// </summary>
    /// <returns></returns>
    public override async UniTask Close() {
        await base.Close();
        _moon?.Teardown();
        _cloud?.Teardown();
        _stageImage.color = _stageImageColor;
        _moveButtonPos.localPosition = Vector3.zero;
    }
    /// <summary>
    /// ボタン移動演出
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask MoveSelectButton(float duration = 1.0f) {
        float elapsedTime = 0.0f;
        float startPosX = 0.0f;
        float goalPosX = _MOVE_POS_X;
        Vector3 movePos = _moveButtonPos.localPosition;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            movePos.x = Mathf.Lerp(startPosX, goalPosX, t);
            _moveButtonPos.localPosition = movePos;
            await UniTask.DelayFrame(1);
        }
        _moveButtonPos.localPosition = movePos;
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// ステージイメージの表示演出
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask ShowStageImage(float duration = 1.0f) {
        UniTask task = SoundManager.instance.PlaySE(10);
        float elapsedTime = 0.0f;
        float imageAlpha = _stageImage.color.a;
        Color targetColor = _stageImage.color;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            targetColor.a = Mathf.Lerp(imageAlpha, 1.0f, t);
            _stageImage.color = targetColor;
            await UniTask.DelayFrame(1);
        }
        _stageImage.color = targetColor;
    }

    /// <summary>
    /// チュートリアルステージ選択
    /// </summary>
    public void SelectTutorialStage() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Tutorial;
        _stageImage.sprite = _spriteList[0];
    }
    /// <summary>
    /// ステージ1選択
    /// </summary>
    public void SelectStage1() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Stage1;
        _stageImage.sprite = _spriteList[1];
    }
    /// <summary>
    /// ステージ2選択
    /// </summary>
    public void SelectStage2() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Stage2;
        _stageImage.sprite = _spriteList[2];
    }
    /// <summary>
    /// ステージ3選択
    /// </summary>
    public void SelectStage3() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Stage3;
        _stageImage.sprite = _spriteList[3];
    }
    /// <summary>
    /// タイトル画面へ戻る
    /// </summary>
    public void ReturnTitle() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Max;
    }
}