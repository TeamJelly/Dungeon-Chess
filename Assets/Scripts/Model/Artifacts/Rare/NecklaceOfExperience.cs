﻿namespace Model.Artifacts.Rare
{
    public class NecklaceOfExperience : Artifact
    {
        public NecklaceOfExperience()
        {
            Name = "경험의 목걸이";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Normal;
            Description = "경험치 획득량 +10%";
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
        bool ArtifactFunction(bool b)
        {
            //기능 구현
            return b;
        }
    }
}