using UnityEngine;

interface IOnMove
{
    public void OnBeforeMove(Unit unit, ref Vector2Int target);
    public void OnAfterMove(Unit unit, ref Vector2Int target);
}