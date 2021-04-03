using ICities;
using UnityEngine;

namespace RCMM
{

    public class RealisticCashMoneyMod : IUserMod
    {

        public string Name
        {
            get { return "RealisticCashMoneyMod"; }
        }

        public string Description
        {
            get { return "Draw cash advances against a limited line of credit with interest compounded daily and payments coming due monthly."; }
        }
    }
}