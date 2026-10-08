using game_test.Character_file;
using game_test.Event;
using game_test.Item;
using game_test.Map;
using System.Numerics;
namespace game_test
{
    public partial class Play : Form
    {
        Block_changed_event block_Changed = new();
        internal Map_creator_and_manager map_creator_and_manager;
        bool Is_paltform_around = false;//是否平台在角色周围
        Platform platform_around => Hero.What_platform_around(platforms);//周围的平台
        //float friction = 0.8f; // 摩擦系数，值越小摩擦力越大
        //float acceleration = 7f; // 加速度，值越大角色加速越快
        //float gravity = 1.5f;//重力加速度
        public Character Hero;
        System.Windows.Forms.Timer Timer { get; set; }//游戏循环计时器
        public List<Platform> platforms;
        public Play(Character character)
        {
            Hero = character;
            Hero.form = this;
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
         ControlStyles.OptimizedDoubleBuffer, true);
            //Character_test();
        }
        void Out_of_bounds()//防止角色超出边界
        {
            if (Hero.Position.X < 0)
            {
                Hero.Position = new Vector2(0, Hero.Position.Y);
                Hero.Velocity = new Vector2(0, Hero.Velocity.Y);
            }
            else if (Hero.Position.X > this.ClientSize.Width - Character.Width)
            {
                Hero.Position = new Vector2(this.ClientSize.Width - Character.Width, Hero.Position.Y);
                Hero.Velocity = new Vector2(0, Hero.Velocity.Y);
            }
        }
        /// <summary>
        /// 游戏逻辑主循环
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Timer_Tick(object? sender, EventArgs e)
        {
            //Move_character();
            Draw_character();
            //Gravity_character();
            block_Changed.Block = map_creator_and_manager.Get_where_block(Hero.Position);
            //Out_of_bounds();
            //Check_platform_collision();
        }
        void Check_platform_collision()//检测角色与平台的碰撞
        {
            Is_paltform_around = platform_around == null;
            if (!Is_paltform_around)//只有角色周围有平台时才检测
            {
                if (Hero.Collider.IsColliding(platform_around.Collider))
                {
                    // 处理碰撞逻辑，例如停止角色的下落，调整位置等
                    Hero.Position = new Vector2(Hero.Position.X, platform_around.position.Y - Character.Height);
                    Hero.Velocity = new Vector2(Hero.Velocity.X, 0); // 停止垂直速度
                }
            }
        }
        /*void Move_character()
        {
            Vector2 velocity = new Vector2(0, 0);
            if (Key_input.Is_key_donw(Keys.D))
            {
                if (Hero.Is_character_on_ground())
                {
                    velocity.X += acceleration;
                }
                else
                {
                    velocity.X += acceleration * 0.3f; // 在空中时
                }
                if (Math.Abs(Hero.Velocity.X) > Hero.Max_speed)
                {
                    Hero.Velocity = new Vector2(Hero.Max_speed, Hero.Velocity.Y);//如果速度超过最大速度，则将速度限制为最大速度
                }
                else
                {
                    Hero.Velocity += velocity;
                }
            }
            else if (Key_input.Is_key_donw(Keys.A))
            {
                if (Hero.Is_character_on_ground())
                {
                    velocity.X -= acceleration;
                }
                else
                {
                    velocity.X -= acceleration * 0.3f; // 在空中时
                }
                if (Math.Abs(Hero.Velocity.X) > Hero.Max_speed)
                {
                    Hero.Velocity = new Vector2(-Hero.Max_speed, Hero.Velocity.Y);//如果速度超过最大速度，则将速度限制为最大速度
                }
                else
                {
                    Hero.Velocity += velocity;
                }
            }
            if (Key_input.Is_key_donw(Keys.Space))
            {
                velocity.Y = -20; // 跳跃时向上施加一个负的垂直速度
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
            if (Hero.Position.Y < this.ClientSize.Height - Character.Height - 15)//在空中时不结算阻力
            {
                Hero.Position += Hero.Velocity;//先更新位置，再应用摩擦力和重力
                Hero.Velocity = new Vector2(Hero.Velocity.X * 0.95f, Hero.Velocity.Y); // 在空中时，水平速度*0.95，模拟空气阻力
            }
            else
            {
                Hero.Position += Hero.Velocity;//先更新位置，再应用摩擦力和重力
                Hero.Velocity = new Vector2(Hero.Velocity.X * friction, Hero.Velocity.Y); // 如果没有按下A或D键，则应用摩擦力减小水平速度
            }
        }*/
        int loop_time = 0;
        void Draw_character()
        {
            PictureBox body = new PictureBox()
            {
                Name = "Hero_body",
                Size = Hero.body.Size,
                Image = Hero.body,
                Location = new Point((int)Math.Round(Hero.Position.X), (int)Math.Round(Hero.Position.Y))
            };
            loop_time++;
            if (loop_time >= 1)
            {
                loop_time = 0;
                this.Controls.RemoveByKey("Hero_body");
            }
            this.Controls.Add(body);
        }
        public static EventHandler Load_finished;
        public static EventHandler Load_started;
        void Play_load(object sender, EventArgs e)
        {
            map_creator_and_manager = new Map_creator_and_manager(this);
            map_creator_and_manager.Create_blocks();
            platforms = map_creator_and_manager.Get_where_block(Hero.Position).platforms;
            //Tips();
            //Create_platform();
            this.Text = $"游戏中 - 角色 {Hero.Name}";
            this.WindowState = FormWindowState.Maximized;

            Load_started?.Invoke(this, EventArgs.Empty);//触发加载开始事件

            Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 1000 / 60; // 每秒60帧
            Timer.Tick += Timer_Tick;
            Timer.Start();

            Load_finished?.Invoke(this, EventArgs.Empty);//触发加载完成事件
            block_Changed.Block_changed +=(sender,e)=> Draw_platforms();
            //MessageBox.Show($"地图尺寸{map_creator_and_manager.map_size}");
        }
        void Tips()
        {
            MessageBox.Show("游戏操作提示：\nA键：向左移动\nD键：向右移动\n空格键：跳跃\n" +
                "移动到窗口最上面即为过关");
        }
        /// <summary>
        /// 随机创建平台
        /// </summary>
        void Create_platform()
        {
            Rectangle workArea = Screen.PrimaryScreen.WorkingArea;
            float Window_width = workArea.Width; // 平台宽度
            float Window_height = workArea.Height; // 平台高度
            int num = (int)(Window_height / 150);//生成平台的数量
            //MessageBox.Show($"生成平台数量{Window_height},{Window_width}");
            Random random = new();
            for (; num > 0; num--)
            {
                float X = (float)random.NextDouble() * Window_width;
                float Y = Window_height * (num / (Window_height / 150));
                Platform platform = new(X, Y);
                //MessageBox.Show($"平台坐标{platform.position.X},{platform.position.Y}");
                platforms.Add(platform);
            }
        }
        void Draw_platforms()
        {
            this.Controls.Clear();
            map_creator_and_manager.Get_where_block(Hero.Position).Load();
            platforms = map_creator_and_manager.Get_where_block(Hero.Position).platforms;
            foreach (var platform in platforms)
            {
                PictureBox platformBox = new PictureBox()
                {
                    Size = new Size((int)platform.Collider.buttom, (int)platform.Collider.right),
                    Image = platform.Platform_body,
                    Location = new Point((int)platform.position.X, (int)platform.position.Y)
                };
                //MessageBox.Show(platformBox.Location.ToString());
                this.Controls.Add(platformBox);
            }
            //map_creator_and_manager.Get_where_block(Hero.Position).Unload();
        }
    }
}