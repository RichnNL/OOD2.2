using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    abstract class Item
    {
        protected decimal flow;   
        public Item()
        {
            this.flow = -1;

        }  
        public virtual decimal getFlow()
        {
            return flow;
        }
     
        public virtual void setFlow(decimal flow)
        {
            this.flow = flow;
            
        }
        public abstract Component getNextComponent();
        public abstract Pipeline getNextPipeline();
       

    }
}
