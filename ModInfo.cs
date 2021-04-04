using System;
using System.Reflection;
using ColossalFramework;
using ICities;

namespace RealisticCashMoney
{
    public class ModInfo : IUserMod
    {
        public static readonly string FullName = $"RealisticCashMoneyMod {Version}";

        public ModInfo()
        {
            try
            {
                if (GameSettings.FindSettingsFileByName(RealisticCashMoney.ModName) == null)
                    GameSettings.AddSettingsFile(new SettingsFile { fileName = RealisticCashMoney.ModName });
            }
            catch (Exception e)
            {
                Dbg.Err("Could not create settings file", e);
            }
        }

        private static string Version => typeof(ModInfo).Assembly.GetName().Version.ToString();

        public string Name => FullName;

        public string Description => "Draw cash advances against a limited line of credit with interest compounding daily and payments coming due monthly.";
    }
}