/*
 *  @file   MoonMove.cs
 *  @brief  月の移動
 *  @author Seki
 *  @date   2025/11/26
 */
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MoonMove : MonoBehaviour {
    // BGM音量
    private float _bgmVolume;
    // 終了フラグ
    private bool _isClose = false;

    private CancellationToken _token;

    // 回転速度の倍率
    private const float _ROTATION_MUL = 500f;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize() {
        _bgmVolume = SoundManager.instance.GetBGMVolume();
    }
    /// <summary>
    /// 準備前処理
    /// </summary>
    public void Setup() {
        _isClose = false;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    /// <summary>
    /// 実行処理
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        _token = this.GetCancellationTokenOnDestroy();

        while (!_isClose) {
            _bgmVolume = SoundManager.instance.GetBGMVolume();
            float speed = _bgmVolume * _ROTATION_MUL;
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
