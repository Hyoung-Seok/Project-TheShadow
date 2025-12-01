using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PaletteGUI
{
    private readonly List<GameObject> _paletteItemList;
    private readonly ListView _listView;
    private const float LISTVIEW_INTERVAL = 150f;

    public PaletteGUI(ListView listView)
    {
        _paletteItemList = new List<GameObject>();
        _listView = listView;
    }

    public void SetupListView()
    {
        // 데이터 목록 등록 -> 데이터를 볼 땐 해당 변수에 저장된 데이터 확인
        _listView.itemsSource = _paletteItemList;
        
        // 아이템 생성 -> UI가 하나 필요할 때 해당 템플릿으로 생성
        _listView.makeItem = () => _listView.itemTemplate.CloneTree();
        
        // 실제 데이터 연결
        _listView.bindItem = (element, index) =>
        {
            var obj = _paletteItemList[index];

            var objPreview = element.Q<VisualElement>("ObjectPreview");
            var selectBtn = element.Q<Button>("SelectBtn");
            var removeBtn = element.Q<Button>("RemoveBtn");

            objPreview.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
            objPreview.style.backgroundImage 
                = ObjectPreview.GetObjectPreview(obj, 150, 100);
            
            selectBtn.clicked -= selectBtn.userData as Action;
            removeBtn.clicked -= removeBtn.userData as Action;
            
            // TODO : 버튼 클릭 함수 등록
            selectBtn.clicked += OnClickedSelectBtn;

            Action removeAction = () => OnClickedRemoveBtn(index);
            removeBtn.userData = removeAction;
            removeBtn.clicked += removeAction;
        };

        _listView.fixedItemHeight = LISTVIEW_INTERVAL;
    }

    public void CreatePaletteItem(GameObject obj)
    {
        _paletteItemList.Add(obj);
        _listView.Rebuild();
    }

    private void OnClickedSelectBtn()
    {
        
    }

    private void OnClickedRemoveBtn(int index)
    {
        _paletteItemList.RemoveAt(index);
        _listView.Rebuild();
    }
}
