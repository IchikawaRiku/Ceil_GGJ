/*
 *  @file   GameEnum.cs
 *  @brief  列挙体宣言
 *  @author Seki
 *  @date   2025/7/29
 */

/// <summary>
/// ゲームパート列挙体
/// </summary>
public enum eGamePart {
    Invalid = -1,
    Stanby,
    Title,
    MainGame,
    Ending,

    Max
}
/// <summary>
/// フェード状態列挙体
/// </summary>
public enum eFadeState {
    Invalid = -1,
    FadeIn,
    FadeOut
}
