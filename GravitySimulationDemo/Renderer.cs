using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GravitySimulationDemo
{
    public class Renderer
    {
        public Renderer()
        {
            // 初始化
        }
        public void RenderBodies(List<Body> bodies, Graphics graphics)
        {
            foreach (var body in bodies)
            {
                graphics.FillEllipse(
                    new SolidBrush(body.DisplayColor),
                    (float)(body.DisplayPosition.X - body.RenderRadius),
                    (float)(body.DisplayPosition.Y - body.RenderRadius),
                    body.RenderRadius * 2,
                    body.RenderRadius * 2);
            }
        }
        public void RenderTrajectory(List<Body> bodies, Graphics graphics)
        {
            // Demo中暂不实现  
        }
        public void RenderText(List<Body> bodies, Graphics graphics)
        {
            // Demo中暂不实现  
        }

    }
}
