using Common.UI;
using Model;
using Model.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using View;

public class AgilityView : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform prefab;
    public RectTransform line;
    List<RectTransform> unitObjects = new List<RectTransform>();

    int bufferSize = 0;
    float imageGap = 0;

    public static AgilityView instance;
    private void Awake()
    {
        instance = this;

    }
    public void Init(int _bufferSize)
    {
        bufferSize = _bufferSize + 1;
        imageGap = line.rect.width / bufferSize;
        for(int i = 0; i < bufferSize; i++)
        {
            RectTransform newObj = Instantiate(prefab, line);
            unitObjects.Add(newObj);
            newObj.anchoredPosition = new Vector2(i * imageGap, 0);
            UIEffect.FadeInPanel(newObj.gameObject);
        }
    }

    public void UpdteView()
    {
        StartCoroutine(ShowAnimation());
        //1. 유닛의 누적 행동력 값들을 정렬시킴
        //2. 행동력마다 대응되는 오브젝트들을 해당 행동력 인덱스 위치를 애니메이션을 보여주며 이동.
    }

    public IEnumerator ShowAnimation()
    {
        bool turnEndInteractable = ViewManager.battle.TurnEndButton.interactable;
        ViewManager.battle.TurnEndButton.interactable = false;
        RectTransform firstObj = unitObjects[0];
        UIEffect.FadeOutPanel(firstObj.gameObject);
        unitObjects.Remove(firstObj);

        for(int i = 0; i < unitObjects.Count; i++)
        {
            StartCoroutine(MoveImagies(unitObjects[i], i));
        }

        yield return new WaitForSeconds(0.8f);

        firstObj.anchoredPosition = Vector2.right * imageGap * (bufferSize - 1);
        unitObjects.Add(firstObj);

        List<Unit> unitBuffer = BattleManager.instance.UnitBuffer.ToList();
        unitObjects[0].GetComponent<Image>().sprite = BattleManager.instance.thisTurnUnit.Portrait;
        for (int i = 1; i < bufferSize; i++)
        {
            unitObjects[i].GetComponent<Image>().sprite = unitBuffer[i - 1].Portrait;
        }
       
        UIEffect.FadeInPanel(firstObj.gameObject);
        ViewManager.battle.TurnEndButton.interactable = turnEndInteractable;
    }

    IEnumerator MoveImagies(RectTransform transform, int nextIndex)
    {
        Vector2 start = Vector2.right * imageGap * (nextIndex + 1); //transform.anchoredPosition;
        Vector2 end = Vector2.right * imageGap * nextIndex;

        float temp = 0;
        float animationTime = 0.8f;
        while (temp < 1)
        {
            temp += Time.deltaTime / animationTime;
            // transform.anchoredPosition = start + (end - start) * sin;//Vector2.Lerp(start, end, sin);
            transform.anchoredPosition = Vector2.Lerp(start, end, temp * temp * (3f - 2f * temp));
            yield return null;
        }
        transform.anchoredPosition = end;
    }

   /* public IEnumerator ShowAnimation(Unit unit, Vector2 end)
    {
        Application.targetFrameRate = 60;

        Vector2 start = transform.anchoredPosition;
        //Vector2 end = new Vector2(line.rect.width * unit.ActionRate/100, 0);

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
    }*/
}
