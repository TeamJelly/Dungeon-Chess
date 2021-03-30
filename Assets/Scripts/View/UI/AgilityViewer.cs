using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgilityViewer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public RectTransform line;
    Dictionary<Unit, RectTransform> finder = new Dictionary<Unit, RectTransform>();
    List<RectTransform> unitObjects = new List<RectTransform>();

    public static AgilityViewer instance;
    private void Awake()
    {
        instance = this;

    }
    public void AddToList(Unit unit)
    {
        RectTransform newObj = Instantiate(prefab,line).transform as RectTransform;

        newObj.GetComponent<Image>().sprite = unit.Portrait;
        finder.Add(unit, newObj);
        unitObjects.Add(newObj);
        newObj.gameObject.SetActive(true);
        newObj.gameObject.name = unit.Name;
    }

    public void UpdteView()
    {

        List<Vector2> a = new List<Vector2>();

        foreach (Unit unit in finder.Keys)
        {
            a.Add(finder[unit].anchoredPosition);
           // StartCoroutine(ShowAnimation(unit));
        }
        //1. 유닛의 누적 행동력 값들을 정렬시킴
        //2. 행동력마다 대응되는 오브젝트들을 해당 행동력 인덱스 위치를 애니메이션을 보여주며 이동.
    }

    public void DestroyObject(Unit unit)
    {
        RectTransform unitObj = finder[unit];
        finder.Remove(unit);
        unitObjects.Remove(unitObj);
        Destroy(unitObj.gameObject);
    }

    public IEnumerator ShowAnimation(Unit unit)
    {
        Application.targetFrameRate = 60;
        RectTransform transform = finder[unit];

        Vector2 start = transform.anchoredPosition;
        Vector2 end = new Vector2(line.rect.width * unit.ActionRate/100, 0);

        float temp = 0;
        float animationTime = 0.8f;
        while (temp < 1)
        {
            temp += Time.deltaTime / animationTime;
            float sin = Mathf.Sin(Mathf.Deg2Rad * temp * 90);
           // transform.anchoredPosition = start + (end - start) * sin;//Vector2.Lerp(start, end, sin);
            transform.anchoredPosition = Vector2.Lerp(start, end, temp * temp * (3f - 2f * temp));

            yield return null;
        }
        transform.anchoredPosition = end;
    }
}
