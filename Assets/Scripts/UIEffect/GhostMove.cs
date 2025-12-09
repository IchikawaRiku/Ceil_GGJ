/*
 *  @file   GhostMove.cs
 *  @brief  幽霊の移動
 *  @author Seki
 *  @date   2025/11/25
 */
using Cysharp.Threading.Tasks;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class GhostMove : MonoBehaviour {
    // 開始位置
    private Vector3 _startPos;
    // 目標位置
    private Vector3 _targetPos;
    // 開閉フラグ
    private bool _isClose = false;

    private CancellationToken _token;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(Vector3 setStartPos, Vector3 setTargetPos) {
        _startPos = setStartPos;
        _targetPos = setTargetPos;
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
        float duration = 3.0f;
        float elapsedTime = 0.0f;

        while (!_isClose) {
            elapsedTime += Time.deltaTime;

            float t = Mathf.PingPong(elapsedTime / duration, 1f);

            transform.localPosition = Vector3.Slerp(_startPos, _targetPos, t);

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
        transform.localPosition = _targetPos;
    }
    /// <summary>
    /// 幽霊移動演出
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask ShowGhostMove(float duration = 1.0f) {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.localPosition = Vector3.Slerp(_startPos, _targetPos, t);

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
        transform.localPosition = _targetPos;
    }

    /// <summary>
    /// 片付け処理
    /// </summary>
    public void Teardown() {
        _isClose = true;
        transform.localPosition = _startPos;
    }
}