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
        
        public Component(Point position)
        {
            this.position = position;
            
        }
        public Component(Graphics g, Point position, Pipeline output)
        {
            this.position = position;
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
                return false;
            }
        }
       
    }
}
