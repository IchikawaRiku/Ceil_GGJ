using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タレットギミック：一定間隔で弾を発射する
/// </summary>
public class Gimmick_Turret : GimmickBase, IDisablable {
    [SerializeField] private Transform firePoint;             // 弾の発射位置
    [SerializeField] private float fireInterval = 0.2f;       // 発射レート
    [SerializeField] private float detectionRange = 10f;      // 撃ち始める距離
    [SerializeField] private Transform playerTransform;       // プレイヤーのTransform
    [SerializeField] private BulletPool bulletPool;           // 使用するBulletPool（ステージごとに個別）

    private float _timer;                                     // 発射間隔用タイマー
    private bool _canFire = true;                             // 発射可能フラグ

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize() {
        _timer = 0f;
        _canFire = true;
    }

    /// <summary>
    /// 使用前準備
    /// </summary>
    public override void SetUp() {
        _canFire = true;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
        if (!_canFire) return;

        // プレイヤーの座標を取得
        Vector3 playerPos = CharacterUtility.GetPlayerPosition();
        if (playerPos == Vector3.negativeInfinity) return;

        // プレイヤーが近くにいるか
        float distance = Vector3.Distance(transform.position, playerPos);
        if (distance > detectionRange) return;

        _timer += Time.deltaTime;

        // 発射間隔が経過したら撃つ
        if (_timer >= fireInterval) {
            Fire();
            _timer = 0f;
        }
    }

    /// <summary>
    /// 撃つ
    /// </summary>
    private void Fire() {
        if (bulletPool == null) return;

        var bullet = bulletPool.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        // 撃つ時に使用するプールを渡す
        bullet.Fire(bulletPool);
    }

    /// <summary>
    /// 撃たない
    /// </summary>
    public void Disable() {
        _canFire = false;
    }

    /// <summary>
    /// 片付け処理
    /// </summary>
    public override void Teardown() {
        _canFire = false;
    }
}