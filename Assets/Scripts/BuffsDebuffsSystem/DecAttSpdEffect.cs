using Model.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BuffsDebuffsSystem
{
    public class DecAttSpdEffect : Effect
    {
        public DecAttSpdEffect(Unit _unit) : base(_unit)
        {
            Modifier = 2f;
            Duration = 25f;
        }
    }
}
