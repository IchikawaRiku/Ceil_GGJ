using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タレット
/// </summary>
public class Gimmick_Turret : GimmickBase {
    [SerializeField] private Transform firePoint;        // 弾の発射位置
    [SerializeField] private float fireInterval = 1f;    // 発射レート

    private float _timer;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        // 時間を初期化
        _timer = 0f;
    }
    
    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        // 時間経過を更新
        _timer += Time.deltaTime;

        // 発射間隔を超えたら弾を発射
        if (_timer >= fireInterval) {
            Fire();
            _timer = 0f;
        }
    }

    /// <summary>
    /// 弾を発射する処理
    /// </summary>
    private void Fire() {
        // BulletPoolから弾を取得し、firePointの位置と回転で発射
        var bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.Fire();
    }
}
