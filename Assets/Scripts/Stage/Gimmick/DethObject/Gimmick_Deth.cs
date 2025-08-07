using Cysharp.Threading.Tasks;
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
            // SE�Đ�
            UniTask task = SoundManager.instance.PlaySE(6);
            // ���S�����MainGameProcessor�ɑ���
            EndGameReason(eEndReason.Dead);
        }

        // �����锠�iPushBox�j�������烊�Z�b�g
        var pushBox = collision.GetComponent<Gimmick_PushBox>();
        if (pushBox != null) {
            pushBox.ResetBox();
        }

    }

}
