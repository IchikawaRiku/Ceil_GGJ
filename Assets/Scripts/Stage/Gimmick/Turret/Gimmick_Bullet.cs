using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾
/// </summary>
public class Gimmick_Bullet : MonoBehaviour {
    [SerializeField] private float speed = 10f;  // 弾の速度

    private bool isActive = false;

    /// <summary>
    /// 弾の発射処理
    /// </summary>
    public void Fire() {
        isActive = true;
        gameObject.SetActive(true);
    }

    private void Update() {
        if (!isActive) return;

        // 前方に移動
        transform.position += transform.forward * speed * Time.deltaTime;

        // 画面がい処理
    }

    /// <summary>
    /// 弾を非アクティブに戻す
    /// </summary>
    public void Deactivate() {
        isActive = false;
        gameObject.SetActive(false);
    }
}
