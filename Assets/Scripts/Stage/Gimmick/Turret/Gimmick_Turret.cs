using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^���b�g�M�~�b�N�F���Ԋu�Œe�𔭎˂���
/// </summary>
public class Gimmick_Turret : GimmickBase, IDisablable {
    [SerializeField] private Transform firePoint;             // �e�̔��ˈʒu
    [SerializeField] private float fireInterval = 0.2f;       // ���˃��[�g
    [SerializeField] private float detectionRange = 10f;      // �����n�߂鋗��
    [SerializeField] private Transform playerTransform;       // �v���C���[��Transform�iInspector�ŃZ�b�g or �����擾�j

    private float _timer;
    private bool _canFire = true;                             // ���ˉ\�t���O

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        _timer = 0f;
        _canFire = true;
    }

    /// <summary>
    /// �g�p�O����
    /// </summary>
    public override void SetUp() {
        _canFire = true;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        if (!_canFire) return;

        // �v���C���[�̍��W���擾
        Vector3 playerPos = CharacterUtility.GetPlayerPosition();
        if (playerPos == Vector3.negativeInfinity) return;
        // �v���C���[���߂��ɂ��邩
        float distance = Vector3.Distance(transform.position, playerPos);
        if (distance > detectionRange) return;

        _timer += Time.deltaTime;

        if (_timer >= fireInterval) {
            Fire();
            _timer = 0f;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Fire() {
        var bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.Fire();
    }


    /// <summary>
    /// �����Ȃ�
    /// </summary>
    public void Disable() {
        _canFire = false;
    }

    /// <summary>
    /// �Еt������
    /// </summary>
    public override void Teardown() {
        _canFire = true;
    }

}