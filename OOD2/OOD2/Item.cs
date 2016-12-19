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
        public double flow;
        public bool isConnectedToPump;
        public bool isConnectedToSink;
        private Graphics graphics;
        public Item(Graphics g)
        {
            this.flow = -1;
            this.graphics = g;
            if (GetFlowFromInput())
            {
                SetOutputFlow();
            }
        }
        public Item()
        {
            this.flow = -1;
            if (GetFlowFromInput())
            {
                SetOutputFlow();
            }
           
        }
        public abstract bool SetOutputFlow();
        public abstract bool GetFlowFromInput();
        public abstract void DrawSelf();
        public virtual double getFlow()
        {
            return flow;
        }
        public void setDraw(Graphics g)
        {
            this.graphics = g;
        }
        public virtual bool setFlow(double flow)
        {
            this.flow = flow;
            return SetOutputFlow();
        }
       

    }
}
