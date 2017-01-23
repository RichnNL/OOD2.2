using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    [Serializable]
    class Pump : Component
    {
        public decimal capacity;

        public Pump(Point position, decimal capacity, decimal flow ) : base(position)
        {
            this.capacity = capacity;
            this.flow = flow;
        }

        public override void removeInput()
        {
            return;
        }
        public override bool addInputPipeline(Pipeline pipeline)
        {
            return false;

        }
       




    }
}
