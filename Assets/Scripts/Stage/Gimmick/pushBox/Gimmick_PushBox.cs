using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_PushBox : GimmickBase {
    // �����ꂽ�Ƃ��̑��x
    [SerializeField]
    private float pushForce = 5f;
    // Rigitbody
    private Rigidbody rb;

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        // Rigitbody�̎擾
        rb = GetComponent<Rigidbody>();

        // ��]���Œ艻
        rb.constraints = RigidbodyConstraints.FreezeRotation |
                         RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezePositionZ;
    }

    /// <summary>
    /// ������Ă���Ԃ̏���
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) {
        // Player�^�O�Ƃ̐ڐG���̂ݔ���
        if (!collision.collider.CompareTag("Player")) return;

        // �Փ˓_�̖@�����擾
        ContactPoint contact = collision.contacts[0];
        Vector3 normal = contact.normal;

        // X���̂ݓ����悤�ɂ���
        // X���̂�����x�̕�Ԃ��|���ĉ����₷������
        float xDot = Vector3.Dot(normal, Vector3.left);
        if (Mathf.Abs(xDot) < 0.7f) return;

        // X�������ɑ��x��^����
        float direction = Mathf.Sign(Vector3.Dot(collision.transform.position - transform.position, Vector3.right));
        rb.velocity = new Vector3(direction * pushForce, 0f, 0f);
    }

    /// <summary>
    /// ���ꂽ�Ƃ��̏���
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) {
        // �v���C���[�Ƃ̐ڐG�I�����ɒ�~
        if (collision.collider.CompareTag("Player")) {
            rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {

    }
}
