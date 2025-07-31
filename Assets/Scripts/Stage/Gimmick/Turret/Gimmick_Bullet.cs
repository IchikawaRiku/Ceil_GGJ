using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾
/// </summary>
public class Gimmick_Bullet : MonoBehaviour {
    // 弾の速度
    private readonly float _BULLET_SPEED = 10;
    // 弾がきえるまでの時間
    private readonly float _LIFETIME = 0.3f;

    // 使用されているかどうか
    private bool _isActive = false;
    // 発射からの経過時間
    private float _timer = 0f;

    /// <summary>
    /// 弾の発射処理
    /// </summary>
    public void Fire() {
        _isActive = true;
        // 経過時間の初期化
        _timer = 0f;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (!_isActive) return;

        // 前方に移動
        transform.position += transform.forward * _BULLET_SPEED * Time.deltaTime;

        // 経過時間を加算
        _timer += Time.deltaTime;

        // 一定時間経過で非アクティブ化
        if (_timer >= _LIFETIME) {
            Deactivate();
        }

    }

    /// <summary>
    /// 弾を非アクティブに戻す
    /// </summary>
    public void Deactivate() {
        _isActive = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 障害物などに当たった時の処理
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        // タグで判定（障害物など）
        if (other.CompareTag("Obstacle")) {
            // 非アクティブにする
            Deactivate();
        }
    }

}
