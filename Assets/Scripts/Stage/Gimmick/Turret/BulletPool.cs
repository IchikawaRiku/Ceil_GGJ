using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾のプーリング
/// </summary>
public class BulletPool : MonoBehaviour {
    // 自身への参照
    public static BulletPool Instance { get; private set; }

    [SerializeField] private Gimmick_Bullet bulletPrefab;      // 弾のプレハブ
    private int initialPoolSize = 20; // 初期生成数

    private Queue<Gimmick_Bullet> bulletPool = new Queue<Gimmick_Bullet>();

    private void Awake() {
        Instance = this;

        // 初期弾生成
        for (int i = 0; i < initialPoolSize; i++) {
            CreateBullet();
        }
    }

    /// <summary>
    /// 弾を取得する（必要なら新しく生成）
    /// </summary>
    public Gimmick_Bullet GetBullet() {
        if (bulletPool.Count == 0) {
            CreateBullet();
        }

        return bulletPool.Dequeue();
    }

    /// <summary>
    /// 弾をプールに戻す
    /// </summary>
    public void ReturnBullet(Gimmick_Bullet bullet) {
        bullet.Deactivate();
        bulletPool.Enqueue(bullet);
    }

    /// <summary>
    /// 弾を生成してプールに追加
    /// </summary>
    private void CreateBullet() {
        Gimmick_Bullet newBullet = Instantiate(bulletPrefab, transform);
        newBullet.gameObject.SetActive(false);
        bulletPool.Enqueue(newBullet);
    }
}
