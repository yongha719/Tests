using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// 반드시 Scroll Rect 컴포넌트가 있는 개체에 넣어야함
public class ScrollUI : MonoBehaviour
{
    private ScrollRect scrollRect;

    [SerializeField]
    private GridLayoutGroup layoutGroup;

    [SerializeField]
    private List<RectTransform> Contents = new();

    [SerializeField]
    private Queue<RectTransform> hiddenContentsQueue = new();

    private int ContentSizeY = 290;

    private int layoutSpacingY = 20;

    private const int CONTENTS_HORIZONTAL_CHILD_COUNT = 6;

    private int prevScrollPosY = 0;

    private int curHiddenRowCount = 0;

    public Vector3[] scrollRectCorners = new Vector3[4];

    [SerializeField, Tooltip("스크롤 렉트 위 Y값")]
    private int ScrollRectTopY;
    [SerializeField, Tooltip("스크롤 렉트 밑 Y값")]
    private int ScrollRectBottomY;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        ContentSizeY = (int)layoutGroup.cellSize.y;
        layoutSpacingY = (int)layoutGroup.spacing.y;

        var layoutRect =  transform.GetChild(0) as RectTransform;

        print(layoutRect.name);
        layoutRect.GetWorldCorners(scrollRectCorners);

        ScrollRectTopY = (int)scrollRectCorners.Max(corner => corner.y);
        ScrollRectBottomY = (int)scrollRectCorners.Min(corner => corner.y);

        for (int i = 0; i < layoutGroup.transform.childCount; i++)
        {
            Contents.Add(layoutGroup.transform.GetChild(i) as RectTransform);
        }
    }

    private void FixedUpdate()
    {
        foreach (var child in Contents)
        {
            Vector3[] corners = new Vector3[4];
            child.GetWorldCorners(corners);

            // 리스트나 아이템이 화면 상단 및 하단 밖으로 나가면 비활성화합니다.
            if (corners[1].y > ScrollRectTopY || corners[0].y < ScrollRectBottomY)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void Scroll()
    {
        int scrollPosY = (int)layoutGroup.transform.localPosition.y;

        int hiddenRowCount = Mathf.Abs(scrollPosY / (ContentSizeY + layoutSpacingY)); // 안보이는 콘텐츠 줄 count

        if (curHiddenRowCount == hiddenRowCount)
            return;

        // 밑으로 스크롤할 때
        if (scrollPosY > prevScrollPosY)
        {
            hiddenRowCount = curHiddenRowCount + hiddenRowCount;

            int hiddenRowContentsCount = (hiddenRowCount * CONTENTS_HORIZONTAL_CHILD_COUNT); // 한줄이 다 차있다고 가정할 때 몇개의 콘텐츠가 있는지

            var contentsCount = hiddenRowContentsCount < Contents.Count ? hiddenRowContentsCount : Contents.Count;

            curHiddenRowCount = (int)Mathf.Ceil((float)contentsCount / CONTENTS_HORIZONTAL_CHILD_COUNT);

            foreach (var content in Contents)
                content.gameObject.SetActive(true);

            for (int i = 0; i < contentsCount; i++)
            {
                var content = Contents[i];

                content.gameObject.SetActive(false);
                hiddenContentsQueue.Enqueue(content);
            }

            // 없어진 행만큼 포지션 조절
            layoutGroup.transform.localPosition -= new Vector3(0, (scrollPosY / (ContentSizeY + layoutSpacingY) * hiddenRowCount));
        }
        // 위로 스크롤 할 때 계산
        else
        {
            hiddenRowCount = curHiddenRowCount + hiddenRowCount;

            int visibleRowCount = (int)Mathf.Ceil((float)Contents.Count / CONTENTS_HORIZONTAL_CHILD_COUNT) - hiddenRowCount;

            int hiddenRowContentsCount = Contents.Count - (visibleRowCount * 6);

            int contentsCount = hiddenRowContentsCount < Contents.Count ? hiddenRowContentsCount : Contents.Count;

            curHiddenRowCount = (int)Mathf.Ceil((float)contentsCount / CONTENTS_HORIZONTAL_CHILD_COUNT);

            foreach (var content in Contents)
                content.gameObject.SetActive(true);

            int startIndex = (visibleRowCount * 6) + 1;

            for (int i = startIndex + 1; i < contentsCount; i++)
            {
                Contents[i].gameObject.SetActive(false);

            }
        }

        prevScrollPosY = scrollPosY;
    }
}

