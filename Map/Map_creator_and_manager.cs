using System.Drawing.Drawing2D;
using System.Numerics;
namespace game_test.Map
{
    /// <summary>
    /// 地图生成器,管理器
    /// </summary>
    internal class Map_creator_and_manager
    {
        Play Form;
        /// <summary>
        /// 地图宽度
        /// </summary>
        int map_width_block = 2;
        /// <summary>
        /// 地图高度
        /// </summary>
        int map_height_block = 2;
        /// <summary>
        /// 地图中心,这才是(0,0)参考点
        /// </summary>
        Vector2 map_center=new(0f,0f);
        public Vector2 map_size = new(0,0);
        public Map_creator_and_manager(Play form)
        {
            Form = form;
        }
        /// <summary>
        /// 创建地图生成器
        /// </summary>
        /// <param name="width">区块宽度</param>
        /// <param name="height">区块高度</param>
        public Map_creator_and_manager(int width, int height)
        {
            map_width_block = width;
            map_height_block = height;
        }
        /*
        /// <summary>
        /// 暂行地图生成
        /// </summary>
        /// <returns></returns>
        public List<Platform> Create_map()
        {
            List<Platform> platforms = new List<Platform>();
            float Window_width = Form.ClientSize.Width; // 平台宽度
            float Window_height = Form.ClientSize.Height; // 平台高度
            int num = (int)(Window_height / 150);//生成平台的数量
            MessageBox.Show($"获得的窗口大小{Window_height},{Window_width}");
            Random random = new();
            for (; num > 0; num--)
            {
                float X = (float)random.NextDouble() * Window_width;
                float Y = Window_height * (num / (Window_height / 150));
                Platform platform = new(X, Y);
                //MessageBox.Show($"平台坐标{platform.position.X},{platform.position.Y}");
                platforms.Add(platform);
            }
            return platforms;
        }
        */
        /// <summary>
        /// 根据坐标获得区块索引
        /// </summary>
        /// <param name="p">世界坐标</param>
        /// <returns></returns>
        public static (int X, int Y) Block_index(Vector2 p) => ((int)Math.Floor(p.X / Block.x), (int)Math.Floor(p.Y / Block.y));               
        /// <summary>
        /// 区块管理，区块索引
        /// </summary>
        public readonly Dictionary<(int x, int y), Block> blocks = new();
        public void Create_blocks()
        {
            //创建区块
            for (int x = 0; x < map_width_block; x++)
            {
                for (int y = 0; y < map_height_block; y++)
                {
                    Vector2 point = new Vector2(x * Block.x, y * Block.y);
                    Vector2 size = new Vector2(Block.x, Block.y);
                    Block block = new Block(x,y);
                    blocks.Add((x,y), block);
                }
            }
            map_size = new(Block.x * map_width_block, Block.y * map_height_block);
        }
        /// <summary>
        /// 获取点所在的区块
        /// </summary>
        /// <param name="point">世界坐标</param>
        /// <returns>该点所在的区块</returns>
        public Block? Get_where_block(Vector2 point) => blocks.TryGetValue(Block_index(point), out var block) ? block : null;
        /// <summary>
        /// 获取窗口在世界的位置
        /// </summary>
        /// <param name="point">世界坐标</param>
        /// <param name="matrix_screen">当前矩阵</param>
        /// <returns>应用了世界坐标的矩阵</returns>
        public Matrix Get_matrix_world_to_screen(Vector2 point, Matrix matrix_screen)
        {
            matrix_screen.Reset();
            matrix_screen.Translate(-point.X + (Form.ClientSize.Width / 2), -point.Y + (Form.ClientSize.Height / 2), MatrixOrder.Append);
            return matrix_screen;
        }
        /// <summary>
        /// 用于复用的单位点
        /// </summary>
        private readonly PointF[] _buf = new PointF[1];
        /// <summary>
        /// 把世界坐标转化为屏幕坐标
        /// </summary>
        /// <param name="point">世界坐标</param>
        /// <param name="matrix">转化后的矩阵</param>
        /// <returns>屏幕坐标</returns>
        public Vector2 Get_vector_world_to_screen(Vector2 point, Matrix matrix)
        {
            _buf[0] = new PointF(point.X, point.Y);
            matrix.TransformPoints(_buf);          // 用 Points（含平移），别用 Vectors
            return new Vector2(_buf[0].X, _buf[0].Y);
        }
    }
}
