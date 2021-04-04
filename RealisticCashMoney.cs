using System;
using ColossalFramework;
using ICities;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RealisticCashMoney
{
    [UsedImplicitly]
    public class RealisticCashMoneyLoader : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            Dbg.Log($"OnCreated {loading}");
            base.OnCreated(loading);
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            Dbg.Log("OnLevelLoaded");
            try
            {
                if (RealisticCashMoney.Instance == null)
                {
                    Dbg.Log("Creating Instance");
                    RealisticCashMoney.Instance = new GameObject(RealisticCashMoney.ModName).AddComponent<RealisticCashMoney>();
                    Object.DontDestroyOnLoad(RealisticCashMoney.Instance);
                    RealisticCashMoney.Instance.Start();
                    RealisticCashMoney.Instance.enabled = true;
                }
                else
                {
                    Dbg.Log("Starting Instance");
                    RealisticCashMoney.Instance.Start();
                    RealisticCashMoney.Instance.enabled = true;
                }

                if (ModUi.Instance == null)
                    try
                    {
                        Dbg.Log("Creating UI...");
                        ModUi.Instance = ToolsModifierControl.toolController.gameObject.AddComponent<ModUi>();
                    }
                    catch (Exception e)
                    {
                        Dbg.Err("Could not create UI", e);
                    }
            }
            catch (Exception e)
            {
                Dbg.Err("Creating Instance FAILED", e);
            }
        }

        public override void OnLevelUnloading()
        {
            Dbg.Log("Level Unloading");
            if (RealisticCashMoney.Instance != null)
            {
                Dbg.Log("Disabling Instance");
                RealisticCashMoney.Instance.enabled = false;
            }

            if (ModUi.Instance != null)
            {
                ModUi.Instance.Hide();
                Object.Destroy(ModUi.Instance);
                ModUi.Instance = null;
                Dbg.Log("UI Destroyed");
            }
        }

        public override void OnReleased()
        {
            Dbg.Log("OnReleased");
            base.OnReleased();
        }
    }

    public class RealisticCashMoney : MonoBehaviour
    {
        public const string ModName = "RealisticCashMoney";

        public static RealisticCashMoney Instance;

        private readonly SavedInt _mAdvanceAmount = new SavedInt("AdvanceAmount", ModName, 20_000, true);

        public int AdvanceAmount
        {
            get => _mAdvanceAmount.value;
            set => _mAdvanceAmount.value = Mathf.Clamp(value, -1_000_000_000, 1_000_000_000);
        }

        public void Start()
        {
            Dbg.Log("Start");
        }

        public void OnDisable()
        {
            Dbg.Log("Disable");
        }

        internal void GrantMoney(int amt)
        {
            AdvanceAmount = amt;
            try
            {
                Dbg.Log($"Advancing ₡{AdvanceAmount:N}");
                if (Singleton<EconomyManager>.exists)
                    // RewardAmount seems logical but does not work
                    // RefundAmount and LoanAmount both seem to work
                    Singleton<EconomyManager>.instance.AddResource(EconomyManager.Resource.LoanAmount,
                        AdvanceAmount * 100,
                        ItemClass.Service.None,
                        ItemClass.SubService.None,
                        ItemClass.Level.None);
                else
                    Dbg.Warn("EconomyManager does not exist -- cannot advance money");

                Dbg.Log($"Finished advancing ₡{AdvanceAmount:N}");
            }
            catch (Exception e)
            {
                Dbg.Err("AdvanceMoney FAILED", e);
            }
        }
    }
}