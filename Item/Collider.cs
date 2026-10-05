using System.Numerics;

namespace game_test.Item
{
    ///<summary>
    ///此单独类为碰撞箱类，我应该在这里加一个bool值来表示物体是否发生了碰撞
    ///这个碰撞箱只有矩形
    ///</summary>
    public class Collider
    {
        float width_lenth;//获得宽度
        float height_lenth;//获得高度
        public float friction = 0.80f; // 摩擦系数，值越小摩擦力越大
        public Vector2 Point_left;//左上角坐标,一般在角色左上角
        public Vector2 Point_right;//右下角坐标，
        public float left => Point_right.Y - Point_left.Y;
        public float right => left;//左右长度一致
        public float top => Point_right.X - Point_left.X;
        public float buttom => top;
        /// <summary>
        /// 创建碰撞箱
        /// </summary>
        /// <param name="point">角色的坐标</param>
        public Collider(Vector2 point, float width, float height)
        {
            Point_left = point;
            Point_right = new Vector2(point.X + width, point.Y + height);
            width_lenth = width;
            height_lenth = height;
        }
        /// <summary>
        /// 移动碰撞箱
        /// </summary>
        /// <param name="point">新的左上角坐标</param>
        public Collider Collider_moved(Vector2 point)
        {
            this.Point_left = point;
            this.Point_right = Point_moved(point,width_lenth,height_lenth);
            return this;
        }
        /// <summary>
        /// 右下角碰撞箱坐标
        /// 用于节省new开销
        /// </summary>
        /// <param name="point"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        Vector2 Point_moved(Vector2 point,float width,float height)
        {
            point.X += width;
            point.Y += height;
            return point;
        }
        public bool IsColliding(Collider other)
        {
            // 检查两个碰撞箱是否重叠
            return !(Point_right.X < other.Point_left.X || // this is left of other
                     Point_left.X > other.Point_right.X || // this is right of other
                     Point_right.Y < other.Point_left.Y || // this is above other
                     Point_left.Y > other.Point_right.Y);  // this is below other
        }
    }
}
