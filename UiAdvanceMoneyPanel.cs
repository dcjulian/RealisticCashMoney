using System;
using System.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace RealisticCashMoney
{
    public class UiAdvanceMoneyPanel : UIPanel
    {
        private static readonly Vector3 RelativePos = new Vector3(559f, 193f);
        private static readonly Vector2 Size = new Vector2(275f, 260f);
        private UIButton _applyButton;
        private UIButton _closeButton;
        private UITextField _moneyEntry;
        private UISprite _sprite;
        private UILabel _statusLabel;
        private UILabel _title;

        private UIView _uiParent;
        private UIButton _xButton;

        private string Status
        {
            set => _statusLabel.text = value;
        }

        private string InputAmount
        {
            get => _moneyEntry.text;
            set => _moneyEntry.text = value;
        }

        public override void Start()
        {
            _uiParent = UIView.GetAView();
            transform.parent = _uiParent.transform;
            relativePosition = RelativePos;

            backgroundSprite = "MenuPanel";
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            size = Size;

            _title = AddUIComponent<UILabel>();
            _applyButton = AddUIComponent<UIButton>();
            _closeButton = AddUIComponent<UIButton>();
            _xButton = AddUIComponent<UIButton>();
            _statusLabel = AddUIComponent<UILabel>();
            _moneyEntry = AddUIComponent<UITextField>();
            _sprite = AddUIComponent<UISprite>();

            var dragHandleObject = new GameObject("RCM_UiGrantMoneyPanel_DragHandler");
            dragHandleObject.transform.parent = transform;
            dragHandleObject.transform.localPosition = Vector3.zero;
            var dragHandle = dragHandleObject.AddComponent<UIDragHandle>();
            dragHandle.width = width - 32;
            dragHandle.height = 40f;
            dragHandle.zOrder = 1000;

            _title.text = "Apply for a cash advance";
            _title.relativePosition = new Vector3(15, 15);
            _title.textScale = 0.9f;
            _title.size = new Vector2(200, 30);

            _xButton.text = "";
            _xButton.normalBgSprite = "buttonclose";
            _xButton.hoveredBgSprite = "buttonclosehover";
            _xButton.pressedBgSprite = "buttonclosepressed";
            _xButton.focusedFgSprite = "DistrictOptionBrushMediumFocused";
            _xButton.size = new Vector2(32, 32);
            _xButton.relativePosition = new Vector3(width - 32 - 4, 8);
            _xButton.eventClick += (c, p) => ModUi.Instance.Hide();
            _xButton.textScale = 0.9f;
            _xButton.playAudioEvents = true;
            _xButton.canFocus = true;

            _sprite.relativePosition = new Vector3(width / 2 - 40, 60);
            _sprite.size = new Vector2(80, 80);
            _sprite.spriteName = "MoneyThumb";
            _sprite.canFocus = false;

            Status = "";
            _statusLabel.relativePosition = new Vector3(15, 160);
            _statusLabel.textScale = 0.8f;
            _statusLabel.size = new Vector3(width - 15 - 15, 20);

            _moneyEntry.relativePosition = new Vector3(15, 190);
            _moneyEntry.textScale = 1.0f;
            _moneyEntry.normalBgSprite = "TextFieldPanel";
            _moneyEntry.hoveredBgSprite = "TextFieldPanelHovered";
            _moneyEntry.selectionSprite = "EmptySprite"; //"TextFieldUnderline";
            _moneyEntry.cursorBlinkTime = 0.45f;
            _moneyEntry.cursorWidth = 1;
            _moneyEntry.size = new Vector3(width - 15 - 15 - 80 - 15, 20);
            _moneyEntry.isInteractive = true;
            _moneyEntry.enabled = true;
            _moneyEntry.readOnly = false;
            _moneyEntry.builtinKeyNavigation = true;
            _moneyEntry.numericalOnly = true;
            _moneyEntry.allowFloats = true;
            _moneyEntry.allowNegative = true;
            _moneyEntry.eventTextSubmitted += (c, v) => FormatText(v);
            _moneyEntry.playAudioEvents = true;
            _moneyEntry.canFocus = true;

            _applyButton.text = "Apply";
            _applyButton.normalBgSprite = "ButtonMenu";
            _applyButton.hoveredBgSprite = "ButtonMenuHovered";
            _applyButton.focusedBgSprite = "ButtonMenuFocused";
            _applyButton.pressedBgSprite = "ButtonMenuPressed";
            _applyButton.normalFgSprite = "ButtonMenuMain";
            _applyButton.size = new Vector2(80, 20);
            _applyButton.relativePosition = new Vector3(15 + _moneyEntry.width + 15, 190);
            _applyButton.eventClick += (c, p) => TryAdvance(InputAmount);
            _applyButton.textScale = 0.8f;
            _applyButton.playAudioEvents = true;
            _applyButton.canFocus = true;

            _closeButton.text = "Close";
            _closeButton.normalBgSprite = "ButtonMenu";
            _closeButton.hoveredBgSprite = "ButtonMenuHovered";
            _closeButton.focusedBgSprite = "ButtonMenuFocused";
            _closeButton.pressedBgSprite = "ButtonMenuPressed";
            _closeButton.size = new Vector2(100, 30);
            _closeButton.relativePosition = new Vector3(165, 220);
            _closeButton.eventClick += (c, p) => ModUi.Instance.Hide();
            _closeButton.textScale = 0.9f;
            _closeButton.playAudioEvents = true;
            _closeButton.canFocus = true;

            ReInitUi();

            Hide();
            Unfocus();
            ModUi.Instance.Hide();

            base.Start();
        }

        internal void ReInitUi()
        {
            var initAmt = RealisticCashMoney.Instance == null ? 1_000_000 : RealisticCashMoney.Instance.AdvanceAmount;
            Dbg.Log($"Init advance amount: ₡{initAmt:N}");
            UpdateAmt(initAmt);

            Status = "Amount?";
        }

        protected override void OnGotFocus(UIFocusEventParameter p)
        {
            _moneyEntry.Focus();
        }

        protected override void OnKeyUp(UIKeyEventParameter p)
        {
            switch (p.keycode)
            {
                case KeyCode.Escape:
                    p.Use();
                    ModUi.Instance.Hide();
                    break;
                case KeyCode.Return:
                    p.Use();
                    FormatText(InputAmount);
                    break;
                default:
                    base.OnKeyUp(p);
                    break;
            }
        }

        private void UpdateAmt(int amount)
        {
            InputAmount = $"{amount:N}";
        }

        private void FormatText(string input)
        {
            try
            {
                // meh ignore cents for now
                UpdateAmt(Convert.ToInt32(decimal.Parse(input.Trim(), NumberStyles.Currency)));
            }
            catch (Exception e)
            {
                Dbg.Err($"Could not parse money '{input}'", e);
                Status = "That does not seem to be a valid amount of money.";
            }
        }

        private void TryAdvance(string input)
        {
            try
            {
                Advance(Convert.ToInt32(decimal.Parse(input.Trim(), NumberStyles.Currency)));
            }
            catch (Exception e)
            {
                Dbg.Err("Could not increase money", e);
                Status = "Unable to make advance.";
            }
        }

        private void Advance(int amount)
        {
            if (RealisticCashMoney.Instance != null)
            {
                RealisticCashMoney.Instance.GrantMoney(amount);
                UpdateAmt(RealisticCashMoney.Instance.AdvanceAmount);
                Status = $"Approved!  ₡{InputAmount} advanced.";
            }
            else
            {
                Status = "Could not give advance.";
            }
        }
    }
}