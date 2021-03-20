using Model;
using System.Collections;
using System.Collections.Generic;
using UI.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<Unit, GameObject> unitObjects = new Dictionary<Unit, GameObject>();
        [SerializeField]
        private Dictionary<Unit, Slider> hpBars = new Dictionary<Unit, Slider>();
        [SerializeField]
        private GameObject hPBarPrefab;
        [SerializeField]
        private UnitInfoView thisTurnUnitInfo;
        [SerializeField]
        private UnitInfoView otherUnitInfo;

        public Button turnEndButton;


        public void AddHPBar(Unit unit)
        {

        }

    }
}