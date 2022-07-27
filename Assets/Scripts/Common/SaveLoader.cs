/// <summary>
/// 세이브와 로드와 관련된 코드가 있다.
/// </summary>

using UnityEngine;

public static class SaveLoader
{
    // public static void Save_Unit_Serializable_Data(this Unit unit)
    // {
    //     DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Data/Unit/");
    //     if (!directoryInfo.Exists) directoryInfo.Create();

    //     string jsonStr = JsonUtility.ToJson(unit.Get_Serializable());
    //     File.WriteAllText(Application.dataPath + "/Resources/Data/Unit/" + unit.Name + ".json", jsonStr);
    // }

    // public static Unit_Serializable Load_Unit_Serializable_Data(this string dataPath)
    // {

    //     string jsonStr = File.ReadAllText(dataPath);
    //     jsonStr = jsonStr.Replace("\n", "");
    //     Unit_Serializable u = JsonUtility.FromJson<Unit_Serializable>(jsonStr);
    //     return u;
    // }

    // public static void SaveScene(string name)
    // {
    //     DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Data/Scene/");
    //     if (!directoryInfo.Exists) directoryInfo.Create();

    //     SceneData sceneData = new SceneData();

    //     List<Obtainable> obtainableList = Model.Managers.BattleManager.instance.AllObtainables;
    //     List<Unit> unitList = Model.Managers.BattleManager.instance.AllUnits;

    //     sceneData.fieldData = Model.Managers.FieldManager.instance.GetFieldData();

    //     sceneData.obtainables = (from obt in obtainableList
    //                              select (obt.GetType().ToString(), ((Vector2Int)obt.Position).x, ((Vector2Int)obt.Position).y)).ToArray();

    //     sceneData.units = (from unit in unitList
    //                        select (unit.Get_Serializable(), unit.Position.x, unit.Position.y)).ToArray();


    //     string jsonStr = JsonConvert.SerializeObject(sceneData, Formatting.Indented);
    //     Debug.Log("Saved");
    //     File.WriteAllText(Application.dataPath + "/Resources/Data/Scene/" + name + ".json", jsonStr);
    // }

    // public static void LoadScene(string dataPath)
    // {
    //     string jsonStr = File.ReadAllText(dataPath);
    //     jsonStr = jsonStr.Replace("\n", "");

    //     SceneData sceneData = JsonConvert.DeserializeObject<SceneData>(jsonStr);

    //     List<Unit> unitList = new List<Unit>();

    //     bool inBattle = Model.Managers.GameManager.InBattle;

    //     Command.UnSummonAllUnit();
    //     Command.UnSummonAllObtainable();

    //     Model.Managers.FieldManager.instance.InitField(sceneData.fieldData);

    //     foreach ((Unit_Serializable unit, int x, int y) in sceneData.units)
    //     {
    //         Unit u = new Unit(unit);
    //         Command.Summon(u, new Vector2Int(x, y));
    //         if (u.Alliance == UnitAlliance.Party) Model.Managers.GameManager.AddPartyUnit(u);
    //         //unitList.Add(u);
    //     }

    //     foreach ((string name, int x, int y) in sceneData.obtainables)
    //     {
    //         Obtainable obt = (Obtainable)Activator.CreateInstance(Type.GetType(name));
    //         Command.Summon(obt, new Vector2Int(x, y));
    //     }

    //     Model.Managers.BattleManager.instance.InitializeUnitBuffer();

    //     // 일단 배틀모드 해제.
    //     UI.Battle.BattleController.SetBattleMode(false);
    //     // UI.Battle.BattleController.instance.NextTurnStart();
    // }
}