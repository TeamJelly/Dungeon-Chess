﻿namespace Model.Artifacts.Legend
{
    /// <summary>
    /// 이동력 증가 대 + 3
    /// </summary>
    public class HerosBoots : Artifact
    {
        protected int increasingValue = 3;
        public HerosBoots()
        {
            Name = "Heros Boots";
            Grade = ArtifactGrade.Legend;
            Description = "이동력 +3";

            SpriteNumber = 87;
            InColor = UnityEngine.Color.cyan;
            OutColor = UnityEngine.Color.clear;
        }

        public override void OnAdd()
        {
            Owner.Mobility += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Mobility -= increasingValue;
        }
    }
}