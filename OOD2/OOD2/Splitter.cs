using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    [Serializable]
    class Splitter : Component
    {
        public Pipeline Input;
        public Pipeline OutputB;
        private int adjustmenpercentage;
        public int adjustmentPercentage { get { return adjustmenpercentage; } set {
                if (value != adjustmenpercentage)
                {
                    adjustmenpercentage = value;
                    this.setFlow(this.flow);
                }
            }
        }
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
      
     

        public override void setFlow(decimal flow)
        {
            this.flow = flow;
            if (OutputB == null && Output != null)
            {
                setFlow(this.flow);
            }
            else if(Output != null && OutputB != null)
            {
                decimal flow1 = (flow * adjustmentPercentage) / 100;
                decimal flow2 = flow1 - flow;
                Output.setFlow(flow1);
                OutputB.setFlow(flow2);
            }
            else
            {
               
            }
        }


        public override void removeInput()
        {
            Output = null;
            this.flow = -1;
        }

        public override bool addInputPipeline(Pipeline pipeline)
        {
            if(this.Input == null)
            {
                this.Input = pipeline;
                if (pipeline.getFlow() != -1)
                {
                    this.flow = pipeline.getFlow();
                    setFlow(flow);
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
