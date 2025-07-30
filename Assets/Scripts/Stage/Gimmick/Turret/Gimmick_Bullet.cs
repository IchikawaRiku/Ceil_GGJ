using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e
/// </summary>
public class Gimmick_Bullet : MonoBehaviour {
    // �e�̑��x
    private readonly float _BULLET_SPEED = 10;
    // �g�p����Ă��邩�ǂ���
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
        transform.position += transform.forward * _BULLET_SPEED * Time.deltaTime;

        
    }

    /// <summary>
    /// �e���A�N�e�B�u�ɖ߂�
    /// </summary>
    public void Deactivate() {
        isActive = false;
        gameObject.SetActive(false);
    }



}
