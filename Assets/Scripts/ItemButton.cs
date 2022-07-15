// using UnityEngine;
// using UnityEngine.EventSystems;
// using Model;
// using Model.Managers;

// namespace View.UI
// {

//     public class ItemButton : PixelButton
//     {

//         public Item CurrentItem => currentItem;
//         Item currentItem = null;

//         public void Init()
//         {
//             toggleOption = true;
//             OnPushButton = () => View.IndicatorView.ShowTileIndicator(FieldManager.instance.allTilesPosition, (v) =>
//             {
//                 currentItem.Use(FieldManager.GetTile(v));
//                 GameManager.Instance.itemBag.Remove(currentItem);

//                 UnitControlView.instance.UpdateItemButtons();
//                 IndicatorView.HideTileIndicator();
//             });
//             OnPopButton = () => View.IndicatorView.HideTileIndicator();
//         }
//         public override void OnPointerDown(PointerEventData eventData)
//         {
//             base.OnPointerDown(eventData);
//             if (!properties.interactable) return;
//             // 상호가능함을 반전시킨다.
//             UnitControlView.instance.ToggleAllButtons();

//             // 나 자신은 상호 가능하게 한다.
//             SetInteractable(true);
//         }

//         public void SetItem(Item item)
//         {
//             currentItem = item;

//             if (item == null)
//             {
//                 MainImage.sprite = null; // 나중에 빈 이미지로 교체하기
//                 MainImage.color = Color.clear;
//                 return;
//             }

//             MainImage.sprite = currentItem.Sprite;
//             MainImage.color = Color.white;
//         }

//         public override void SetInteractable(bool boolean)
//         {
//             properties.interactable = boolean && CurrentItem != null;
//             FrameImage.color = properties.interactable ? Color.white : Color.grey;
//         }

//     }

// }