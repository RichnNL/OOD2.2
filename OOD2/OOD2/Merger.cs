﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    [Serializable]
    class Merger : Component
    {
        public Pipeline InputA;
        public Pipeline InputB;
        private decimal flow2;

        public Merger(Point position) : base(position)
        {
            flow2 = -1;
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
        public override void setFlow(decimal flow)
        {
          if(InputA != null && InputB == null)
            {
                this.flow = flow;
                if(Output!= null)
                {
                    Output.setFlow(this.flow);
                }
            }
          else if(InputA != null && InputB != null)
            {
                this.flow = InputA.getFlow();
                this.flow2 = InputB.getFlow();
                if(Output != null)
                {
                    Output.setFlow(flow + flow2);
                }
            }
            
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
       
        public void removeInputB()
        {
            this.InputB = null;
            flow2 = -1;
        }

        public override void removeInput()
        {
            this.InputA = null;
            if(InputB != null)
            {
                this.InputA = InputB;
                InputB = null;
                flow = flow2;
                flow2 = -1;
                setFlow(flow);
            }
            else
            {
                flow = -1;
                flow2 = -1;
                setFlow(flow);
            }
            
        }
        public override bool addInputPipeline(Pipeline pipeline)
        {
            if (this.InputA == null && this.InputB == null)
            {
                this.InputA = pipeline;
                if (pipeline.getFlow() != -1)
                {
                    this.flow = pipeline.getFlow();
                }
               
            }
            else if(this.InputA != null && this.InputB == null)
            {
                this.InputB = pipeline;
                this.flow2 = pipeline.getFlow();
                
            }
            else if(this.InputA == null && this.InputB != null)
            {
                this.InputA = pipeline;
                this.flow = pipeline.getFlow();
            }
            else
            {
                return false;
            }
            if(Output != null)
            {
                if (flow != -1 && flow2 != -1)
                {
                    Output.setFlow(flow + flow2);
                }
                else if (flow2 == -1 && flow != -1)
                {
                    Output.setFlow(flow);
                }
                else if (flow2 != -1 && flow == -1)
                {
                    Output.setFlow(flow2);
                }
            }
          
            return true;

        }
        
    }
}
