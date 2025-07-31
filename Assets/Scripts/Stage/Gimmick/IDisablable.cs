
/// <summary>
/// 停止可能なギミックに共通するインターフェース
/// </summary>
public interface IDisablable {
    
    // ギミックを停止する
    void Disable();
}


/// <summary>
/// 破壊可能なギミックに共通するインターフェース
/// </summary>
public interface IDestroyable {
    // 破壊する
    void DestroyGimmick();
}

/// <summary>
/// 可視状態を切り替え可能なギミックに共通するインターフェース
/// </summary>
public interface IVisibleToggleable {
    // みちゃう
    void ToggleVisibility();
}