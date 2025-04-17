using System;
using System.Drawing;

namespace GravitySimulationDemo
{
    public class Body
    {
        public string Name { get; set; }
        // 物理属性 
        // 仅在初始化时对用户可见，之后不再修改
        public Vector2D Position { get; set; }           // 物理真实位置
        public Vector2D Velocity { get; set; }           // 速度
        public double Mass { get; set; }                 // 质量
        // 永远对用户隐藏
        public Vector2D Acceleration { get; set; }       // 当前加速度
        public Vector2D Force { get; set; }               // 当前受到的力 

        // 绘制属性
        // 仅在初始化时对用户可见，之后不再修改
        public int RenderRadius { get; set; }        // 绘制半径
        public bool IsCenter { get; set; }              // 是否为中心天体
        public Color DisplayColor { get; set; }                // 绘制颜色
        // 永远对用户隐藏
        public Vector2D DisplayPosition { get; set; }    // 画布上的位置

        public Body(string name_in, Vector2D position_in, Vector2D velocity_in, double mass_in, int displayRadius_in, bool isCenter_in, Color color_in)
        {
            Name = name_in;
            Position = position_in;
            Velocity = velocity_in;
            Mass = mass_in;
            IsCenter = isCenter_in;
            DisplayColor = color_in;
            RenderRadius = displayRadius_in;

            Force = Vector2D.ZeroVector; // 初始化力为零
            Acceleration = Vector2D.ZeroVector; // 初始化加速度为零
        }
    }

    // 简单二维向量类
    public struct Vector2D
    {
        private double x, y;
        private double length;

        public double X
        {
            get => x;
            set
            {
                x = value;
                length = Math.Sqrt(x * x + y * y);
            }
        }

        public double Y
        {
            get => y;
            set
            {
                y = value;
                length = Math.Sqrt(x * x + y * y);
            }
        }

        public double Length => length;

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
            length = Math.Sqrt(x * x + y * y);
        }

        // 运算符重载
        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator *(Vector2D a, double k) => new Vector2D(a.X * k, a.Y * k);
        public static Vector2D operator *(double k, Vector2D a) => a * k;
        public static Vector2D operator /(Vector2D a, double k) => new Vector2D(a.X / k, a.Y / k);
        public static Vector2D operator -(Vector2D a) => new Vector2D(-a.X, -a.Y);

        public static Vector2D ZeroVector => new Vector2D(0, 0);

        public Vector2D Normalize() => length == 0 ? ZeroVector : this * (1.0 / length);
    }

}
