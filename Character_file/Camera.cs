using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace game_test.Character_file
{
    /// <summary>
    /// 摄像机，获取视野，分区渲染
    /// </summary>
    internal class Camera
    {
        public float X, Y;              // 视口左上角对应的世界坐标
        public float Zoom = 1f;
        public float FollowSpeed = 8f;  // 越大跟得越紧（帧率无关）
        public SizeF DeadZone = new SizeF(90, 70);  // 死区半宽/半高
        public float LookAhead = 0.35f; // 沿速度方向前瞻（秒）
        /// <summary>跟随目标。view=画布尺寸, world=地图尺寸, dt=本帧秒数</summary>
        public void Follow(RectangleF target, PointF velocity, Size view, Size world, float dt)
        {
            float halfW = view.Width / (2f * Zoom);
            float halfH = view.Height / (2f * Zoom);
            // 摄像机中心
            float camCX = X + halfW, camCY = Y + halfH;
            // 目标中心 + 前瞻
            float tgtCX = target.X + (target.Width / 2f) + (velocity.X * LookAhead);
            float tgtCY = target.Y + (target.Height / 2f) + (velocity.Y * LookAhead);
            // ① 死区：只有"超出死区的那部分"才推动摄像机
            float dx = tgtCX - camCX, dy = tgtCY - camCY;
            float pushX = Math.Abs(dx) > DeadZone.Width ? dx - (Math.Sign(dx) * DeadZone.Width) : 0;
            float pushY = Math.Abs(dy) > DeadZone.Height ? dy - (Math.Sign(dy) * DeadZone.Height) : 0;
            // ② 帧率无关的指数平滑（比 cam += (t-cam)*0.1 更稳）
            float k = 1f - (float)Math.Exp(-FollowSpeed * dt);
            camCX += pushX * k;
            camCY += pushY * k;
            // ③ 中心 → 左上角，并钳制在地图内
            X = camCX - halfW;
            Y = camCY - halfH;
            X = Clamp(X, 0, Math.Max(0, world.Width - (view.Width / Zoom)));
            Y = Clamp(Y, 0, Math.Max(0, world.Height - (view.Height / Zoom)));
        }
        /// <summary>世界 → 屏幕 的变换矩阵（注意：先平移后缩放）</summary>
        public Matrix GetMatrix()
        {
            var m = new Matrix();
            m.Translate(-X , -Y , MatrixOrder.Append);
            m.Scale(Zoom, Zoom, MatrixOrder.Append);   // 顺序反了缩放就会错位
            return m;
        }
        /// <summary>可见世界矩形，用于剔除</summary>
        public RectangleF ViewRect(Size view) =>
            new RectangleF(X, Y, view.Width / Zoom, view.Height / Zoom);

        /// <summary>屏幕 → 世界（鼠标点选）</summary>
        public PointF ScreenToWorld(Point screen)
        {
            var m = GetMatrix();
            m.Invert();
            var p = new[] { new PointF(screen.X, screen.Y) };
            m.TransformPoints(p);
            return p[0];
        }
        static float Clamp(float v, float lo, float hi) => v < lo ? lo : v > hi ? hi : v;
    }
}

