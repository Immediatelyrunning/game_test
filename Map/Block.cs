using game_test.Item;
using System.Numerics;
namespace game_test.Map
{
    /// <summary>
    /// 区块，加载的最小单位
    /// 可用多线程进行更新
    /// </summary>
    internal class Block
    {
        public List<Platform> platforms = new List<Platform>();
        /// <summary>
        /// 区块左上角的点
        /// </summary>
        public Vector2 Point_upleft;
        /// <summary>
        /// 区块右下角的点
        /// </summary>
        public Vector2 Point_downright => new Vector2(Point_upleft.X + x, Point_upleft.Y + y);
        public const int x = 1024;
        public const int y = 1024;
        /// <summary>
        /// 创建区块
        /// </summary>
        /// <param name="point">区块坐标，左上角的点</param>
        /// <summary>区块整数索引，支持负数</summary>
        public readonly (int X, int Y) Index;
        public bool Loaded { get; private set; }
        public Block(int bx, int by) => Index = (bx, by);

        public void Load()
        {
            if (Loaded) return;
            // 用索引做种子：确定性生成，来回走地形不变
            var random = new Random(HashCode.Combine(Index.X, Index.Y));
            for (int i = 0; i < 50; i++)
            {
                platforms.Add(new Platform(
                                    ((float)random.NextDouble() * x) + Point_upleft.X,
                                    ((float)random.NextDouble() * y) + Point_upleft.Y));
            }
            Loaded = true;
        }
        public void Unload() { platforms.Clear(); Loaded = false; }
        public bool Intersects(float l, float t, float r, float b)
            => Point_upleft.X < r && Point_downright.X > l
            && Point_upleft.Y < b && Point_downright.Y > t;
    }
}
