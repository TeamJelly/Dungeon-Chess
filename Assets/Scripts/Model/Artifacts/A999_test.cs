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
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
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
