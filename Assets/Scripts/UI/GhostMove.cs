/*
 *  @file   GhostMove.cs
 *  @brief  —H—ì‚ÌˆÚ“®
 *  @author Seki
 *  @date   2025/11/25
 */
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class GhostMove : MonoBehaviour {
    private Vector3 _currentPos;
    private Vector3 _targetPos;
    private bool _isClose = false;

    private CancellationToken _token;

    public void Initialize(Vector3 startPos) {
        _currentPos = new Vector3(0, 120, 0);
        _targetPos = new Vector3(150, 140, 0);
    }

    public void Setup() {
        transform.localPosition = _currentPos;
        _isClose = false;
    }

    public async UniTask Execute() {
        _token = this.GetCancellationTokenOnDestroy();
        float duration = 3.0f;
        float elapsedTime = 0.0f;

        while (!_isClose) {
            elapsedTime += Time.deltaTime;

            // 0->1->0 ‚Ì‰•œ‚ğì‚é
            float t = Mathf.PingPong(elapsedTime / duration, 1f);

            // Slerp ‚Å•âŠÔ
            transform.localPosition = Vector3.Slerp(_currentPos, _targetPos, t);

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
    }

    public void Teardown() {
        _isClose = true;
        transform.localPosition = _currentPos;
    }
}