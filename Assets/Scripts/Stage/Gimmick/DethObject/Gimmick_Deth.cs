using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using static MainGameProcessor;
/// <summary>
/// �������̎��S����
/// </summary>
public class Gimmick_Deth : GimmickBase {

    /// <summary>
    /// ����
    /// </summary>
    public override void SetUp() {
        base.SetUp();
    }

    /// <summary>
    /// �X�V(�g��Ȃ�)
    /// </summary>
    protected override void OnUpdate() {
    }


    /// <summary>
    /// ���S����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision) {
        // �v���C���[��������
        if (collision.gameObject.layer == 6) {
            // ���S�����MainGameProcessor�ɑ���
            EndGameReason(eEndReason.Dead);
        }
    }

}
