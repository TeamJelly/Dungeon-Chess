using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitPosition
{
    public Vector2Int lowerLeft, upperRight;

    public UnitPosition()
    {
        this.lowerLeft = new Vector2Int();
        this.upperRight = new Vector2Int();
    }

    public UnitPosition(Vector2Int lowerLeft, Vector2Int upperRight)
    {
        this.lowerLeft = lowerLeft;
        this.upperRight = upperRight;
    }
    public void Set(UnitPosition newPosition)
    {
        lowerLeft.x = newPosition.lowerLeft.x;
        lowerLeft.y = newPosition.lowerLeft.y;
        upperRight.x = newPosition.upperRight.x;
        upperRight.y = newPosition.upperRight.y;
    }
    public void Up(int number)
    {
        lowerLeft.y += number;
        upperRight.y += number;
    }

    public void Down(int number)
    {
        lowerLeft.y -= number;
        upperRight.y -= number;
    }

    public void Right(int number)
    {
        lowerLeft.x += number;
        upperRight.x += number;
    }

    public void Left(int number)
    {
        lowerLeft.x -= number;
        upperRight.x -= number;
    }

    public static UnitPosition UpPosition(UnitPosition unitPosition, int number)
    {
        UnitPosition newPosition = new UnitPosition(unitPosition.lowerLeft, unitPosition.upperRight);
        newPosition.lowerLeft.y += number;
        newPosition.upperRight.y += number;
        return newPosition;
    }

    public static UnitPosition DownPosition(UnitPosition unitPosition, int number)
    {
        UnitPosition newPosition = new UnitPosition(unitPosition.lowerLeft, unitPosition.upperRight);
        newPosition.lowerLeft.y -= number;
        newPosition.upperRight.y -= number;
        return newPosition;
    }

    public static UnitPosition RightPosition(UnitPosition unitPosition, int number)
    {
        UnitPosition newPosition = new UnitPosition(unitPosition.lowerLeft, unitPosition.upperRight);
        newPosition.lowerLeft.x += number;
        newPosition.upperRight.x += number;
        return newPosition;
    }

    public static UnitPosition LeftPosition(UnitPosition unitPosition, int number)
    {
        UnitPosition newPosition = new UnitPosition(unitPosition.lowerLeft, unitPosition.upperRight);
        newPosition.lowerLeft.x -= number;
        newPosition.upperRight.x -= number;
        return newPosition;
    }

    public void ShowUnitPosition()
    {
        Debug.LogError("LowerLeft : " + lowerLeft + ", UpperRight : " + upperRight);
    }

    public static List<UnitPosition> GetNeighborPosition(UnitPosition unitPosition)
    {
        List<UnitPosition> temp = new List<UnitPosition>();

        if (UpPosition(unitPosition, 1).IsMovableUnitPosition())
            temp.Add(UpPosition(unitPosition, 1));
        if (DownPosition(unitPosition, 1).IsMovableUnitPosition())
            temp.Add(DownPosition(unitPosition, 1));
        if (RightPosition(unitPosition, 1).IsMovableUnitPosition())
            temp.Add(RightPosition(unitPosition, 1));
        if (LeftPosition(unitPosition, 1).IsMovableUnitPosition())
            temp.Add(LeftPosition(unitPosition, 1));

        return temp;
    }

    public bool IsMovableUnitPosition() // 사용 가능한 UnitPosition인가?
    {
        if (lowerLeft.x < 0 || lowerLeft.y < 0) // 맵 크기 안인가?
            return false;
        if (upperRight.x >= BattleManager.instance.AllTiles.GetLength(0) || upperRight.y >= BattleManager.instance.AllTiles.GetLength(1)) // 맵 크기 안인가?
            return false;

        for (int i = lowerLeft.x; i <= upperRight.x; i++)
            for (int j = lowerLeft.y; j <= upperRight.y; j++)
                if (!BattleManager.instance.AllTiles[i, j].IsUsable()) // 사용 가능한 타일인가?
                    return false;

        return true;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        UnitPosition temp = (UnitPosition)obj;

        if (lowerLeft.Equals(temp.lowerLeft) && upperRight.Equals(temp.upperRight))
            return true;

        return false;
    }

    public override int GetHashCode()
    {
        int hashCode = -750348174;
        hashCode = hashCode * -1521134295 + lowerLeft.GetHashCode();
        hashCode = hashCode * -1521134295 + upperRight.GetHashCode();
        return hashCode;
    }
}

