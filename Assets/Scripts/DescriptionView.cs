// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using Model;

// namespace View
// {
//     //유닛 누르면 팝업 형식으로 뜨는 ui
//     public class DescriptionView : MonoBehaviour
//     {
//         public Text text;
//         public static DescriptionView instance;
//         //여기에 유물정보 있으면 좋을 것 같음.

//         private void Awake()
//         {
//             instance = this;
//             gameObject.SetActive(false);
//         }
        
//         public void Show(Skill skill)
//         {

//         }

//         public void Show(Unit unit)
//         {
//             text.text =
//                 "이름: " + unit.Name +
//                 "\nstrength: " + unit.Strength;
//             gameObject.SetActive(true);
//         }

//         public void Disable()
//         {
//             gameObject.SetActive(false);
//         }
//     }
// }