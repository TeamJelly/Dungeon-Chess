namespace Model.Artifacts.Rare
{
    public class Diamond : Artifact
    {
        protected int increasingValue;
        public Diamond()
        {
            Name = "Diamond";
            Grade = ArtifactGrade.Rare;
            Description = "골드 +200";

            SpriteNumber = 214;
            InColor = UnityEngine.Color.cyan;
            OutColor = UnityEngine.Color.clear;
        }
        bool ArtifactFunction(bool b)
        {
            //기능 구현
            return b;
        }
    }
}