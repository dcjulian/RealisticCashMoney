using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RCMM
{
    public class AdvanceWindow : UIPanel
    {
        UIButton okButton;
        UIButton cancelButton;
        UILabel title;
        UITextField moneyEntry;
        UILabel errorLabel;

        Event onUpdated;

        private UIView uiParent;

        private static float WidthScale;
        private static float HeightScale;
        private static float PanelWidth = 275f;
        private static float PanelHeight = 370f;
        private static float DistanceFromLeft = 10f;
        private static float DistanceFromTop = 65f;
        private static float ControlHeight = 25f;

        public override void Start()
        {
            foreach (var uiView in GameObject.FindObjectsOfType<UIView>())
            {
                if (uiView.name == "UIView")
                {
                    this.uiParent = uiView;
                    this.transform.parent = this.uiParent.transform;
                    this.relativePosition = new Vector3(DistanceFromLeft, DistanceFromTop);
                    break;
                }
            }

            base.Start();

            this.backgroundSprite = "MenuPanel";
            this.isVisible = true;
            this.canFocus = true;
            this.isInteractive = true;
            this.width = PanelWidth;
            this.height = PanelHeight;

            try
            {
                SetupControls();
            }
            catch (Exception e)
            {
                GameObject.Destroy(this.gameObject);
                throw e;
            }

            this.Hide();
        }

        private void SetupControls()
        {
            title = AddUIComponent<UILabel>();
            okButton = AddUIComponent<UIButton>();
            cancelButton = AddUIComponent<UIButton>();
            errorLabel = AddUIComponent<UILabel>();
            moneyEntry = AddUIComponent<UITextField>();

            title.text = "Cash Advance amount requested";
            title.relativePosition = new Vector3(15, 15);
            title.textScale = 0.9f;
            title.size = new Vector2(200, 30);

            moneyEntry.relativePosition = new Vector3(15, 50);
            moneyEntry.text = "20000";
            moneyEntry.textScale = 0.8f;
            moneyEntry.normalBgSprite = "TextFieldPanel";
            moneyEntry.hoveredBgSprite = "TextFieldPanelHovered";
            moneyEntry.focusedBgSprite = "TextFieldUnderline";
            moneyEntry.size = new Vector3(width - 120 - 30, 20);
            moneyEntry.isInteractive = true;
            moneyEntry.enabled = true;
            moneyEntry.readOnly = false;
            moneyEntry.builtinKeyNavigation = true;

            var toggleButtonDragHandleObject = new GameObject("ButtonDragHandler");
            toggleButtonDragHandleObject.transform.parent = this.okButton.transform;
            toggleButtonDragHandleObject.transform.localPosition = Vector3.zero;
            var toggleButtonDragHandle = toggleButtonDragHandleObject.AddComponent<UIDragHandle>();
            toggleButtonDragHandle.width = this.okButton.width;
            toggleButtonDragHandle.height = this.okButton.height;

            // Create a drag handle for this panel
            var dragHandleObject = new GameObject("DragHandler");
            dragHandleObject.transform.parent = this.transform;
            dragHandleObject.transform.localPosition = Vector3.zero;
            var dragHandle = dragHandleObject.AddComponent<UIDragHandle>();
            dragHandle.width = this.width;
            dragHandle.height = 40f * HeightScale;
            dragHandle.zOrder = 1000;

            errorLabel.relativePosition = new Vector3(15, 80);
            errorLabel.text = "";
            errorLabel.textScale = 0.8f;
            errorLabel.size = new Vector3(120, 20);

            cancelButton.text = "Close";
            cancelButton.normalBgSprite = "ButtonMenu";
            cancelButton.hoveredBgSprite = "ButtonMenuHovered";
            cancelButton.focusedBgSprite = "ButtonMenuFocused";
            cancelButton.pressedBgSprite = "ButtonMenuPressed";
            cancelButton.size = new Vector2(100, 30);
            cancelButton.relativePosition = new Vector3(15, 110);
            cancelButton.eventClick += cancelButton_eventClick;
            cancelButton.textScale = 0.8f;

            okButton.text = "OK";
            okButton.normalBgSprite = "ButtonMenu";
            okButton.hoveredBgSprite = "ButtonMenuHovered";
            okButton.focusedBgSprite = "ButtonMenuFocused";
            okButton.pressedBgSprite = "ButtonMenuPressed";
            okButton.size = new Vector2(100, 30);
            okButton.relativePosition = new Vector3(130, 110);
            okButton.eventClick += okButton_eventClick;
            okButton.textScale = 0.8f;

            height = 116;

        }

        private void setErrorLabel(string msg)
        {
            errorLabel.text = msg;
        }

        private void cancelButton_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            this.Hide();
        }

        private void okButton_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            var amount = moneyEntry.text.Trim();

            try
            {
                int amountInt = Int32.Parse(amount);
                increaseMoneyAmount(amountInt);
            }
            catch (Exception e)
            {
                setErrorLabel(e.Message);
            }
        }

        private void increaseMoneyAmount(int amount)
        {
            Singleton<EconomyManager>.instance.AddResource(EconomyManager.Resource.RewardAmount, amount * 100, new ItemClass());
            setErrorLabel(String.Format("Available funds increased by ${0}", amount));
        }
    }
}