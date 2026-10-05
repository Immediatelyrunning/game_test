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
        /// <summary>
        /// 区块左上角的点
        /// </summary>
        public Vector2 Point_upleft;
        /// <summary>
        /// 区块右下角的点
        /// </summary>
        public Vector2 Point_downright => new Vector2(Point_upleft.X + x, Point_upleft.Y + y);
        public static int x = 2048;
        public static int y = 2048;
        /// <summary>
        /// 创建区块
        /// </summary>
        /// <param name="point">区块坐标，左上角的点</param>
        public Block(Vector2 point)
        {
            Point_upleft = point;
        }
    }
}
