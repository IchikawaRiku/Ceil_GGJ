using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^���b�g
/// </summary>
public class Gimmick_Turret : GimmickBase {
    [SerializeField] private Transform firePoint;        // �e�̔��ˈʒu
    [SerializeField] private float fireInterval = 1f;    // ���˃��[�g

    private float _timer;

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        // ���Ԃ�������
        _timer = 0f;
    }
    
    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        // ���Ԍo�߂��X�V
        _timer += Time.deltaTime;

        // ���ˊԊu�𒴂�����e�𔭎�
        if (_timer >= fireInterval) {
            Fire();
            _timer = 0f;
        }
    }

    /// <summary>
    /// �e�𔭎˂��鏈��
    /// </summary>
    private void Fire() {
        // BulletPool����e���擾���AfirePoint�̈ʒu�Ɖ�]�Ŕ���
        var bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.Fire();
    }
}
