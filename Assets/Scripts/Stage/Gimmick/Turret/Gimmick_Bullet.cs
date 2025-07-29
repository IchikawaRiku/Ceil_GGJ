using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e
/// </summary>
public class Gimmick_Bullet : MonoBehaviour {
    [SerializeField] private float speed = 10f;  // �e�̑��x

    private bool isActive = false;

    /// <summary>
    /// �e�̔��ˏ���
    /// </summary>
    public void Fire() {
        isActive = true;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (!isActive) return;

        // �O���Ɉړ�
        transform.position += transform.forward * speed * Time.deltaTime;

        // ��ʂ�������
    }

    /// <summary>
    /// �e���A�N�e�B�u�ɖ߂�
    /// </summary>
    public void Deactivate() {
        isActive = false;
        gameObject.SetActive(false);
    }
}
