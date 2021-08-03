using Model;
using UnityEngine;

// 스프라이트화 할수 있는 클래스에 붙혀줍시다.
public interface Spriteable
{
    Sprite Sprite{get;}
    Color Color{get;}
}