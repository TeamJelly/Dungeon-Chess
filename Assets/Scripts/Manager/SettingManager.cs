using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{

    #region 개발자 Flag

    public bool inBattle; // 전투상태 or 비전투상태
    public bool inAuto;   // 자동전투 or 수동전투
    public float AnimationDelaySpeed = 0.5f;

    #endregion


    #region 유저 Flag

    public float SoundVolume = 0.5f;

    #endregion
}