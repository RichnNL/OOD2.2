using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    class PipelineNetwork
    {
        private string PipelineName;
        private Item selectedItem;
        private Item selectedItem2;
        private List<Item> items;
        private int ComponentSize;
        private int pipelineWidth;
        SaveLoad fileHandler;
        public PipelineNetwork()
        {
            items = new List<Item>();
            ComponentSize = 20;
            pipelineWidth = 5;
        }
        public bool addComponent(Item i, Point pos)
        {
            if (CanPlaceItem(pos))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool remove(Item i)
        {
            return true;
        }
        public bool addPipeline(Item i, Item i2)
        {
            if(CheckForLoop(i,i2) && pathClear(i,i2))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public void calculateNetwork()
        {

        }
        public Item getItem(Point p)
        {
            Item i;
            return i;
        }
        private bool pathClear(Item i, Item i2)
        {
            return true;
        }
        private bool CheckForLoop(Item i, Item i2)
        {
            return true;
        }
        private bool CanPlaceItem(Point pos)
        {
            foreach(Item i in items)
            {
                if(i.GetType() == typeof(Component))
                {
                    // The item is a component 
                    if (ComponentInPosition((Component)i, pos))
                    {
                        return false;
                    }

                }
                else
                {
                    //the item is a pipeline
                    if (PipelineInPosition((Pipeline)i, pos))
                        {
                        return false;
                        }

                }
            }
            return true;
        }
        public bool ReplaceComponent(Item i)
        {
            return true;
        }
        private bool checkCanConnect(Item i, Item i2)
        {
            return true;
        }
        private Graphics getGraphic(Item i)
        {

        }
       
        private bool ComponentInPosition(Component c , Point pos)
        {
            Point topLeft = c.getPosition();
            Point bottomRight = c.getPosition();
            topLeft.X -= (ComponentSize / 2);
            topLeft.Y -= (ComponentSize / 2);
            bottomRight.X += (ComponentSize / 2);
            bottomRight.Y += (ComponentSize / 2);
            if (topLeft.X >= pos.X && bottomRight.X <= pos.X)
            {
                if (topLeft.Y >= pos.Y && bottomRight.Y <= pos.Y)
                {
                    return true;
                }
            }
            return false;
        }

        private bool PipelineInPosition(Pipeline p ,Point pos)
        {
            //First need to get the four points of the pipeline
            Point slope = new Point();
            slope.Y = p.output.getPosition().Y - p.input.getPosition().Y;
            slope.X = p.output.getPosition().X - p.input.getPosition().X;
            Point temp = slope;
            // Get the perpendicular slope
            slope.Y = -(slope.X);
            slope.X = -(temp.Y);
            Point A = new Point(); //Top Left
            Point B = new Point(); //Top Right
            Point C = new Point(); //Bottom Right
            Point D = new Point(); //Bottom Left
            A.X = p.input.getPosition().X + (slope.X * (pipelineWidth / 2));
            A.Y = p.input.getPosition().Y + (slope.Y * (pipelineWidth / 2));
            B.X = p.input.getPosition().X - (slope.X * (pipelineWidth / 2));
            B.Y = p.input.getPosition().Y - (slope.Y * (pipelineWidth / 2));
            D.X = p.output.getPosition().X + (slope.X * (pipelineWidth / 2));
            D.Y = p.output.getPosition().Y - (slope.Y * (pipelineWidth / 2));
            C.X = p.output.getPosition().X + (slope.X * (pipelineWidth / 2));
            C.Y = p.output.getPosition().Y - (slope.Y * (pipelineWidth / 2));

            double PipelineArea = getArea(A, B, C, D);
            double APD = getArea(A, pos, D);
            double DPC = getArea(D, pos, C);
            double CPB = getArea(C, pos, B);
            double PBA = getArea(pos, B, A);

            double PointArea = APD + DPC + CPB + PBA;
            if(PointArea < PipelineArea)
            {
                return true;
            }
            else
            {
                return false;
            }



        }
        private void getDistance(Point A, Point B, Point C, out double ab, out double ac , out double bc)
        {
            ab = Math.Sqrt(Math.Pow((B.X - A.X), 2) + Math.Pow((B.Y - A.Y), 2));
            ac = Math.Sqrt(Math.Pow((C.X - A.X), 2) + Math.Pow((C.Y - A.Y), 2));
            bc = Math.Sqrt(Math.Pow((C.X - B.X), 2) + Math.Pow((C.Y - B.Y), 2));
        }
        private double getArea(Point A, Point B, Point C)
        {
            double ab, ac, bc;
            getDistance(A, B, C,out ab, out ac, out bc);
            //Herons formula
            double s = (ab + ac + bc) / 2;
            double area = Math.Pow((s*((s - ab)*(s - ac)*(s - bc))), 2);
            return area;
        }
        private double getArea(Point A,Point B, Point C,Point D)
        {
            double width = Math.Sqrt(Math.Pow((B.X - A.X), 2) + Math.Pow((B.Y - A.Y), 2));
            double length = Math.Sqrt(Math.Pow((C.X - A.X), 2) + Math.Pow((C.Y - A.Y), 2));
            return width * length;
        }
    }
}
