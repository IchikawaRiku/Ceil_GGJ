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

/// <summary>
/// ステージ列挙体
/// </summary>
public enum eStageStage {
    Invalid = -1,
    Tutorial,
    Stage1,
    Stage2,
    Stage3,

    Max
}
