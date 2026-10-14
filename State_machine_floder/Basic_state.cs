using System;
using System.Collections.Generic;
using System.Text;

namespace game_test.State_machine_floder
{
    /// <summary>
    /// 基本状态，负责判断角色应该进入什么状态
    /// 如果你不知道角色应该进入那种状态就用这个
    /// </summary>
    internal class Basic_state:State
    {
        public Basic_state(State_machine state_Machine) : base(state_Machine, "Basic_state")
        {
        }
        public override void Update()
        {
            Change_to();
        }
        void Change_to()
        {
            if (Hero.Is_character_on_ground() || collider_around != null)
            {
                Change_state("Move_state");
            }
            else if (!Hero.Is_character_on_ground())
            {
                Change_state("In_air_state");
            }
        }
    }
}
