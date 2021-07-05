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
        public ObjectInfo info = new ObjectInfo();

        GameObject gameObject;
        public void AssignTo(Unit unit)
        {
            Debug.Log(info.name + "을 " + unit.Name +"에게 할당");
            unit.Antiques.Add(this);
        }
        public void DropImage(Vector2Int pos)
        {
            // 게임 오브젝트 생성
            gameObject = new GameObject(info.name);
            // 위치 지정
            gameObject.transform.position = new Vector3(pos.x, pos.y, -0.1f);

            // 스프라이터 랜더러 추가
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = info.Sprite;
        }
        public void DeleteImage()
        {
            GameObject.Destroy(gameObject);
        }


    }
}