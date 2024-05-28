using Model.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BuffsDebuffsSystem
{
    public class IncAttSpdEffect : Effect
    {
        public IncAttSpdEffect(Unit _unit) : base(_unit)
        {
            Modifier = 0.75f;
            Duration = 5f;
        }
    }
}
