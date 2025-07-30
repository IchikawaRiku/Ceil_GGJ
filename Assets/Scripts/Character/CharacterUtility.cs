/*
 *  @file   CharacterUtility.cs
 *  @brief  �L�����N�^�[�֘A���s����
 *  @author Seki
 *  @date   2025/7/30
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUtility{
    /// <summary>
    /// �v���C���[�̐���
    /// </summary>
    public static void UsePlayer() {
        CharacterManager.instance.UsePlayer();
    }
    /// <summary>
    /// �v���C���[�̖��g�p��
    /// </summary>
    public static void UnusePlayer() {
        CharacterManager.instance.UnusePlayer();
    }
    /// <summary>
    /// �H��̐���
    /// </summary>
    public static void UseSpirit() {
        CharacterManager.instance.UseSpirit();
    }
    /// <summary>
    /// �H��̖��g�p��
    /// </summary>
    public static void UnuseSpirit() {
        CharacterManager.instance.UnuseSpirit();
    }
}
