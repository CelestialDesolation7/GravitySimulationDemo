using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace GravitySimulationDemo
{
    public partial class Form1 : Form
    {
        private Simulator simulator;
        private Renderer renderer;
        public double timeStep = 10800; // 单位：秒，每帧约3小时物理时间
        public double scale = 23376e2; // 比例尺，每一个像素代表的物理距离
        public bool RenderTrajectory = false;
        public Vector2D canvasCenter;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            simulator = new Simulator();
            renderer = new Renderer();
            InitBodies();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();


            timer1.Interval = 16; // 16ms = 60帧/秒
            timer1.Tick += Timer_Tick;
            panelCanvas.Paint += panelCanvas_Paint;
            //panelCanvas.Click += PanelCanvas_Click;

            timer1.Start();
        }

        private void InitBodies()
        {
            // 此处是Demo代码，演示地月系的二维运动，并以地球为中心天体
            // 暂时不关心天体的物理真实半径，视为质点
            // 地球质量为5.972e24kg 
            double earthMass = 5.972e24;
            // 月球质量为7.348e22kg
            double moonMass = 7.348e22;
            // 地月距离为384400km
            double distance = 384400e3;
            // 月球公转速度为1022m/s
            double moonSpeed = 1022;
            // 地球公转速度为0
            double earthSpeed = 0;

            Body centerBody = new Body(
                name_in: "地球",
                position_in: new Vector2D(0, 0),
                velocity_in: new Vector2D(earthSpeed, 0),
                mass_in: earthMass,
                displayRadius_in: 20,
                isCenter_in: true,
                color_in: Color.Blue
            );
            Body moon = new Body(
                name_in: "月球",
                position_in: new Vector2D(distance, 0),
                velocity_in: new Vector2D(0, moonSpeed),
                mass_in: moonMass,
                displayRadius_in: 10,
                isCenter_in: false,
                color_in: Color.Gray
            );
            simulator.AddBody(centerBody);
            simulator.AddBody(moon);

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            simulator.Update(timeStep);
            UpdateDisplayPositions();
            panelCanvas.Invalidate();
        }

        private void PanelCanvas_Click(object sender, EventArgs e)
        {
            simulator.Update(timeStep); // 每次点击手动更新一次物理状态
            UpdateDisplayPositions(); // 更新画布坐标
            panelCanvas.Invalidate(); // 请求重绘画布
        }


        private void UpdateDisplayPositions()
        {
            canvasCenter = new Vector2D(panelCanvas.Width / 2, panelCanvas.Height / 2);
            debugTextBox.Clear();

            foreach (var body in simulator.Bodies)
            {
                if (body.IsCenter) { body.DisplayPosition = canvasCenter; continue; }
                // 此处直接以中心天体为原点，将物理位置转换为画布位置
                var scaledPhysicalPosition = body.Position / scale;
                body.DisplayPosition = (body.Position / scale) + canvasCenter;
                // 调试代码。
                debugTextBox.Clear();
                debugTextBox.AppendText($"名称:{body.name}\r\n");
                debugTextBox.AppendText($"物理位置: {body.Position.X},{body.Position.Y}\r\n");
                debugTextBox.AppendText($"缩放后位置:{scaledPhysicalPosition.X},{scaledPhysicalPosition.Y}\r\n");
                debugTextBox.AppendText($"画布位置: {body.DisplayPosition.X},{body.DisplayPosition.Y}\r\n");
                debugTextBox.AppendText($"速度: {body.Velocity.X},{body.Velocity.Y}\r\n");
                debugTextBox.AppendText($"加速度: {body.Acceleration.X},{body.Acceleration.Y}\r\n");
                debugTextBox.AppendText($"力: {body.Force.X},{body.Force.Y}\r\n");


            }
        }


        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            // 此函数是一个委托，用于绘制画布
            // 在panelCanvas_Paint中，e.Graphics是用于绘制画布的Graphics对象
            // e所包含的参数有：
            // e.Graphics：用于绘制画布的Graphics对象
            // e.ClipRectangle：当前需要绘制的矩形区域
            // 在绘图区域中，坐标系原点在左上角，x轴向右，y轴向下
            renderer.RenderBodies(simulator.Bodies, e.Graphics);
            if (RenderTrajectory)
            {
                renderer.RenderTrajectory(simulator.Bodies, e.Graphics);
            }
        }
    }
}
