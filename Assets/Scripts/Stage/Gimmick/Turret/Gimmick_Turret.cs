using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^���b�g�M�~�b�N�F���Ԋu�Œe�𔭎˂���
/// </summary>
public class Gimmick_Turret : GimmickBase, IDisablable {
    [SerializeField] private Transform firePoint;        // �e�̔��ˈʒu
    private float fireInterval = 0.1f;    // ���˃��[�g

    private float _timer;
    private bool _canFire = true;                        // ���ˉ\�t���O

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        _timer = 0f;
        _canFire = true; // ������ԂŔ��ˉ\�ɂ���
    }

    /// <summary>
    /// ���܂͂���Ȃ�
    /// </summary>
    public override void SetUp() {}

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        // ���˒�~����Ă����猂���Ȃ�
        if (!_canFire) return;

        // �o�ߎ��Ԃ̍X�V
        _timer += Time.deltaTime;

        if (_timer >= fireInterval) {
            // ����
            Fire();
            // �o�ߎ��ԏ�����
            _timer = 0f;
        }
    }

    /// <summary>
    /// �e�𔭎˂��鏈��
    /// </summary>
    private void Fire() {
        var bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.Fire();
    }

    /// <summary>
    /// �^���b�g�̒e�̔��˂��~����
    /// </summary>
    public void Disable() {
        _canFire = false;
    }
}