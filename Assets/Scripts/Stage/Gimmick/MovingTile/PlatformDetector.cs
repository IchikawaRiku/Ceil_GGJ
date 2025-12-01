/*
 *  @file   PlatformDetector.cs
 *  @brief  移動する床の乗り降りの検知
 *  @author oorui
 */

using UnityEngine;
using System;

public class PlatformDetector : MonoBehaviour {
    public event Action<Rigidbody> OnDetectedEnter;
    public event Action<Rigidbody> OnDetectedExit;

    [SerializeField] private string targetLayerName = "Player";     // 乗せる対象

    /// <summary>
    /// プレイヤーが床に乗ったときRigidBodyを付与
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        if (IsTarget(other)) {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null) {
                OnDetectedEnter?.Invoke(rb);
            }
        }
    }

    /// <summary>
    /// プレイヤーが床から降りたときRigidBodyを取る
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other) {
        if (IsTarget(other)) {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null) {
                OnDetectedExit?.Invoke(rb);
            }
        }
    }

    /// <summary>
    /// プレイヤーが触れたらフラグを立てる
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool IsTarget(Collider other) {
        return other.gameObject.layer == LayerMask.NameToLayer(targetLayerName);
    }
}