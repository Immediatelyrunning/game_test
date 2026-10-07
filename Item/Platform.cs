using System.Numerics;

namespace game_test.Item
{
    public class Platform
    {
        public static float width = 300;
        public static float height = 20;
        public Vector2 position;
        public Collider Collider;
        public Bitmap Platform_body = new Bitmap((int)width, (int)height);
        void Draw_platform()//绘制图像
        {
            using (Graphics g = Graphics.FromImage(Platform_body))
            {
                g.Clear(Color.Black);
            }
        }
        /// <summary>
        /// 输入随机数然后在随机位置生成平台
        /// </summary>
        /// <param name="X">X坐标</param>
        /// <param name="Y">Y坐标</param>
        public Platform(float X, float Y)
        {
            position = new Vector2(X, Y);
            Collider = new Collider(position, width, height);
            Draw_platform();
        }
    }
}
