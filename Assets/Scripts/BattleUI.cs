using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TileButton
{

}

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance;

    public GameObject moveTileButton;

    private void Start()
    {
        instance = this;
    }

    public void showTileButton(List<Vector2Int> positions)
    {

    }


}
