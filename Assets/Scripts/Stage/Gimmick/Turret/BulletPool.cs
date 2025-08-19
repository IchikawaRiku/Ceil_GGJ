using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�̃v�[�����O�i�X�e�[�W���ƂɓƗ����đ��݁j
/// </summary>
public class BulletPool : MonoBehaviour {
    [SerializeField] private Gimmick_Bullet bulletPrefab;   // �e�̃v���n�u
    [SerializeField] private int initialPoolSize = 20;      // ����������

    private Queue<Gimmick_Bullet> bulletPool = new Queue<Gimmick_Bullet>();

    private void Awake() {
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