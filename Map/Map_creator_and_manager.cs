using game_test.Item;
using System.Numerics;
namespace game_test.Map
{
    /// <summary>
    /// 地图生成器,管理器
    /// </summary>
    internal class Map_creator_and_manager
    {
        /// <summary>
        /// 地图尺寸,世界坐标系的参考，奇了怪了winform真的挺难做负坐标
        /// </summary>
        SizeF map_size;
        Form Form;
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
        Vector2 map_center;
        int[,] map_data;
        public Map_creator_and_manager(Form form)
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
            map_data = new int[width, height];
        }
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
            //MessageBox.Show($"获得的窗口大小{Window_height},{Window_width}");
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
        List<Block> blocks = new List<Block>();
        public void Create_block()
        {
            //创建区块
            for (int x = 0; x < map_width_block; x++)
            {
                for (int y = 0; y < map_height_block; y++)
                {
                    Vector2 point = new Vector2(x * Block.x, y * Block.y);
                    Size size = new Size(Block.x, Block.y);
                    Block block = new Block(point);
                    map_size+=size;
                    blocks.Add(block);
                }
            }
            map_center = new Vector2(map_size.Width / 2, map_size.Height / 2);
        }
        /// <summary>
        /// 将世界坐标转换为以地图中心为参考点的坐标
        /// </summary>
        /// <param name="point">角色相对于地图（左上角为0，0）的坐标</param>
        /// <returns>中心系坐标</returns>
        public Vector2 Get_point_based_on_center(Vector2 point)
        {
            point.X -= map_center.X;
            point.Y -= map_center.Y;
            return point;
        }
        /// <summary>
        /// 获取点所在的区块
        /// </summary>
        /// <param name="point"></param>
        /// <returns>该点所在的区块</returns>
        public Block? Get_where_block(Vector2 point)
        {
            Block block = blocks.Find((block) =>
            {
                return point.X >= block.Point_upleft.X && point.X <= block.Point_downright.X && point.Y >= block.Point_upleft.Y && point.Y <= block.Point_downright.Y;
            });
            return block;
        }
    }
}
