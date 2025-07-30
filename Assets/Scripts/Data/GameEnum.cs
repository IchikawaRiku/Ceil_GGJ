/*
 *  @file   GameEnum.cs
 *  @brief  �񋓑̐錾
 *  @author Seki
 *  @date   2025/7/29
 */

/// <summary>
/// �Q�[���p�[�g�񋓑�
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
/// �t�F�[�h��ԗ񋓑�
/// </summary>
public enum eFadeState {
    Invalid = -1,
    FadeIn,
    FadeOut
}

/// <summary>
/// �X�e�[�W�񋓑�
/// </summary>
public enum eStageStage {
    Invalid = -1,
    Tutorial,
    Stage1,
    Stage2,
    Stage3,

    Max
}
