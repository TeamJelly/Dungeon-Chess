using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    [System.Serializable]
    public class Artifact : Obtainable
    {
        public Effect antiqueEffect;

        GameObject gameObject;
       
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }

        protected string spritePath;

        private Sprite sprite;

        public Sprite Sprite
        {
            set => sprite = value;
            get
            {
                if (sprite == null)
                {
                    sprite = Common.Data.LoadSprite(spritePath);
                }
                return sprite;
            }
        }

        public virtual void OnAddThisArtifact()
        {


        }

        public virtual void OnRemoveThisArtifact()
        {

        }

        public void AssignTo(Unit unit)
        {
            Debug.Log(Name + "을 " + unit.Name + "에게 할당");
            unit.Antiques.Add(this);
        }

        public void DropImage(Vector2Int pos)
        {
            // 게임 오브젝트 생성
            gameObject = new GameObject(Name);
            // 위치 지정
            gameObject.transform.position = new Vector3(pos.x, pos.y, -0.1f);

            // 스프라이터 랜더러 추가
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite;
        }
        public void DeleteImage()
        {
            GameObject.Destroy(gameObject);
        }
    }

}