using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace game_test.State_machine_floder
{
    internal class In_air_state : State
    {
        public In_air_state(State_machine state_Machine) : base(state_Machine, "In_air_state")
        {
        }
        public override void Update()
        {
            // 在空中状态下的逻辑
            In_air();
            base.Update();
        }
        public void Change_state_to()
        {
            if (Hero.Is_character_on_ground() || platform_around != null)
            {
                Change_state("Move_state");
            }
        }
        void In_air()
        {
            Vector2 velocity = new Vector2(0, 0);
            if (Key_input.Is_key_donw(Keys.A))
            {
                velocity.X -= Hero.acceleration * 0.4f; // 在空中时
                if (Math.Abs(Hero.Velocity.X) > Hero.Max_speed)
                {
                    Hero.Velocity = new Vector2(-Hero.Max_speed, Hero.Velocity.Y);//如果速度超过最大速度，则将速度限制为最大速度
                }
                else
                {
                    Hero.Velocity += velocity;
                }
            }
            if (Key_input.Is_key_donw(Keys.D))
            {
                velocity.X += Hero.acceleration * 0.4f; // 在空中时
                if (Math.Abs(Hero.Velocity.X) > Hero.Max_speed)
                {
                    Hero.Velocity = new Vector2(Hero.Max_speed, Hero.Velocity.Y);//如果速度超过最大速度，则将速度限制为最大速度
                }
                else
                {
                    Hero.Velocity += velocity;
                }
            }
        }
    }
}
