using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    [Serializable]
    abstract class Component : Item
    {
        public Pipeline Output;
        private Point position;
        public string direction { get; set; }
       
        
        public Component(Point position)
        {
            this.position = position;
            
        }
        public Component(Graphics g, Point position, Pipeline output)
        {
            this.position = position;
            this.Output = output;
           
        }
        public abstract bool addInputPipeline(Pipeline pipeline);
       
        public Point getPosition()
        {
            return position;
        }
        public override void setFlow(decimal flow)
        {
            this.flow = flow;
            if(Output != null)
            {
                Output.setFlow(flow);
            }
            
        }
        public abstract void removeInput();
      
        public override Component getNextComponent()
        {
            if (Output != null)
            {

                return Output.getNextComponent();
            }
            else
            {
                return null;
            }
        }

        public override Pipeline getNextPipeline()
        {
            if (Output != null)
            {
                return Output;
            }
            else
            {
                return null;
            }
        }
        public virtual void addOutput(Pipeline pipeline)
        {
            this.Output = pipeline;
        }
    }
}
