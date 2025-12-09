/*
 *  @file   MenuStageSelect.cs
 *  @brief  ステージセレクトメニュー
 *  @author Seki
 *  @date   2025/8/1
 */
using Cysharp.Threading.Tasks;
using System;
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
    // ステージ背景画像
    [SerializeField]
    private Image _stageBGImage = null;
    // 移動ボタンオブジェクト
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
    // タスク中断用トークン
    private CancellationTokenSource _changeEffectToken;
    private CancellationTokenSource _delayToken;

    // 移動先のX座標
    private const float _MOVE_POS_X = -235;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        // 各クラスの初期化
        _buttonInput = new AcceptMenuButtonInput();
        // ボタン移動演出の初期化
        _buttonMove = new ButtonSelectMove(new Button[] { 
            _buttonList[(int)eStageType.Stage1],
            _buttonList[(int)eStageType.Stage2],
            _buttonList[(int)eStageType.Stage3] 
        });
        // UIの初期化
        _moon?.Initialize();
        _cloud?.Initialize();
        // 画像アルファ値の設定
        _stageImageColor = _stageImage.color;
        _stageImageColor.a = 0.0f;
        _stageImage.color = _stageImageColor;
        _stageBGImage.color = _stageImageColor;
    }
    /// <summary>
    /// 開く
    /// </summary>
    /// <returns></returns>
    public override async UniTask Open() {
        await base.Open();
        stageNum = eStageType.Invalid;
        _buttonMove.Setup();
        ResetEffect();
        // UI演出の準備前処理
        _moon?.Setup();
        _cloud?.Setup();
        await FadeManager.instance.FadeIn();
        // ボタン情報の設定
        await SetPushButtonState(_buttonList, true);
        await _buttonInput.Setup(_initSelectButton);
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
                // 演出の初期化
                ResetEffect();

                // 演出対象なら演出開始
                if (IsEffectButton(currentButton)) {
                    StartSelectEffect(currentButton).Forget();
                }
            }
            await UniTask.DelayFrame(1);
        }
        // ボタンの片付け処理
        await _buttonInput.Teardown();
        // ボタンの状態をリセットする
        await SetPushButtonState(_buttonList, false);
        await FadeManager.instance.FadeOut();
        await Close();
    }
    /// <summary>
    /// 閉じる
    /// </summary>
    /// <returns></returns>
    public override async UniTask Close() {
        await base.Close();
        // UI演出のリセット
        _moon?.Teardown();
        _cloud?.Teardown();
        _stageImage.color = _stageImageColor;
        _stageBGImage.color = _stageImageColor;
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
        _stageBGImage.color = _stageImageColor;
    }
    /// <summary>
    /// ボタン演出開始処理
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private async UniTask StartSelectEffect(Button button) {
        if (!IsEffectButton(button)) return;
        // 既存の演出中止
        _changeEffectToken?.Cancel();
        _delayToken?.Cancel();

        _delayToken = new CancellationTokenSource();
        try {
            // 待機
            await UniTask.Delay(500, cancellationToken: _delayToken.Token);
        } catch (OperationCanceledException) {
            return;
        }
        if (_buttonInput.GetCurrentButton() != button) {
            ResetEffect();
            return;
        }

        // 演出トークンを生成
        _changeEffectToken = new CancellationTokenSource();
        // 演出
        try {
            // 画像のセット
            SetStageSprite(button);
            // 演出
            await MoveSelectButton(0.8f, _changeEffectToken.Token);
            await ShowStageImage(0.5f, _changeEffectToken.Token);
        } catch (OperationCanceledException) {
            ResetEffect();
        }
    }
    /// <summary>
    /// ボタン移動演出
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="token"></param>
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
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask ShowStageImage(float duration, CancellationToken token) {
        UniTask task = SoundManager.instance.PlaySE(10);
        float elapsedTime = 0f;
        Color color = _stageImage.color;
        color.a = 0f;
        _stageImage.color = color;
        _stageBGImage.color = color;

        while (elapsedTime < duration) {
            token.ThrowIfCancellationRequested(); // ここでキャンセル判定
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            color.a = Mathf.Lerp(0f, 1f, t);
            if(_stageImage)_stageImage.color = color;
            if( _stageBGImage) _stageBGImage.color = color;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        _stageImage.color = color;
        _stageBGImage.color = color;
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