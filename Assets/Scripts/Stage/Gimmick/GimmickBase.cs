using UnityEngine;

/// <summary>
/// ステージギミックの基底クラス
/// </summary>
public abstract class GimmickBase : MonoBehaviour {
    /// <summary>
    /// 初期化処理
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected virtual void Update() {
        OnUpdate();
    }

    /// <summary>
    /// 派生先での更新処理
    /// </summary>
    protected abstract void OnUpdate();
}