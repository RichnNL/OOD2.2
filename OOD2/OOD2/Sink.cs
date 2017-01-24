using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    [Serializable]
    class Sink : Component
    {
        
        public Sink(Point position) : base(position)
        {
            
        }

        public override void removeInput()
        {
            Output = null;
            this.flow = -1;
        }
        public override bool addInputPipeline(Pipeline pipeline)
        {
            if (this.Output == null)
            {
                this.Output = pipeline;
                if (pipeline.getFlow() != -1)
                {
                    this.flow = pipeline.getFlow();
                    
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        public override void setFlow(decimal flow)
        {
            this.flow = flow;
        }
    }
}
