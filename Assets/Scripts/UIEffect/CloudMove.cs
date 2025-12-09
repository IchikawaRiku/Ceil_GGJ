/*
 *  @file   CloudMove.cs
 *  @brief  雲の移動
 *  @author Seki
 *  @date   2025/11/27
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CloudMove : MonoBehaviour {
    [SerializeField]
    private Vector3 _startPos;
    [SerializeField]
    private Vector3 _targetPos;

    private float _seVolume = 0.0f;
    private bool _isClose = false;

    private CancellationToken _token;

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
        int dir = 1; // 1: 0→1 , -1: 1→0

        while (!_isClose) {
            _seVolume = SoundManager.instance.GetSEVolume();

            // 音量の強調処理
            float v = Mathf.Pow(_seVolume, 0.3f);

            // 速度に反映
            speed = Mathf.Lerp(0.1f, _BASE_SPEED, v);

            elapsedTime += (speed * Time.deltaTime) * dir;

            // 端で反転
            if (elapsedTime > 1f) { elapsedTime = 1f; dir = -1; }
            if (elapsedTime < 0f) { elapsedTime = 0f; dir = 1; }

            transform.localPosition = Vector3.Lerp(_startPos, _targetPos, elapsedTime);

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
