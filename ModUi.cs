
using ColossalFramework.UI;


namespace RealisticCashMoney
{
    public class ModUi : UICustomControl
    {
        public static ModUi Instance;
        private readonly UiAdvanceMoneyPanel _advanceMoneyPanel;

        private readonly UiMainButton _mainButton;

        public ModUi()
        {
            Dbg.Log("UI Init");
            var uiView = UIView.GetAView();
            _mainButton = (UiMainButton)uiView.AddUIComponent(typeof(UiMainButton));
            _advanceMoneyPanel = (UiAdvanceMoneyPanel)uiView.AddUIComponent(typeof(UiAdvanceMoneyPanel));
            Hide();
        }

        private bool Visible => _advanceMoneyPanel.isVisible;

        ~ModUi()
        {
            Dbg.Log("UI Destroy");
            Destroy(_mainButton);
            Destroy(_advanceMoneyPanel);
        }

        public void Toggle()
        {
            if (Visible)
                Hide();
            else
                Show();
        }

        public void Show()
        {
            Dbg.Log("UI Show");
            _advanceMoneyPanel.ReInitUi();
            _advanceMoneyPanel.Show();
            _advanceMoneyPanel.Focus();
        }

        public void Hide()
        {
            Dbg.Log("UI Hide");
            _advanceMoneyPanel.Hide();
            _advanceMoneyPanel.Unfocus();
        }
    }
}