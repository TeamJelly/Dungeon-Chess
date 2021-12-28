using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace Model
{
    public class Artifact : Effect, Obtainable
    {
        public enum ArtifactGrade { Normal, Rare, Legend }
        public Vector2Int Position { get; set; }
        public ArtifactGrade Grade { get; set; }
        public int Price { get; set; }
        public override string Type => "Artifact";

        public Sprite sprite;

        public Artifact() { }
        public override Sprite Sprite { 
            get 
            {
                if (Color != new Color(0,0,0,0))
                    return Common.Data.MakeOutline(sprite, Color, Artifact.GradeToColor[Grade]); 
                else
                    return Common.Data.MakeOutline(sprite, Artifact.GradeToColor[Grade]);
            }            
            set => sprite = value;
        }
        
        public override void OnOverlap(Effect oldEffect)
        {
            // nothing
        }

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
            //Clone<Artifacts.Normal.ACoin>();
        }
    }

    /*
             public Unit Owner { get; set; }
        public string Name { get; set; }
        public virtual Sprite Sprite { get; set; }
        public Color Color {get; set;}
        public string Description { get; set; }

        private int turnCount;
        public int TurnCount
        {
            get => turnCount;
            set => turnCount = value;
        }*/
}