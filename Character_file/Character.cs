using game_test.Item;
using game_test.State_machine_floder;
using System.Numerics;
namespace game_test.Character_file
{
    public class Character
    {
        internal readonly State_machine state_machine;//角色的状态机
        /// <summary>
        /// 摩擦乘数每帧结算到速度上，值越小摩擦力越大，范围0-1
        /// </summary>
        public float friction = 0.7f;
        public float acceleration = 7f; // 加速度，值越大角色加速越快
        public float gravity = 1.5f;//重力加速度
        public Play form { get; set; }//角色所在的窗体
        public string Name { get; set; }
        public double Health { get; set; }
        public double Max_health => 100 + (Level * 20);//角色最大生命值
        public int Level { get; set; }
        /// <summary>
        /// 这是检测平台碰撞区域的左上角（这片区域用于检测角色与平台的碰撞历遍）
        /// </summary>
        public Vector2 Check_collider_left => new Vector2(Position.X - 300, Position.Y);
        /// <summary>
        /// 这是检测平台碰撞区域的右下角
        /// </summary>
        public Vector2 Check_collider_right => new Vector2(Position.X + Width + 300, Position.Y + Height);
        public Vector2 Position { get; set; } = new Vector2(0, 0);//角色位置
        public Collider Collider
        {
            get
            {
                return field.Collider_moved(Position);
            }
        } = new Collider(new Vector2(0, 0), Width, Height);
        /// <summary>
        /// 这是角色的速度，X轴为水平速度，Y轴为垂直速度
        /// </summary>
        public Vector2 Velocity { get; set; } = new Vector2(0, 0);//角色速度
        public float Max_speed { get; } = 25;//角色最大移动速度
        public int Experience_to_get_up => 100 + (Level * 50);//角色升级所需经验值
        public bool Is_character_on_ground()
        {
            return Position.Y >= form.ClientSize.Height - Height - 15;//角色是否在地面上
        }
        public int Experience_now
        {
            get;
            set
            {
                // 1. 防御性编程：如果传入负数，直接设为 0（或者根据你的游戏规则处理）
                if (value < 0)
                {
                    field = 0;
                    return;
                }
                // 3. 使用 while 循环处理“连升多级”的情况（比如一次加 1000 经验）
                while (value >= Experience_to_get_up)
                {
                    //在没升级时记录最大经验，以防出现升级后当前经验值为负数
                    int remaining = Experience_to_get_up;
                    Level++;
                    this.Health = Max_health; // 升级时恢复生命值
                    value -= remaining;
                }
                // 4. 最后将剩余经验存入后备字段（这里必须用 field，不能用 Experience_now）
                field = value;
            }
        }
        public bool is_dead => Health <= 0;
        public static int Height = 50;//角色的高度
        public static int Width = 50;//角色的宽度
        public Bitmap body = new Bitmap(Height, Width);//角色的绘图对象
        void Draw_character()//绘制角色的图像
        {
            using (Graphics g = Graphics.FromImage(body))
            {
                g.Clear(Color.Blue);
            }
        }
        /// <summary>
        /// 这是正常的创建角色(普通)，输入名称，然后等级为1，生命值为最大生命值
        /// </summary>
        /// <param name="name">角色的名字</param>
        public Character(string name)
        {
            this.Name = name;
            this.Level = 1;
            Health = Max_health;
            Draw_character();
            state_machine = new State_machine(this);
        }
        /// <summary>
        /// 默认创建角色
        /// </summary>
        public Character()
        {
            this.Name = "mono";
            this.Level = 1;
            Health = Max_health;
            Draw_character();
            state_machine = new State_machine(this);
        }
        /// <summary>
        /// 角色获得经验值的方法，传入经验值，自动处理升级和经验值溢出
        /// </summary>
        /// <param name="Exp">获取的经验值</param>
        public void Experience_get(int Exp)
        {
            this.Experience_now += Exp;
        }
        /// <summary>
        /// 角色受到伤害的方法，传入伤害值，自动处理死亡状态
        /// </summary>
        /// <param name="damage">角色受到的伤害</param>
        public void Health_lose(double damage)
        {
            this.Health -= damage;
            if (is_dead)
            {
                MessageBox.Show($"角色 {this.Name} 已死亡！");
                return;
            }
        }
        /// <summary>
        /// 此函数检测玩家周围是否有平台（我可不想每一帧都吧全图的平台都历遍一遍）
        /// </summary>
        /// <param name="platforms">平台的集合</param>
        /// <returns>返回该平台</returns>
        public Platform What_platform_around(List<Platform> platforms)
        {
            //我只要把每个平台的坐标和角色的检测箱比较就可以得出是否有平台在周围
            //这里偷懒把碰撞检测区域改成比平台还大
            foreach (Platform platform in platforms)
            {
                //        检测该点是否在区域内
                if (platform.position.X <= this.Check_collider_right.X && platform.position.X >= this.Check_collider_left.X && platform.position.Y >= this.Check_collider_left.Y && platform.position.Y <= this.Check_collider_right.Y)
                {
                    return platform;
                }
            }
            return null;
        }
        public void Show_character_info()
        {
            Show_character show_Character = new Show_character(this);
            show_Character.Show();
        }
    }
}
