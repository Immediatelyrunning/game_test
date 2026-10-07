using game_test.Character_file;
using game_test.Item;
using System.Numerics;
namespace game_test.State_machine_floder
{
    /// <summary>
    /// 基本状态
    /// </summary>
    class State
    {
        List<Platform> platforms => Hero.form.platforms;//获取窗体中的平台列表
        public Platform platform_around => Hero.What_platform_around(platforms);//周围的平台
        public Character Hero => state_machine.Hero;
        public string Name { get; set; }
        State_machine state_machine { get; set; }
        public State(State_machine state_Machine, string name)
        {
            state_machine = state_Machine;
            Name = name;
        }
        /// <summary>
        /// 这是状态机的更新方法，每个状态都会在每一帧调用这个方法，可以在这里实现状态的逻辑
        /// base.Update()包含了重力作用于角色，结算速度，帧结算，自动切换状态等逻辑，基本结算状态
        /// </summary>
        public virtual void Update()
        {
            Auto_change_state();
            Settling_basic_effects();
            Gravity_character();
        }
        void Gravity_character()//重力作用于角色
        {
            Vector2 velocity = new Vector2(0, 0);
            if (Hero.Position.Y < Hero.form.ClientSize.Height - Character.Height - 15)//只有角色未触底时才应用重力
            {
                velocity.Y += Hero.gravity;
                Hero.Velocity += velocity;
                Hero.Position += Hero.Velocity;
            }
            else//使角色触底时一直保持在地面上
            {
                Hero.Position = new Vector2(Hero.Position.X, Hero.form.ClientSize.Height - Character.Height - 15);
                Hero.Velocity = new Vector2(Hero.Velocity.X, 0);//落地时将垂直速度归零
            }
        }
        /// <summary>
        /// 结算速度，帧结算
        /// </summary>
        void Settling_basic_effects()
        {
            if (Hero.Position.Y < Hero.form.ClientSize.Height - Character.Height - 15)//在空中时
            {
                Hero.Position += Hero.Velocity;//先更新位置，再应用摩擦力和重力
                if (platform_around != null)
                {
                    Hero.Velocity = new Vector2(Hero.Velocity.X * platform_around.Collider.friction, Hero.Velocity.Y ); // 在空中时，水平速度*平台摩擦力，垂直速度+重力
                }
                else
                {
                    Hero.Velocity = new Vector2(Hero.Velocity.X * 0.95f, Hero.Velocity.Y); // 在空中时，水平速度*0.95，模拟空气阻力
                }
            }
            else
            {
                Hero.Position += Hero.Velocity;//先更新位置，再应用摩擦力和重力
                Hero.Velocity = new Vector2(Hero.Velocity.X * Hero.friction, Hero.Velocity.Y); // 如果没有按下A或D键，则应用摩擦力减小水平速度
            }
        }
        void Auto_change_state()
        {
            if (Hero.Is_character_on_ground() || platform_around != null)
            {
                Change_state("Move_state");
            }
        }
        /// <summary>
        /// 这是状态机的切换方法，可以在这里实现状态的切换逻辑
        /// </summary>
        /// <param name="new_state_name"></param>
        public virtual void Change_state(string new_state_name)
        {
            state_machine.Change_state(new_state_name);
        }
    }
    /// <summary>
    /// 角色状态机
    /// </summary>
    internal class State_machine
    {
        public Character Hero { get; set; }//状态机的主人公引用
        List<State> states = new List<State>();
        public State current_state { get; set; }
        System.Threading.Timer timer;
        public void Get_character(Character character)
        {
            Hero = character;
        }
        /// <summary>
        /// 状态机，使用Timer来进行循环调用每一帧的Update方法，默认60帧每秒
        /// </summary>
        /// <param name="character"></param>
        public State_machine(Character character)
        {
            states.Add(new Move_state(this));
            states.Add(new In_air_state(this));
            current_state = states[0];
            Get_character(character);
            Play.Load_finished += (sender, e) =>
            {
                New_thread();
            };
        }
        void New_thread()
        {
            Thread thread = new Thread(() =>
            {
                //MessageBox.Show("状态机线程已启动");
                timer = new System.Threading.Timer((e) =>
                            {
                                current_state?.Update();
                            }, null, 0, 1000 / 45); // 45帧采样率
            });
            thread.IsBackground = true;
            thread.Start();
        }
        public void Change_state(string Name)
        {
            foreach (var state in states)
            {
                if (state.Name == Name)
                {
                    current_state = state;
                    return;
                }
            }
            throw new Exception($"State '{Name}' not found in the state machine.");
        }
    }
}
