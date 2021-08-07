using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Model.Artifacts
{
    
    public class test : Artifact
    {
        public test()
        {
            Name = "test";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Common;
        }

        public int OnDamage(int value)
        {
            return value - 1;
        }

        public override void OnAdd()
        {
            
        }

        public override void OnRemove()
        {

        }
    }

}
