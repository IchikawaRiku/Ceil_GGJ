/*
 *  @file   CloudMove.cs
 *  @brief  ‰_‚ÌˆÚ“®
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
    private Vector3 _currentPos;
    [SerializeField]
    private Vector3 _targetPos;

    private bool _isClose = false;

    private CancellationToken _token;

    /// <summary>
    /// ‰Šú‰»ˆ—
    /// </summary>
    public void Initialize() {
        _currentPos = Vector3.zero;
        _targetPos = new Vector3(0, 30, 0);
    }
    /// <summary>
    /// €”õ‘Oˆ—
    /// </summary>
    public void Setup() {
        transform.localPosition = _currentPos;
        _isClose = false;
    }
    /// <summary>
    /// Àsˆ—
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        _token = this.GetCancellationTokenOnDestroy();
        float duration = 3.0f;
        float elapsedTime = 0.0f;

        while (!_isClose) {
            elapsedTime += Time.deltaTime;

            // 0->1->0 ‚Ì‰•œ‚ğì‚é
            float t = Mathf.PingPong(elapsedTime / duration, 1f);

            // Slerp ‚Å•âŠÔ
            transform.localPosition = Vector3.Lerp(_currentPos, _targetPos, t);

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
    }
    /// <summary>
    /// •Ğ•t‚¯ˆ—
    /// </summary>
    public void Teardown() {
        _isClose = true;
        transform.localPosition = _currentPos;
    }

}
