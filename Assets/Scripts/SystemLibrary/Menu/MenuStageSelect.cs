/*
 *  @file   MenuStageSelect.cs
 *  @brief  ステージセレクトメニュー
 *  @author Seki
 *  @date   2025/8/1
 */
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private Button[] _effectButtonList;
    private CancellationTokenSource _changeEffectToken;
    private CancellationTokenSource _delayToken;

    private const float _MOVE_POS_X = -235;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        // 各クラスの初期化
        _buttonInput = new AcceptMenuButtonInput();
        _buttonMove = new ButtonSelectMove(new Button[] { 
            _buttonList[(int)eStageType.Stage1],
            _buttonList[(int)eStageType.Stage2],
            _buttonList[(int)eStageType.Stage3] 
        });
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
        _buttonMove.Setup();
        await FadeManager.instance.FadeIn();
        await SetPushButtonState(_buttonList, true);
        // ボタン入力管理クラスの準備前処理
        await _buttonInput.Setup(_initSelectButton);
        _moon?.Setup();
        _cloud?.Setup();
        // 実行開始
        UniTask moonMoveTask = _moon.Execute();
        UniTask cloudMoveTask = _cloud.Execute();
        // 初期選択ボタンのEventSystem反映
        EventSystem.current.SetSelectedGameObject(_initSelectButton.gameObject);
        // 最初の演出（対象ボタンのみ）
        StartSelectEffect(_initSelectButton).Forget();
        // ボタンが押されるまでループ
        while (stageNum == eStageType.Invalid) {
            Button prevButton = _buttonInput.GetPrevButton();
            await _buttonInput.AcceptInput();
            Button currentButton = _buttonInput.GetCurrentButton();
            _buttonMove.Execute(currentButton);
            if (prevButton != currentButton) {
                // どのボタンに移っても、まず既存の演出はキャンセルして初期化する
                _changeEffectToken?.Cancel();
                _delayToken?.Cancel();
                ResetEffect();

                // 演出対象なら演出開始（内部でディレイ/確定判定を行う）
                if (IsEffectButton(currentButton)) {
                    StartSelectEffect(currentButton).Forget();
                }
            }
            await UniTask.DelayFrame(1);
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
    /// 状態の初期化
    /// </summary>
    public void ResetEffect() {
        // 移動の初期化
        _moveButtonPos.localPosition = Vector3.zero;
        // 画像の初期化
        _stageImageColor = _stageImage.color;
        _stageImageColor.a = 0.0f;
        _stageImage.color = _stageImageColor;
    }
    /// <summary>
    /// ボタン演出開始処理
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private async UniTask StartSelectEffect(Button button) {
        if (!IsEffectButton(button)) return;

        // 既存の演出は中止（演出中のトークンを止める）
        _changeEffectToken?.Cancel();

        _delayToken?.Cancel();
        _delayToken = new CancellationTokenSource();
        var delayToken = _delayToken.Token;

        try {
            // 待機
            await UniTask.Delay(500, cancellationToken: delayToken);
        } catch (OperationCanceledException) {
            return;
        }

        if (_buttonInput.GetCurrentButton() != button) {
            ResetEffect();
            return;
        }

        // 実際の演出トークンを生成
        _changeEffectToken = new CancellationTokenSource();
        var token = _changeEffectToken.Token;

        try {
            // 画像のセット
            SetStageSprite(button);
            // 演出
            await MoveSelectButton(0.8f, token);
            await ShowStageImage(0.5f, token);
        } catch (OperationCanceledException) {
            ResetEffect();
        }
    }
    /// <summary>
    /// ボタン移動演出
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask MoveSelectButton(float duration, CancellationToken token) {
        float elapsedTime = 0f;
        float startPosX = 0f;
        float goalPosX = _MOVE_POS_X;
        Vector3 movePos = Vector3.zero;

        while (elapsedTime < duration) {
            token.ThrowIfCancellationRequested(); // ここでキャンセル判定
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            movePos.x = Mathf.Lerp(startPosX, goalPosX, t);
            if (_moveButtonPos)_moveButtonPos.localPosition = movePos;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        _moveButtonPos.localPosition = movePos;
    }
    /// <summary>
    /// ステージイメージの表示演出
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask ShowStageImage(float duration, CancellationToken token) {
        UniTask task = SoundManager.instance.PlaySE(10);
        float elapsedTime = 0f;
        Color color = _stageImage.color;
        color.a = 0f;
        _stageImage.color = color;

        while (elapsedTime < duration) {
            token.ThrowIfCancellationRequested(); // ここでキャンセル判定
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            color.a = Mathf.Lerp(0f, 1f, t);
            if(_stageImage)_stageImage.color = color;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        _stageImage.color = color;
    }
    /// <summary>
    /// ボタンに応じてスプライトを変更
    /// </summary>
    /// <param name="button"></param>
    private void SetStageSprite(Button button) {
        if (button == _buttonList[(int)eStageType.Stage1])
            _stageImage.sprite = _spriteList[0];
        if (button == _buttonList[(int)eStageType.Stage2])
            _stageImage.sprite = _spriteList[1];
        if (button == _buttonList[(int)eStageType.Stage3])
            _stageImage.sprite = _spriteList[2];
    }
    /// <summary>
    /// 演出可能なボタンか判定
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private bool IsEffectButton(Button button) {
        return button == _buttonList[(int)eStageType.Stage1] || 
            button == _buttonList[(int)eStageType.Stage2] || 
            button == _buttonList[(int)eStageType.Stage3];
    }
    /// <summary>
    /// チュートリアルステージ選択
    /// </summary>
    public void SelectTutorialStage() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Tutorial;
    }
    /// <summary>
    /// ステージ1選択
    /// </summary>
    public void SelectStage1() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Stage1;
        _stageImage.sprite = _spriteList[0];
    }
    /// <summary>
    /// ステージ2選択
    /// </summary>
    public void SelectStage2() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Stage2;
        _stageImage.sprite = _spriteList[1];
    }
    /// <summary>
    /// ステージ3選択
    /// </summary>
    public void SelectStage3() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Stage3;
        _stageImage.sprite = _spriteList[2];
    }
    /// <summary>
    /// タイトル画面へ戻る
    /// </summary>
    public void ReturnTitle() {
        UniTask task = SoundManager.instance.PlaySE(1);
        stageNum = eStageType.Max;
    }
}