using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    class Splitter : Component
    {
        public Pipeline Input;
        public Pipeline OutputB;
        public int adjustmentPercentage { get; set; }
        public Splitter(Point position, int percentage) :base(position)
        {
            this.adjustmentPercentage = percentage;
        }
        public bool addOutputB(Pipeline pipeline)
        {
            if(OutputB == null)
            {
                this.Output = pipeline;
                return true;
            }
            else{
                return false;
            }
            
        }
        public bool addInput(Pipeline pipeline)
        {
            
            if (Input == null)
            {
                this.Input = pipeline;
                return true;
            }
            else {
                return false;
            }
        }
        public override bool SetOutputFlow()
        {
            if(OutputB == null)
            {
                return base.SetOutputFlow();
            }
            else
            {
                decimal flow1 = (flow * adjustmentPercentage)/100;
                decimal flow2 = flow1 - flow;
                bool o1 = Output.setFlow(flow1);
                bool o2 = OutputB.setFlow(flow2);
                if( o1 || o2)
                {
                    return true;
                }
                else{
                    return false;
                }
            }
        }
        public override bool addOutput(Pipeline pipeline)
        {
            if(Output == null)
            {
               return base.addOutput(pipeline);
            }
            else if(OutputB == null && Output != null)
            {
                OutputB = pipeline;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool GetFlowFromInput()
        {
          if(Input != null)
            {
                if(Input.getFlow() != -1)
                {
                    this.flow = Input.getFlow();
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public override void DrawSelf()
        {
            throw new NotImplementedException();
        }
    }
}
