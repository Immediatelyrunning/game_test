using game_test.Character_file;

namespace game_test
{
    public partial class Main_form : Form
    {
        Character Hero => Play.Hero;
        Play Play { get; set; }
        public Main_form()
        {
            InitializeComponent();
            Input_test();
        }
        void Start_game(string name)
        {
            Play play = new Play(name);
            Play = play;
            Play.Show();
            Show_character_info();
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
                    Start_game(name);
                    return;
                }
                Start_game(name);
            };
            this.Controls.Add(create_character_button);
            this.ActiveControl = create_character_button;
        }
    }
}
