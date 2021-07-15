using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Model.Artifacts
{
    
    public class A000_Test1 : Artifact
    {
        public A000_Test1()
        {
            Name = "test artifact name";
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
        }

        public int OnDamage(int value)
        {
            return value - 1;
        }

        public override void OnAddThisArtifact()
        {
            

            // base.OnAddThisArtifact();
        }

        public override void OnRemoveThisArtifact()
        {
            // base.OnRemoveThisArtifact();
        }

    }

}
