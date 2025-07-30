using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タレットギミック：一定間隔で弾を発射する
/// </summary>
public class Gimmick_Turret : GimmickBase, IDisablable {
    [SerializeField] private Transform firePoint;        // 弾の発射位置
    private float fireInterval = 0.1f;    // 発射レート

    private float _timer;
    private bool _canFire = true;                        // 発射可能フラグ

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        _timer = 0f;
        _canFire = true; // 初期状態で発射可能にする
    }

    /// <summary>
    /// いまはつかわない
    /// </summary>
    public override void SetUp() {}

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        // 発射停止されていたら撃たない
        if (!_canFire) return;

        // 経過時間の更新
        _timer += Time.deltaTime;

        if (_timer >= fireInterval) {
            // 発射
            Fire();
            // 経過時間初期化
            _timer = 0f;
        }
    }

    /// <summary>
    /// 弾を発射する処理
    /// </summary>
    private void Fire() {
        var bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.Fire();
    }

    /// <summary>
    /// タレットの弾の発射を停止する
    /// </summary>
    public void Disable() {
        _canFire = false;
    }
}