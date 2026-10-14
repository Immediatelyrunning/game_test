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
        public float friction = 0.7f; // 摩擦系数，值越小摩擦力越大
        public Vector2 Point_upleft;//左上角坐标,一般在角色左上角
        public Vector2 Point_downright;//右下角坐标，
        public float left => Point_downright.Y - Point_upleft.Y;
        public float right => left;//左右长度一致
        public float top => Point_downright.X - Point_upleft.X;
        public float buttom => top;
        public Vector2 center_point => (Point_upleft + Point_downright) / 2;
        /// <summary>
        /// 碰撞箱矩形
        /// </summary>
        public RectangleF Collider_rectangle;
        /// <summary>
        /// 创建碰撞箱
        /// </summary>
        /// <param name="point">角色的坐标</param>
        public Collider(Vector2 point, float width, float height)
        {
            Point_upleft = point;
            Point_downright = new Vector2(point.X + width, point.Y + height);
            width_lenth = width;
            height_lenth = height;
            Collider_rectangle = new RectangleF(point.X,point.Y, width_lenth, height_lenth);
        }
        /// <summary>
        /// 移动碰撞箱矩形
        /// </summary>
        /// <param name="point">新的左上角坐标</param>
        public Collider Collider_moved_rectangle(Vector2 point)
        {
            this.Collider_rectangle.Location = (PointF)point;
            return this;
        }
        /// <summary>
        /// 移动碰撞箱
        /// </summary>
        /// <param name="point">新的左上角坐标</param>
        public Collider Collider_moved_point(Vector2 point)
        {
            this.Point_upleft = point;
            this.Point_downright = Point_moved(point,width_lenth,height_lenth);
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
        /// <summary>
        /// 检测碰撞箱矩形是否重叠
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Is_colliding(Collider other)
        {
            return Collider_rectangle.IntersectsWith(other.Collider_rectangle);
        }
        public bool IsColliding(Collider other)
        {
            // 检查两个碰撞箱是否重叠
            return !(Point_downright.X < other.Point_upleft.X || // this is left of other
                     Point_upleft.X > other.Point_downright.X || // this is right of other
                     Point_downright.Y < other.Point_upleft.Y || // this is above other
                     Point_upleft.Y > other.Point_downright.Y);  // this is below other
        }
    }
}
