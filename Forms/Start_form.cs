using game_test.Character_file;

namespace game_test
{
    public partial class Main_form : Form
    {
        Character Hero { get; set; }
        Play Play { get; set; }
        public Main_form()
        {
            InitializeComponent();
            Input_test();
            Show_button();
        }
        void Start_game()
        {
            Play play = new Play(Hero);
            Play = play;
            Play.Show();
        }
        void Show_character_info()
        {
            if (Hero == null)
            {
                MessageBox.Show("请先创建角色！");
                return;
            }
            Hero.Show_character_info();
        }
        void Input_test()
        {
            TextBox textBox = new TextBox()
            {
                Location = new Point(5, 60),
                Size = new Size(200, 60),
                PlaceholderText = "请输入角色名称",
            };
            this.Controls.Add(textBox);
            Button create_character_button = new Button()
            {
                Text = "创建角色",
                Location = new Point(textBox.Location.X + textBox.Size.Width + 5, textBox.Location.Y),
                Size = new Size(100, textBox.Size.Height)
            };
            create_character_button.Click += (sender, e) =>
            {
                string name = textBox.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("无输入，使用默认角色创建");
                    Character character = new Character();
                    Hero = character;
                    //Hero.Show_character_info();
                    Start_game();
                    return;
                }
                Create_character(name);
                Start_game();
            };
            this.Controls.Add(create_character_button);
            this.ActiveControl = create_character_button;
        }
        void Show_button()
        {
            Button button1 = new Button()
            {
                Text = "测试增加经验",
                Location = new Point(5, 0),
                Size = new Size(200, 50),
            };
            this.Controls.Add(button1);
            button1.Click += Get_experience;
            Button button_hurt = new Button()
            {
                Text = "测试扣血",
                Location = new Point(button1.Location.X + button1.Size.Width + 5, 0),
                Size = new Size(100, 50)
            };
            this.Controls.Add(button_hurt);
            button_hurt.Click += Hurt;
            Button Show_info = new Button()
            {
                Text = "显示角色信息",
                Location = new Point(button_hurt.Location.X + button_hurt.Size.Width + 5, 0),
                Size = new Size(200, 50),

            };
            Show_info.Click += (sender, e) => Show_character_info();
            this.Controls.Add(Show_info);
            /*
            Button Continue = new Button()
            {
                Text = "继续游戏",
                Location = new Point(Show_info.Location.X + Show_info.Size.Width + 5, 0),
                Size = new Size(200, 50),
            };
            Continue.Click += (sender, e) =>
            {
                if (Hero == null)
                {
                    MessageBox.Show("请先创建角色！");
                    return;
                }
                Play.Show();
            };
            this.Controls.Add(Continue);*/
        }
        void Create_character(string name)
        {
            if (Hero != null)
            {
                MessageBox.Show("角色已存在！");
                return;
            }
            Character character = new Character(name);
            MessageBox.Show($"角色 {character.Name} 创建成功，等级：{character.Level}，生命值：{character.Health:F2}/{character.Max_health:F2}");
            Hero = character;
            Hero.Show_character_info();
        }
        void Get_experience(object? sender, EventArgs e)
        {
            if (Hero == null)
            {
                MessageBox.Show("请先创建角色！");
                return;
            }
            Hero.Experience_get(200);
        }
        void Hurt(object? sender, EventArgs e)
        {
            if (Hero == null)
            {
                MessageBox.Show("请先创建角色！");
                return;
            }
            Hero.Health_lose(50);
            if (Hero.is_dead)
            {
                Hero = null;
            }
        }
    }
}
