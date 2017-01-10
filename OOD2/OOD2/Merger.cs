using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    class Merger : Component
    {
        public Pipeline InputA;
        public Pipeline InputB;
        private decimal flow2;

        public Merger(Point position) : base(position)
        {

        }
        public bool addInput(Pipeline pipeline)
        {
            if(InputA == null)
            {
                InputA = pipeline;
                return true;
            }
            else if(InputB == null && InputB != null)
            {
                InputB = pipeline;
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool setFlow(decimal flow)
        {
            if(this.flow != -1)
            {
                flow2 = flow;
            }
            else
            {
                this.flow = flow;
            }
            return SetOutputFlow();
        }
        public override decimal getFlow()
        {
            decimal f = flow;
            if (flow2 != -1)
            {
                f += flow2;
            }
            return f;
            
        }
        public override bool GetFlowFromInput()
        {
            if(InputA == null && InputB == null)
            {
                return false;
            }
            decimal theFlow = 0;
            if (InputB == null)
            {
                if (InputA.getFlow() != -1)
                {
                    this.flow = InputA.getFlow();
                }
                
            }
            
                
                if(InputA.getFlow() != -1)
                {
                    theFlow += InputA.getFlow();
                }
                if(InputB != null)
                 {
                    if (InputB.getFlow() != -1)
                    {
                        theFlow += InputB.getFlow();
                    }
                 }
               
                if (theFlow != 0)
                {
                    this.flow = theFlow;
                    return true;
                }
                 return false;

            
        }

        public override void DrawSelf()
        {
            throw new NotImplementedException();
        }
    }
}
