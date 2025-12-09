/*
 *  @file   CloudMove.cs
 *  @brief  雲の移動
 *  @author Seki
 *  @date   2025/11/27
 */
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class CloudMove : MonoBehaviour {
    // 開始位置
    private Vector3 _startPos;
    // 向かう位置
    private Vector3 _targetPos;

    // SEの音量倍率(0.0f～1.0f)
    private float _seVolume = 0.0f;
    // 終了フラグ
    private bool _isClose = false;

    private CancellationToken _token;

    // 通常スピード
    private const float _BASE_SPEED = 2.0f;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize() {
        _startPos = Vector3.zero;
        _targetPos = new Vector3(0, 30, 0);
        _seVolume = SoundManager.instance.GetSEVolume();
    }
    /// <summary>
    /// 準備前処理
    /// </summary>
    public void Setup() {
        transform.localPosition = _startPos;
        _isClose = false;
    }
    /// <summary>
    /// 実行処理
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        _token = this.GetCancellationTokenOnDestroy();
        float speed = 1.0f;
        float elapsedTime = 0.0f;
        float duration = 2.0f;

        while (!_isClose) {
            _seVolume = SoundManager.instance.GetSEVolume();
            speed = Mathf.Lerp(0.1f, _BASE_SPEED, _seVolume);
            elapsedTime += speed * Time.deltaTime;
            float t = Mathf.PingPong(elapsedTime / duration, 1.0f);
            transform.localPosition = Vector3.Lerp(_startPos, _targetPos, t);
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    public void Teardown() {
        _isClose = true;
        transform.localPosition = _startPos;
    }
}
