using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Model.Artifacts
{
    
    public class A999_test : Artifact
    {
        public A999_test()
        {
            Name = "test artifact name";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
        }

        public int OnDamage(int value)
        {
            return value - 1;
        }

        public override void OnAddThisEffect()
        {

        }

        public override void OnRemoveThisEffect()
        {

        }

    }

}
