namespace game_test
{
    public partial class Show_character : Form
    {
        Character Hero { get; set; }
        public Show_character(Character Hero)
        {
            InitializeComponent();
            Show_character_Load();
            this.Hero = Hero;
            this.Text = $"角色 {Hero.Name} 的信息";
        }
        void Show_character_Load()
        {
            Label label = new Label()
            {
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("微软雅黑", 20)
            };
            this.Controls.Add(label);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100; // 每0.1秒触发一次
            timer.Tick += (s, e) =>
            {
                if (Hero.is_dead)
                {
                    label.Text = $"角色 {Hero.Name} 已死亡！";
                }
                else
                {
                    label.Text = $"角色 {Hero.Name}\n等级：{Hero.Level}\n生命值：{Hero.Health:F2}/{Hero.Max_health:F2}\n经验值：{Hero.Experience_now}/{Hero.Experience_to_get_up}" +
                    $"\n坐标({Hero.Position.X},{Hero.Position.Y})"+$"\n碰撞箱坐标({Hero.Collider.Point_left.X},{Hero.Collider.Point_left.Y})"
                    +$"\n周围是否有平台：{Hero.What_platform_around(Hero.form.platforms)!=null}"
                    +$"\n现在的状态：{Hero.state_machine.current_state.Name}";
                }
            };
            timer.Start(); // 开始更新
        }
    }
}
