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
        public delegate void SelectedItem1ChangedHandler(Item selectedItem);
        public delegate void NetWorkErrorHandler(string error);
        public delegate void DrawComponentHandler(bool Draw, string component, Point Position, int HeightWidth);
        public delegate void DrawPipelinehandler(bool Draw, bool Green, Point Component1Position, Point Component2Position, int height);
        public event DrawPipelinehandler drawPipelineEvent;
        public event DrawComponentHandler drawComponentEvent;
        public event NetWorkErrorHandler NetworkErrorEvent;
        public event SelectedItem1ChangedHandler SelectedItem1Event;
        public PipelineNetwork()
        {
            items = new List<Item>();
            ComponentSize = 20;
            pipelineWidth = 5;
        }

        public void disselect()
        {
            selectedItem = null;
            selectedItem2 = null;
        }
        public void NetworkClicked(Point position)
        {
            Item item = getItem(position);
            if(item != null)
            {
                
                    selectedItem = item;
                    if (SelectedItem1Event != null)
                    {
                        SelectedItem1Event(selectedItem);
                    }
                
                
            }
            else
            {
                disselect();
                if (SelectedItem1Event != null)
                {
                    SelectedItem1Event(null);
                }
            }
        }
        public void NetworkDoubleClicked(Point Position,decimal safetyLimit)
        {
            Item item = getItem(Position);
            if (item != null)
            {

                selectedItem2 = item;
                if(selectedItem.GetType() != typeof(Pipeline) && selectedItem2.GetType() != typeof(Pipeline))
                {
                    if (addPipeline(((Component)selectedItem), ((Component)selectedItem2), safetyLimit))
                    {
                        // disselect components? message box after pipeline success??
                    }
                }


            }
          
        }
        public bool addComponent(string Component, Point pos, decimal value, decimal flow )
        {
            Component c = null;
            if (CanPlaceItem(pos))
            {
                if(Component == "Sink")
                {
                    c = new Sink(pos);
                    
                }
                else if(Component == "Merger")
                {
                    c = new Merger(pos);
                }
                else if(Component == "Splitter")
                {
                    c = new Splitter(pos,Convert.ToInt32(value));
                }
                else if(Component == "Pump")
                {
                    c = new Pump(pos, value, flow);
                }
                if(drawComponentEvent != null && c != null)
                {
                    drawComponentEvent(true, c.GetType().ToString(), pos, ComponentSize);
                    items.Add(c);
                    return true;
                }
                return false;
            }
            else
            {
              
                return false;
            }
        }
        
        
        private void remove(Item i)
        {
            if (i.GetType() == typeof(Pipeline))
            {
                if (drawPipelineEvent != null)
                {
                    //undraw pipeline
                    drawPipelineEvent(false, true, ((Pipeline)i).input.getPosition(), ((Pipeline)i).output.getPosition(), pipelineWidth);
                }
                if (drawComponentEvent != null)
                {
                    //redraw components
                    drawComponentEvent(true, ((Pipeline)i).input.GetType().ToString(), ((Pipeline)i).input.getPosition(), ComponentSize);
                    drawComponentEvent(true, ((Pipeline)i).output.GetType().ToString(), ((Pipeline)i).output.getPosition(), ComponentSize);
                }
                i = null;
                items.Remove(i);
            }
            else
            {
                //Item is a Component
                //First must delete Pipelines Connected to Components
                if (((Component)i).Output != null)
                {
                    remove(((Component)i).Output);
                }
                if (i.GetType() == typeof(Splitter))
                {
                    if (((Splitter)i).Input != null)
                    {
                        remove(((Splitter)i).Input);
                    }
                    else if (((Splitter)i).OutputB != null)
                    {
                        remove(((Splitter)i).OutputB);
                    }
                }
                else if (i.GetType() == typeof(Merger))
                {
                    if (((Merger)i).InputA != null)
                    {
                        remove(((Merger)i).InputA);
                    }
                    else if (((Merger)i).InputB != null)
                    {
                        remove(((Merger)i).InputB);
                    }
                }
                if (drawComponentEvent != null)
                {
                    //Undraw Component
                    drawComponentEvent(false, i.GetType().ToString(), ((Component)i).getPosition(), ComponentSize);
                }
                i = null;
                items.Remove(i);
            }
        }
        public void remove(Point position)
        {
            Item i = getItem(position);
            if (i.GetType() == typeof(Pipeline))
            {
                if (drawPipelineEvent != null)
                {
                    //undraw pipeline
                    drawPipelineEvent(false,true, ((Pipeline)i).input.getPosition(), ((Pipeline)i).output.getPosition(), pipelineWidth);
                }
                if (drawComponentEvent != null)
                {
                    //redraw components
                    drawComponentEvent(true, ((Pipeline)i).input.GetType().ToString(), ((Pipeline)i).input.getPosition(), ComponentSize);
                    drawComponentEvent(true, ((Pipeline)i).output.GetType().ToString(), ((Pipeline)i).output.getPosition(), ComponentSize);
                }
                i = null;
                items.Remove(i); 
            }
            else
            {
                //Item is a Component
                //First must delete Pipelines Connected to Components
                if (((Component)i).Output != null)
                {
                    remove(((Component)i).Output);
                }
                if (i.GetType() == typeof(Splitter))
                {
                    if (((Splitter)i).Input != null)
                    {
                        remove(((Splitter)i).Input);
                    }
                    else if(((Splitter)i).OutputB != null)
                    {
                        remove(((Splitter)i).OutputB);
                    }
                }
                else if(i.GetType() == typeof(Merger))
                {
                    if (((Merger)i).InputA != null)
                    {
                        remove(((Merger)i).InputA);
                    }
                    else if (((Merger)i).InputB != null)
                    {
                        remove(((Merger)i).InputB);
                    }
                }
                if (drawComponentEvent != null)
                {
                    //Undraw Component
                    drawComponentEvent(false, i.GetType().ToString(), ((Component)i).getPosition(), ComponentSize);
                }
                i = null;
                items.Remove(i);
            }
        }
        public bool addPipeline(Component c1Output, Component c2Input, decimal safetyLimit)
        {
            if(HasLoop(c1Output,c2Input) && pathClear(c1Output,c2Input) && checkCanConnect(c1Output,c2Input))
            {
                Pipeline pipe = new Pipeline(c1Output, c2Input, safetyLimit);
                items.Add(pipe);
                bool green;
                Pipeline next = pipe;
                while(next != null)
                {
                    if(next.flow != -1 && next.getFlow() > next.safetyLimit)
                    {
                        green = false;
                    }
                    else
                    {
                        green = true;
                    }
                    if (drawPipelineEvent != null)
                    {
                        drawPipelineEvent(true, green, next.input.getPosition(), next.output.getPosition(), pipelineWidth);
                        if(drawComponentEvent != null)
                        {
                            drawComponentEvent(true, next.input.GetType().ToString(), next.input.getPosition(), ComponentSize);
                            drawComponentEvent(true, next.output.GetType().ToString(), next.output.getPosition(), ComponentSize);
                        }
                    }
                    if(next.output == null)
                    {
                        next = null;
                    }
                    else
                    {
                        next = next.output.Output;
                    }
                }
               
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
        private Item getItem(Point selectedPosition)
        {
           foreach(Item i in items)
            {
                if(i is Component)
                {
                    if(ComponentInPosition(((Component)(i)), selectedPosition))
                    {
                        return ((Component)i);
                    }
                }
                else
                {
                    if(PipelineInPosition(((Pipeline)i), selectedPosition))
                    {
                        return ((Pipeline)i);
                    }
                }
            }
            return null;
        }
        private bool pathClear(Item i, Item i2)
        {
   
           for(int p = 0; p < items.Count; p++)
            {
                if(items[p] != i || items[p] != i2)
                {
                    if(items[p] is Component)
                    {
                        if (PipelineInPosition((Component)i, (Component)i2, ((Component)items[p]).getPosition()))
                        {
                            if (NetworkErrorEvent != null)
                            {
                                NetworkErrorEvent("Cannot make pipeline connection there is a component in the way");
                            }
                            return false;
                        }
                    }
                    else
                    {
                       if( PipelinesIntersect((Component)i, (Component)i2, ((Pipeline)items[p]).output, ((Pipeline)items[p]).input))
                        {
                            if (NetworkErrorEvent != null)
                            {
                                NetworkErrorEvent("Cannot Make a Pipeline Connection it would interspect with another pipeline");
                            }
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private bool HasLoop(Component c1Output, Component c2Input)
        {
            Component next = null;
            if(c2Input.Output == null)
            {
                return false;
            }
            else
            {
                next = c2Input.Output.output;
            }
            while(next != null)
            {
                if(next == c1Output)
                {
                    if(NetworkErrorEvent != null)
                    {
                        NetworkErrorEvent("Cannot Connect Network would loop with" + next.ToString());
                    }
                    return true;
                }
                if(next.Output == null)
                {
                    break;
                }
                else
                {
                    next = next.Output.output;
                }
                
            }
            return false;
        }
        private bool CanPlaceItem(Point pos)
        {
            foreach(Item i in items)
            {
                if(i is Component)
                {
                    // The item is a component 
                    if (ComponentInPosition((Component)i, pos))
                    {
                        if (NetworkErrorEvent != null)
                        {
                            NetworkErrorEvent("Cannot Place a Component here, Remove " + i.GetType().ToString() + " or choose another place");
                        }
                        return false;
                    }

                }
                else
                {
                    //the item is a pipeline
                    if (PipelineInComponentPosition((Pipeline)i, pos))
                        {
                        if (NetworkErrorEvent != null)
                        {
                            NetworkErrorEvent("Cannot place Component A Pipeline is in the way");
                        }
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
        private bool checkCanConnect(Component c1Output, Component c2Input)
        {
            bool outputok = false;
            if(c1Output.GetType() != typeof(Sink))
            {
                if(c1Output.Output == null)
                {
                    if (c1Output.GetType() != typeof(Splitter))
                    {
                        outputok = true;
                    }
                    else if (c1Output.GetType() == typeof(Splitter))
                    {
                        if (((Splitter)c2Input).Output == null || ((Splitter)c2Input).OutputB == null)
                        {
                            outputok = true;
                        }
                    }
                }
                else
                {
                    if (NetworkErrorEvent != null)
                    {
                        NetworkErrorEvent("Selected Component already has a pipeline");
                    }
                }
               
            }
            else
            {
                if (NetworkErrorEvent != null)
                {
                    NetworkErrorEvent("A Sink has no output to make a connection");
                }
            }
            
            if (outputok)
            {
                if (c2Input.GetType() != typeof(Pump))
                {
                    if (c2Input.GetType() == typeof(Sink))
                    {
                        if (((Sink)c2Input).Output == null)
                        {
                            return true;
                        }
                        else
                        {
                            if (NetworkErrorEvent != null)
                            {
                                NetworkErrorEvent("Cannot Connect the Selected Sink an input connection");
                            }
                            return false;
                        }
                    }
                    else if (c2Input.GetType() == typeof(Merger))
                    {
                        if (((Merger)c2Input).InputA == null || ((Merger)c2Input).InputB == null)
                        {
                            return true;
                        }
                        else
                        {
                            if (NetworkErrorEvent != null)
                            {
                                NetworkErrorEvent("Cannot Connect Both Mergers inputs already have connections");
                            }
                            return false;
                        }
                    }
                    else if (c2Input.GetType() == typeof(Splitter))
                    {
                        if (((Splitter)c2Input).Input == null)
                        {
                            return true;
                        }
                        else
                        {
                            if (NetworkErrorEvent != null)
                            {
                                NetworkErrorEvent("Cannot Connect the second selected Component already has an input connection");
                            }
                            return false;
                        }
                    }

                }
                else
                {
                    if (NetworkErrorEvent != null)
                    {
                        NetworkErrorEvent("Cannot Connect, a Pump has to input");
                    }
                    return false;
                }
            }
            return false;
        }
        private Graphics getGraphic(Item i)
        {
            return null;
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
            slope.Y = p.input.getPosition().Y - p.output.getPosition().Y;
            slope.X = p.input.getPosition().X - p.output.getPosition().X;
            Point temp = slope;
            // Get the perpendicular slope
            slope.Y = -(slope.X);
            slope.X = -(temp.Y);
            Point A = new Point(); //Top Left
            Point B = new Point(); //Top Right
            Point C = new Point(); //Bottom Right
            Point D = new Point(); //Bottom Left
            A.X = p.output.getPosition().X + (slope.X * (pipelineWidth / 2));
            A.Y = p.output.getPosition().Y + (slope.Y * (pipelineWidth / 2));
            B.X = p.output.getPosition().X - (slope.X * (pipelineWidth / 2));
            B.Y = p.output.getPosition().Y - (slope.Y * (pipelineWidth / 2));
            D.X = p.input.getPosition().X + (slope.X * (pipelineWidth / 2));
            D.Y = p.input.getPosition().Y - (slope.Y * (pipelineWidth / 2));
            C.X = p.input.getPosition().X + (slope.X * (pipelineWidth / 2));
            C.Y = p.input.getPosition().Y - (slope.Y * (pipelineWidth / 2));

           
                double PipelineArea = getArea(A, B, C, D);
                double APD = getArea(A, pos, D);
                double DPC = getArea(D, pos, C);
                double CPB = getArea(C, pos, B);
                double PBA = getArea(pos, B, A);

                double PointArea = APD + DPC + CPB + PBA;
                if (PointArea < PipelineArea)
                {
                    return true;
                }

            
                return false;
        }
        private bool PipelineInComponentPosition(Pipeline p, Point pos)
        {
            //First need to get the four points of the pipeline
            Point slope = new Point();
            slope.Y = p.input.getPosition().Y - p.output.getPosition().Y;
            slope.X = p.input.getPosition().X - p.output.getPosition().X;
            Point temp = slope;
            // Get the perpendicular slope
            slope.Y = -(slope.X);
            slope.X = -(temp.Y);
            Point A = new Point(); //Top Left
            Point B = new Point(); //Top Right
            Point C = new Point(); //Bottom Right
            Point D = new Point(); //Bottom Left
            A.X = p.output.getPosition().X + (slope.X * (pipelineWidth / 2));
            A.Y = p.output.getPosition().Y + (slope.Y * (pipelineWidth / 2));
            B.X = p.output.getPosition().X - (slope.X * (pipelineWidth / 2));
            B.Y = p.output.getPosition().Y - (slope.Y * (pipelineWidth / 2));
            D.X = p.input.getPosition().X + (slope.X * (pipelineWidth / 2));
            D.Y = p.input.getPosition().Y - (slope.Y * (pipelineWidth / 2));
            C.X = p.input.getPosition().X + (slope.X * (pipelineWidth / 2));
            C.Y = p.input.getPosition().Y - (slope.Y * (pipelineWidth / 2));

            Point center = pos;
            for (int i = 0; i < 4; i++)
            {
                pos = center;
                switch (i)
                {
                    case 0:
                        //Top Left Point
                        pos.X -= (ComponentSize / 2);
                        pos.Y -= (ComponentSize / 2);
                        break;
                    case 1:
                        //Top Right Point
                        pos.X += (ComponentSize / 2);
                        pos.Y -= (ComponentSize / 2);
                        break;
                    case 2:
                        //Bottom Right
                        pos.X += (ComponentSize / 2);
                        pos.Y += (ComponentSize / 2);
                        break;
                    case 3:
                        //Bottom Left
                        pos.X -= (ComponentSize / 2);
                        pos.Y += (ComponentSize / 2);
                        break;
                }
                double PipelineArea = getArea(A, B, C, D);
                double APD = getArea(A, pos, D);
                double DPC = getArea(D, pos, C);
                double CPB = getArea(C, pos, B);
                double PBA = getArea(pos, B, A);

                double PointArea = APD + DPC + CPB + PBA;
                if (PointArea < PipelineArea)
                {
                    return true;
                }

            }
            return false;
        }

        private bool PipelineInPosition(Component a , Component b, Point pos)
        {
            //First need to get the four points of the pipeline
            Point slope = new Point();
           
            slope.Y = b.getPosition().Y - a.getPosition().Y;
            slope.X = b.getPosition().X - a.getPosition().X;
            Point temp = slope;
            // Get the perpendicular slope
            slope.Y = -(slope.X);
            slope.X = -(temp.Y);
            Point A = new Point(); //Top Left
            Point B = new Point(); //Top Right
            Point C = new Point(); //Bottom Right
            Point D = new Point(); //Bottom Left
            A.X = a.getPosition().X + (slope.X * (pipelineWidth / 2));
            A.Y = a.getPosition().Y + (slope.Y * (pipelineWidth / 2));
            B.X = a.getPosition().X - (slope.X * (pipelineWidth / 2));
            B.Y = a.getPosition().Y - (slope.Y * (pipelineWidth / 2));
            D.X = b.getPosition().X + (slope.X * (pipelineWidth / 2));
            D.Y = b.getPosition().Y - (slope.Y * (pipelineWidth / 2));
            C.X = b.getPosition().X + (slope.X * (pipelineWidth / 2));
            C.Y = b.getPosition().Y - (slope.Y * (pipelineWidth / 2));

            Point center = pos;
            for (int i = 0; i < 4; i++)
            {
                pos = center;
                switch (i)
                {
                    case 0:
                        //Top Left Point
                        pos.X -= (ComponentSize / 2);
                        pos.Y -= (ComponentSize / 2);
                        break;
                    case 1:
                        //Top Right Point
                        pos.X += (ComponentSize / 2);
                        pos.Y -= (ComponentSize / 2);
                        break;
                    case 2:
                        //Bottom Right
                        pos.X += (ComponentSize / 2);
                        pos.Y += (ComponentSize / 2);
                        break;
                    case 3:
                        //Bottom Left
                        pos.X -= (ComponentSize / 2);
                        pos.Y += (ComponentSize / 2);
                        break;
                }
                double PipelineArea = getArea(A, B, C, D);
                double APD = getArea(A, pos, D);
                double DPC = getArea(D, pos, C);
                double CPB = getArea(C, pos, B);
                double PBA = getArea(pos, B, A);

                double PointArea = APD + DPC + CPB + PBA;
                if (PointArea < PipelineArea)
                {
                    return true;
                }

            }
            return false;
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
        private bool PipelinesIntersect(Component ab, Component ba, Component cd , Component dc)
        {
            int m, m2, x, y, b, b2;
            m = (ba.getPosition().Y - ab.getPosition().Y) / (ba.getPosition().X - ab.getPosition().X);
            m2 = (dc.getPosition().Y - cd.getPosition().Y) / (dc.getPosition().X - cd.getPosition().X);
            b = ab.getPosition().Y - (ab.getPosition().X * m);
            b2 = cd.getPosition().Y - (cd.getPosition().X * m);
            // get line
            if(m == m2)
            {
                //lines are parallel
                return false;
            }
            if(m2 > 0)
            {
                x = m - m2;
            }
            else
            {
                x = m + m2;
            }
            if(b > 0)
            {
                y = b2 - b;
            }
            else
            {
                y = b2 + b;
            }
            x = y / x;
            y = m * x + b;
            if(((x >= ab.getPosition().X && x <= ba.getPosition().X) || (x <= ab.getPosition().X && x >= ba.getPosition().X)))
            {
                if((x >= cd.getPosition().X && x <= dc.getPosition().X) || (x <= cd.getPosition().X && x >= dc.getPosition().X))
                {
                    if((y >= ab.getPosition().Y && y <= ba.getPosition().Y) || (y <= ab.getPosition().Y && y >= ba.getPosition().Y))
                    {
                        if(((y >= cd.getPosition().Y && y <= dc.getPosition().Y) || (y <= cd.getPosition().Y && y >= dc.getPosition().Y)))
                        {
                            //the interept point x and y is indeed between the two lines
                          
                            return true;
                        }
                    }
                }
            }
            return false;

        }
    }
}
