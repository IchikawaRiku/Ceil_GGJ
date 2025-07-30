using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾
/// </summary>
public class Gimmick_Bullet : MonoBehaviour {
    // 弾の速度
    private readonly float _BULLET_SPEED = 10;
    // 使用されているかどうか
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
        transform.position += transform.forward * _BULLET_SPEED * Time.deltaTime;

        
    }

    /// <summary>
    /// 弾を非アクティブに戻す
    /// </summary>
    public void Deactivate() {
        isActive = false;
        gameObject.SetActive(false);
    }



}
