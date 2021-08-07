using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    public class Artifact : Effect, Obtainable
    {
        public enum ArtifactGrade { Common, Rare, Legend }
        public Vector2Int Position { get; set; }
        public ArtifactGrade Grade { get; set; }
        public int Price { get; set; }
        
        public static Dictionary<ArtifactGrade, Color> GradeToColor = new Dictionary<ArtifactGrade, Color>()
        {
            {ArtifactGrade.Common , Color.gray},
            {ArtifactGrade.Rare , Color.cyan},
            {ArtifactGrade.Legend , Color.green}
        };

        public void ToBag()
        {
            Managers.GameManager.Instance.artifactBag.Add(this);
            Debug.Log("GetArtifact");
        }
    }
}