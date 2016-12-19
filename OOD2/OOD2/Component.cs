using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    abstract class Component : Item
    {
        public Pipeline Output;
        private Point position;
        private Graphics graphics;
        public Component(Graphics g, Point position)
        {
            this.position = position;
            this.graphics = g;
        }
        public Component(Graphics g, Point position, Pipeline output)
        {
            this.position = position;
            this.graphics = g;
            this.Output = output;
           
        }
        public virtual bool addOutput(Pipeline pipeline)
        {
            if(Output == null)
            {
                Output = pipeline;
                return true;
            }
            else
            {
                return false;
            }
            
           
        }
        public Point getPosition()
        {
            return position;
        }
        public override bool SetOutputFlow()
        {
            if (Output != null)
            {
                if(this.flow == -1)
                {
                    if (GetFlowFromInput())
                    {
                        Output.setFlow(getFlow());
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Output.setFlow(getFlow());
                    return true;
                }
               
               
            }
            else
            {
                return false;
            }
        }
       
    }
}
