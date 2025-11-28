/*
 *  @file   MoonMove.cs
 *  @brief  月の移動
 *  @author Seki
 *  @date   2025/11/26
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MoonMove : MonoBehaviour {
    private float _rotationMultiplier = 500f;  // 回転速度の倍率
    private AudioSource _bgmAudio;             // 音量を取る

    private bool _isClose = false;             // 開閉フラグ

    private CancellationToken _token;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize() {
        _bgmAudio = SoundManager.instance.GetBGMSource();
    }
    /// <summary>
    /// 準備前処理
    /// </summary>
    public void Setup() {
        _isClose = false;
        transform.localRotation *= Quaternion.Euler(0, 0, 0);
    }
    /// <summary>
    /// 実行処理
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        _token = this.GetCancellationTokenOnDestroy();

        while (!_isClose) {
            // 音量(0〜1)を取得
            float volume = 0f;
            if (_bgmAudio != null) volume = _bgmAudio.volume;

            // 回転速度 = 音量 * 任意の倍率
            float speed = volume * _rotationMultiplier;

            // ローカル座標の Z 軸回転を変更
            transform.localRotation *= Quaternion.Euler(0, 0, -speed * Time.deltaTime);

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    public void Teardown() {
        _isClose = true;
    }
}
