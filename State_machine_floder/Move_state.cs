using game_test.divice_manager;
using System.Numerics;
namespace game_test.State_machine_floder
{
    internal class Move_state : State
    {

        public Move_state(State_machine state_Machine) : base(state_Machine, "Move_state")
        {
        }
        public override void Update()
        {
            Move_character();
            Change_state_to();
            base.Update();
        }
        void Change_state_to()
        {
            if (!Hero.Is_character_on_ground() && platform_around == null)
            {
                Change_state("In_air_state");
            }
        }
        void Move_character()
        {
            Vector2 velocity = new Vector2(0, 0);
            if (Key_input.Is_key_donw(Keys.D))
            {
                if (Math.Abs(Hero.Velocity.X) > Hero.Max_speed)
                {
                    //Hero.Velocity = new Vector2(Hero.Max_speed, Hero.Velocity.Y);//如果速度超过最大速度，则将速度限制为最大速度
                }
                else
                {
                    velocity.X += Hero.acceleration;
                    Hero.Velocity += velocity;
                }
            }
            else if (Key_input.Is_key_donw(Keys.A))
            {
                if (Math.Abs(Hero.Velocity.X) > Hero.Max_speed)
                {
                    //Hero.Velocity = new Vector2(-Hero.Max_speed, Hero.Velocity.Y);//如果速度超过最大速度，则将速度限制为最大速度
                }
                else
                {
                    velocity.X -= Hero.acceleration;
                    Hero.Velocity += velocity;
                }
            }
            if (Key_input.Is_key_donw(Keys.Space))
            {
                velocity.Y = -15; // 跳跃时向上施加一个负的垂直速度
                if (Hero.Is_character_on_ground())
                {
                    Hero.Velocity += velocity;
                }
                if (platform_around != null)//只有角色周围有平台时才检测角色是否在上面
                {
                    if (Hero.Collider.IsColliding(platform_around.Collider))
                    {
                        Hero.Velocity += velocity;
                    }
                }
            }
        }
    }
}
