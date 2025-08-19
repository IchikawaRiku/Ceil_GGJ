using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e
/// </summary>
public class Gimmick_Bullet : MonoBehaviour {
    // �e�̑��x
    private readonly float _BULLET_SPEED = 10f;
    // �e��������܂ł̎���
    private readonly float _LIFETIME = 0.3f;

    // �g�p����Ă��邩�ǂ���
    private bool _isActive = false;
    // ���˂���̌o�ߎ���
    private float _timer = 0f;
    // �v�[���̌�
    private BulletPool _ownerPool;

    /// <summary>
    /// �e�̔��ˏ���
    /// </summary>
    public void Fire(BulletPool ownerPool) {
        _ownerPool = ownerPool;
        _isActive = true;
        // �o�ߎ��Ԃ̏�����
        _timer = 0f;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (!_isActive) return;

        // �O���Ɉړ�
        transform.position += transform.forward * _BULLET_SPEED * Time.deltaTime;

        // �o�ߎ��Ԃ����Z
        _timer += Time.deltaTime;

        // ��莞�Ԍo�߂Ŕ�A�N�e�B�u��
        if (_timer >= _LIFETIME) {
            Deactivate();
        }
    }

    /// <summary>
    /// �e���A�N�e�B�u�ɖ߂�
    /// </summary>
    public void Deactivate() {
        _isActive = false;
        gameObject.SetActive(false);

        // �v�[���ɖ߂�
        if (_ownerPool != null) {
            _ownerPool.ReturnBullet(this);
        }
    }

    /// <summary>
    /// ��Q���Ȃǂɓ����������̏���
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        // ��Q�������m
        if (other.CompareTag("Obstacle")) {
            // ��A�N�e�B�u�ɂ���
            Deactivate();
        }
    }
}