using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�̃v�[�����O
/// </summary>
public class BulletPool : MonoBehaviour {
    // ���g�ւ̎Q��
    public static BulletPool Instance { get; private set; }

    [SerializeField] private Gimmick_Bullet bulletPrefab;      // �e�̃v���n�u
    private int initialPoolSize = 20; // ����������

    private Queue<Gimmick_Bullet> bulletPool = new Queue<Gimmick_Bullet>();

    private void Awake() {
        Instance = this;

        // �����e����
        for (int i = 0; i < initialPoolSize; i++) {
            CreateBullet();
        }
    }

    /// <summary>
    /// �e���擾����i�K�v�Ȃ�V���������j
    /// </summary>
    public Gimmick_Bullet GetBullet() {
        if (bulletPool.Count == 0) {
            CreateBullet();
        }

        return bulletPool.Dequeue();
    }

    /// <summary>
    /// �e���v�[���ɖ߂�
    /// </summary>
    public void ReturnBullet(Gimmick_Bullet bullet) {
        bullet.Deactivate();
        bulletPool.Enqueue(bullet);
    }

    /// <summary>
    /// �e�𐶐����ăv�[���ɒǉ�
    /// </summary>
    private void CreateBullet() {
        Gimmick_Bullet newBullet = Instantiate(bulletPrefab, transform);
        newBullet.gameObject.SetActive(false);
        bulletPool.Enqueue(newBullet);
    }
}
