using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    class Pump : Component
    {
        double capacity;

        public Pump(Graphics g, Point position, double capacity, double flow ) : base(g,position)
        {
            this.capacity = capacity;
            this.flow = flow;
        }

        public override void DrawSelf()
        {
            throw new NotImplementedException();
        }

        public override bool GetFlowFromInput()
        {
            return false;
        }
    }
}
