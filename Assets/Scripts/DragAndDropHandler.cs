using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDropHandler : MonoBehaviour
{
    [SerializeField] private UIItemSlot cursorSlot = null;
    private ItemSlot cursorItemSlot;
    
    [SerializeField] private GraphicRaycaster m_Raycaster = null;
    private PointerEventData m_PointerEventData;

    [SerializeField] private EventSystem m_Eventsystem = null;

    World world;

    private void Start() {

        world = GameObject.Find("World").GetComponent<World>();

        cursorItemSlot = new ItemSlot(cursorSlot);
    }
    private void Update()
    {
        if (!world.inUI) {
            return;
        }
        cursorSlot.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0)) {
            HandleSlotClick(CheckForSlot());
        }
    }

    private void HandleSlotClick(UIItemSlot clickedslot) {
        if (clickedslot == null) {
            cursorSlot.itemSlot.EmptySlot();
            return;
        }
        if (!cursorSlot.HasItem && !clickedslot.HasItem) {
            return;
        }
        if (clickedslot.itemSlot.isCreative) {
            cursorItemSlot.EmptySlot();
            cursorItemSlot.InsertStack(clickedslot.itemSlot.stack);
        }
        if (!cursorSlot.HasItem && clickedslot.HasItem) {
            cursorItemSlot.InsertStack(clickedslot.itemSlot.TakeAll());
            cursorSlot.UpdateSlot();
            return;
        }
        if (cursorSlot.HasItem && !clickedslot.HasItem) {
            clickedslot.itemSlot.InsertStack(cursorItemSlot.TakeAll());
            clickedslot.UpdateSlot();
            return;
        }
        if (cursorSlot.HasItem && clickedslot.HasItem) {
            if (cursorSlot.itemSlot.stack.id != clickedslot.itemSlot.stack.id) {
                ItemStack oldCursorSlot = cursorSlot.itemSlot.TakeAll();
                ItemStack oldSlot = clickedslot.itemSlot.TakeAll();

                clickedslot.itemSlot.InsertStack(oldCursorSlot);
                cursorSlot.itemSlot.InsertStack(oldSlot); 

            }
            else if (cursorSlot.itemSlot.stack.id == clickedslot.itemSlot.stack.id) {
                if (cursorSlot.itemSlot.stack.amount == cursorSlot.itemSlot.stackSize) {
                    ItemStack oldCursorSlot = cursorSlot.itemSlot.TakeAll();
                    ItemStack oldSlot = clickedslot.itemSlot.TakeAll();

                    clickedslot.itemSlot.InsertStack(oldCursorSlot);
                    cursorSlot.itemSlot.InsertStack(oldSlot);
                }
                else if (cursorSlot.itemSlot.stack.amount < cursorSlot.itemSlot.stackSize || clickedslot.itemSlot.stack.amount > clickedslot.itemSlot.stackSize) {
                    int cursorslotamount = cursorSlot.itemSlot.stack.amount;
                    int clickedslotamount = clickedslot.itemSlot.stack.amount;
                    int combinedresult = cursorslotamount + clickedslotamount;
                    cursorSlot.itemSlot.EmptySlot();
                    if (combinedresult <= 64) {
                        clickedslot.itemSlot.stack.amount = combinedresult;
                    }
                    else {
                        clickedslot.itemSlot.stack.amount = 64;
                    }
                }
                    
                
            }
        }
    }
    private UIItemSlot CheckForSlot() {
        m_PointerEventData = new PointerEventData(m_Eventsystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results) {
            if (result.gameObject.tag == "UIItemSlot") {
                return result.gameObject.GetComponent<UIItemSlot>();
            }
        }
        return null;
    }
}
