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
        decimal capacity;

        public Pump(Point position, decimal capacity, decimal flow ) : base(position)
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
