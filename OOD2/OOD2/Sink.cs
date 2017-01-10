using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    class Sink : Component
    {
        
        public Sink(Point position) : base(position)
        {
            
        }
        public override void DrawSelf()
        {
            throw new NotImplementedException();
        }

        public override bool GetFlowFromInput()
        {
            if(Output != null)
            {
                if(Output.getFlow() != -1)
                {
                    this.flow = Output.getFlow();
                    return true;
                }
                
            }
            return false;
        }

        public override bool SetOutputFlow()
        {
            if(Output != null)
            {
                return true;
            }
            return false;
        }
    }
}
