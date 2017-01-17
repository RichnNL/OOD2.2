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
        public delegate void DrawItemsHandler();
        public event DrawItemsHandler DrawItemsEvent;
        
        public event NetWorkErrorHandler NetworkErrorEvent;
        public event SelectedItem1ChangedHandler SelectedItem1Event;
        public PipelineNetwork(int ComponentSize, int PipelineWidth)
        {
            items = new List<Item>();
            this.ComponentSize = ComponentSize;
            this.pipelineWidth = PipelineWidth;
        }

        public void disselect()
        {
            // When the pointer component is clicked
            if(selectedItem != null)
            {
                selectedItem.selected = false;
                selectedItem = null;
                if(DrawItemsEvent != null)
                {
                    DrawItemsEvent();
                }
            }
            
            selectedItem2 = null;
        }
        public void NetworkClicked(Point position)
        {
            Item item = getItem(position);
            if(item != null)
            {
                    if(selectedItem != null)
                    {
                    selectedItem.selected = false;
                     }   
                    else if(selectedItem == item)
                    {
                    selectedItem.selected = false;
                    disselect();
                    return;
                    }

                    selectedItem = item;
                    selectedItem.selected = true;

                    if (SelectedItem1Event != null)
                    {
                        SelectedItem1Event(selectedItem);
                    }
                
                
            }
            else
            {
                disselect();
              
            }
        }
        public void NetworkDoubleClicked(string Component, Point Position,decimal value, decimal flow)
        {
            Item item = getItem(Position);
            if (item != null)
            {
                if(selectedItem != null && Component == "Pipeline")
                {
                    selectedItem2 = item;
                    if (selectedItem.GetType() != typeof(Pipeline) && selectedItem2.GetType() != typeof(Pipeline))
                    {
                        if (addPipeline(((Component)selectedItem), ((Component)selectedItem2), value))
                        {
                            // disselect components? message box after pipeline success??
                        }
                    }
                }
                else
                {
                    ReplaceComponent(Component, value, flow, ((Component)item));
                }
                


            }
            else
            {
                addComponent(Component, Position, value, flow);
            }
          
        }
        public void NetworkDoubleClicked(string Component, Point Position, decimal value)
        {
            Item item = getItem(Position);
            if (item != null)
            {
                if (selectedItem != null && Component == "Pipeline")
                {
                    selectedItem2 = item;
                    if (selectedItem.GetType() != typeof(Pipeline) && selectedItem2.GetType() != typeof(Pipeline))
                    {
                        if (addPipeline(((Component)selectedItem), ((Component)selectedItem2), value))
                        {
                            // disselect components? message box after pipeline success??
                        }
                    }
                }
                else
                {
                    ReplaceComponent(Component, value, ((Component)item));
                }



            }
            else
            {
                addComponent(Component, Position, value);
            }

        }
        public void NetworkDoubleClicked(string Component, Point Position)
        {
            Item item = getItem(Position);
            if (item != null)
            {
                if (selectedItem != null)
                {
                    disselect();
                }
                else
                {
                    ReplaceComponent(Component, ((Component)item));
                }



            }
            else
            {
                addComponent(Component, Position);
            }

        }
        private bool addComponent(string Component, Point pos, decimal value, decimal flow )
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
                if(DrawItemsEvent != null && c != null)
                {
                    c.direction = "East";
                    items.Add(c);
                    DrawItemsEvent();
                    return true;
                }
                return false;
            }
            else
            {
              
                return false;
            }
        }
        private bool addComponent(string Component, Point pos, decimal value)
        {
            Component c = null;
            if (CanPlaceItem(pos))
            {
                if (Component == "Sink")
                {
                    c = new Sink(pos);

                }
                else if (Component == "Merger")
                {
                    c = new Merger(pos);
                }
                else if (Component == "Splitter")
                {
                    c = new Splitter(pos, Convert.ToInt32(value));
                }
                if (DrawItemsEvent != null && c != null)
                {
                    c.direction = "East";
                    items.Add(c);
                    DrawItemsEvent();
                    return true;
                }
                return false;
            }
            else
            {

                return false;
            }
        }
        private bool addComponent(string Component, Point pos)
        {
            Component c = null;
            if (CanPlaceItem(pos))
            {
                if (Component == "Sink")
                {
                    c = new Sink(pos);

                }
                else if (Component == "Merger")
                {
                    c = new Merger(pos);
                }
                if (DrawItemsEvent != null && c != null)
                {
                    c.direction = "East";
                    items.Add(c);
                    DrawItemsEvent();
                    return true;
                }
                return false;
            }
            else
            {

                return false;
            }
        }

        public void ChangeSelectedItemValues(decimal value, decimal flow)
        {
            bool change = false;
                if (selectedItem.GetType() == typeof(Pump))
                {
                    if( ((Pump)selectedItem).getFlow() != flow && value <= flow)
                    {
                    ((Pump)selectedItem).setFlow(flow);
                    change = true;
                        
                    }
                    if(((Pump)selectedItem).capacity != value && value <= flow)
                    {
                        ((Pump)selectedItem).capacity = value;
                    change = true;
                    }

                }
                else if (selectedItem.GetType() == typeof(Pipeline))
                {
                    if (((Pipeline)selectedItem).safetyLimit != value)
                    {
                        ((Pipeline)selectedItem).safetyLimit = value;
                            if(((Pipeline)selectedItem).safetyLimit < ((Pipeline)selectedItem).getFlow())
                            {
                        change = true;
                             }
                    }
                 }
                else if (selectedItem.GetType() == typeof(Splitter))
                {
                    if (((Splitter)selectedItem).adjustmentPercentage != value)
                    {
                        ((Splitter)selectedItem).adjustmentPercentage = Convert.ToInt32(value);
                    change = true;

                    }

                }
            if (change)
            {
                if (DrawItemsEvent != null)
                {
                    DrawItemsEvent();
                }
            }
                
           
          
        }

        private void remove(Item i)
        {
            if (i.GetType() == typeof(Pipeline))
            {
             
                items.Remove(i);
                i.getNextComponent().removeInput();
                i = null;
                if(DrawItemsEvent != null)
                {
                    DrawItemsEvent();
                }
            }
            else
            {
                //Item is a Component
                //First must delete Pipelines Connected to Components
                if (((Component)i).getNextPipeline() != null)
                {
                    remove(((Component)i).getNextPipeline());
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
                if (DrawItemsEvent != null)
                {

                    items.Remove(i);
                    i = null;
                    DrawItemsEvent();
                }
            }
        }
        public void remove(Point position)
        {
            Item i = getItem(position);
            if (i.GetType() == typeof(Pipeline))
            {

                items.Remove(i);
                i.getNextComponent().removeInput();
                i = null;
                if (DrawItemsEvent != null)
                {
                    DrawItemsEvent();
                }

            }
            else
            {
                //Item is a Component
                //First must delete Pipelines Connected to Components
                if (((Component)i).getNextPipeline() != null)
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
                if (DrawItemsEvent != null)
                {

                    items.Remove(i);
                    i = null;
                    DrawItemsEvent();
                }
            }
        }
        private bool addPipeline(Component c1Output, Component c2Input, decimal safetyLimit)
        {
            if(!HasLoop(c1Output,c2Input) && pathClear(c1Output,c2Input) && checkCanConnect(c1Output,c2Input))
            {
                Pipeline pipe = new Pipeline(c1Output, c2Input, safetyLimit);
                items.Add(pipe);
                setDrawDirection(c1Output);
                if(c2Input.GetType() == typeof(Sink))
                {
                    setDrawDirection(c2Input);
                }
                if(DrawItemsEvent != null)
                {
                    DrawItemsEvent();
                }
               
                return true;
            }
            else
            {
                return false;
            }
            
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
                if(items[p] != i && items[p] != i2)
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
                       if( PipelinesIntersect((Component)i, (Component)i2, ((Pipeline)items[p]).getNextComponent(), ((Pipeline)items[p]).getInput()))
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
                next = c2Input.getNextComponent();
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
                    next = next.getNextComponent();
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
                            NetworkErrorEvent("Cannot Place a Component here, Remove " + i.GetType().Name + " or choose another place");
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
        public bool ReplaceComponent(string Component, decimal value, decimal flow, Component replace)
        {
            return true;
        }
        public bool ReplaceComponent(string Component, decimal value, Component replace)
        {
            return true;
        }
        public bool ReplaceComponent(string Component, Component replace)
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
            if (topLeft.X <= pos.X && bottomRight.X >= pos.X)
            {
                if (topLeft.Y <= pos.Y && bottomRight.Y >= pos.Y)
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
            slope.Y = p.getInput().getPosition().Y - p.getNextComponent().getPosition().Y;
            slope.X = p.getInput().getPosition().X - p.getNextComponent().getPosition().X;
            Point temp = slope;
            // Get the perpendicular slope
            slope.Y = -(slope.X);
            slope.X = -(temp.Y);
            Point A = new Point(); //Top Left
            Point B = new Point(); //Top Right
            Point C = new Point(); //Bottom Right
            Point D = new Point(); //Bottom Left
            A.X = p.getNextComponent().getPosition().X + (slope.X * (pipelineWidth / 2));
            A.Y = p.getNextComponent().getPosition().Y + (slope.Y * (pipelineWidth / 2));
            B.X = p.getNextComponent().getPosition().X - (slope.X * (pipelineWidth / 2));
            B.Y = p.getNextComponent().getPosition().Y - (slope.Y * (pipelineWidth / 2));
            D.X = p.getInput().getPosition().X + (slope.X * (pipelineWidth / 2));
            D.Y = p.getInput().getPosition().Y - (slope.Y * (pipelineWidth / 2));
            C.X = p.getInput().getPosition().X + (slope.X * (pipelineWidth / 2));
            C.Y = p.getInput().getPosition().Y - (slope.Y * (pipelineWidth / 2));

           
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
            slope.Y = p.getInput().getPosition().Y - p.getNextComponent().getPosition().Y;
            slope.X = p.getInput().getPosition().X - p.getNextComponent().getPosition().X;
            Point temp = slope;
            // Get the perpendicular slope
            slope.Y = -(slope.X);
            slope.X = -(temp.Y);
            Point A = new Point(); //Top Left
            Point B = new Point(); //Top Right
            Point C = new Point(); //Bottom Right
            Point D = new Point(); //Bottom Left
            A.X = p.getNextComponent().getPosition().X + (slope.X * (pipelineWidth / 2));
            A.Y = p.getNextComponent().getPosition().Y + (slope.Y * (pipelineWidth / 2));
            B.X = p.getNextComponent().getPosition().X - (slope.X * (pipelineWidth / 2));
            B.Y = p.getNextComponent().getPosition().Y - (slope.Y * (pipelineWidth / 2));
            D.X = p.getInput().getPosition().X + (slope.X * (pipelineWidth / 2));
            D.Y = p.getInput().getPosition().Y - (slope.Y * (pipelineWidth / 2));
            C.X = p.getInput().getPosition().X + (slope.X * (pipelineWidth / 2));
            C.Y = p.getInput().getPosition().Y - (slope.Y * (pipelineWidth / 2));

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
            if((ab.GetType() == typeof(Splitter) && cd.GetType() == typeof(Splitter) && ab.getPosition() == cd.getPosition()) || (ab.GetType() == typeof(Merger) && cd.GetType() == typeof(Merger) && ba.getPosition() == dc.getPosition()))
            {
                return false;
            }
            else
            {
                int m, m2, x, y, b, b2;
                m = (ba.getPosition().Y - ab.getPosition().Y) / (ba.getPosition().X - ab.getPosition().X);
                m2 = (dc.getPosition().Y - cd.getPosition().Y) / (dc.getPosition().X - cd.getPosition().X);
                b = ab.getPosition().Y - (ab.getPosition().X * m);
                b2 = cd.getPosition().Y - (cd.getPosition().X * m);
                // get line
                if (m == m2)
                {
                    //lines are parallel
                    return false;
                }
                if (m2 > 0)
                {
                    x = m - m2;
                }
                else
                {
                    x = m + m2;
                }
                if (b > 0)
                {
                    y = b2 - b;
                }
                else
                {
                    y = b2 + b;
                }
                x = y / x;
                y = m * x + b;
                if (((x >= ab.getPosition().X && x <= ba.getPosition().X) || (x <= ab.getPosition().X && x >= ba.getPosition().X)))
                {
                    if ((x >= cd.getPosition().X && x <= dc.getPosition().X) || (x <= cd.getPosition().X && x >= dc.getPosition().X))
                    {
                        if ((y >= ab.getPosition().Y && y <= ba.getPosition().Y) || (y <= ab.getPosition().Y && y >= ba.getPosition().Y))
                        {
                            if (((y >= cd.getPosition().Y && y <= dc.getPosition().Y) || (y <= cd.getPosition().Y && y >= dc.getPosition().Y)))
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
        private void setDrawDirection(Component component)
        {
            if(component.getNextComponent() != null && component.GetType() != typeof(Pump) && component.GetType() != typeof(Sink))
            {
                Point center = component.getPosition();
                Point end = component.getNextComponent().getPosition();
                decimal slope = (end.Y - center.Y) / (end.X / center.X);
                if ((end.Y < center.Y && (slope >= 1 || slope <= -1)) || (center.X == end.X && end.Y < center.Y))
                {
                    // north
                    component.direction = "North";

                }
                else if (((end.Y < center.Y) && (slope < 1 && slope >= 0)) || (center.Y == end.Y && end.X < center.X))
                {

                    component.direction = "West";
                }
                else if (((end.Y < center.Y) && (slope > -1 && slope < 0)) || (center.Y == end.Y && end.X > center.X))
                {
                    component.direction = "East";
                }
                else if (end.Y > center.Y && slope > -1 && slope < 0)
                {

                    component.direction = "West";
                }
                else if (end.Y > center.Y && slope > 1 && slope > 0)
                {

                    component.direction = "East";
                }
                else
                {
                    component.direction = "South";
                }

            }
            else
            {
                component.direction = "East";
            }

        }
        public bool ComponentIsSelected()
        {
            if(selectedItem != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Item> getitems()
        {
            return items.ToList();
        }
    }
}
