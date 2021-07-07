using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [System.Serializable]
    public class Artifact
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }

        public virtual void OnAddThisArtifact()
        {

        }

        public virtual void OnRemoveThisArtifact()
        {

        }
    }

}