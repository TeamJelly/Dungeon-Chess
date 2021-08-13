using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    public class Artifact : Effect, Obtainable
    {
        public enum ArtifactGrade { Normal, Rare, Legend }
        public Vector2Int Position { get; set; }
        public ArtifactGrade Grade { get; set; }
        public int Price { get; set; }
        
        public static Dictionary<ArtifactGrade, Color> GradeToColor = new Dictionary<ArtifactGrade, Color>()
        {
            {ArtifactGrade.Normal , new Color(0.5f,0.5f,0.5f,0.8f)},
            {ArtifactGrade.Rare , new Color(8/255f,209/255f,176/255f,0.8f)},
            {ArtifactGrade.Legend , new Color(255/255f,0/255f,132/255f,0.8f)}
        };

        public void BelongTo(Unit unit)
        {
            Common.Command.AddArtifact(unit, this);
            Common.Command.UnSummon(this);
            //Managers.GameManager.Instance.artifactBag.Add(this);
            //Debug.Log("GetArtifact");
        }
    }
}