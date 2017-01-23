using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OOD2
{
    [Serializable]
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
                    ReplaceComponent(Component, value, flow, ((Component)item),Position);
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
                    ReplaceComponent(Component, value, ((Component)item),Position);
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
            if (Component == "Eraser")
            {
                remove(Position);
            }
            if (item != null)
            {
                if (selectedItem != null)
                {
                    disselect();
                }
                else
                {
                    ReplaceComponent(Component, ((Component)item),Position);
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
                    if( ((Pump)selectedItem).getFlow() != flow && value >= flow)
                    {
                    ((Pump)selectedItem).setFlow(flow);
                    change = true;
                        
                    }
                    if(((Pump)selectedItem).capacity != value && value >= flow)
                    {
                        ((Pump)selectedItem).capacity = value;
                    change = true;
                    }

                }
                else if (selectedItem.GetType() == typeof(Pipeline))
                {
                    if (((Pipeline)selectedItem).safetyLimit != flow)
                    {
                        ((Pipeline)selectedItem).safetyLimit = flow;
                            if(((Pipeline)selectedItem).safetyLimit < ((Pipeline)selectedItem).getFlow())
                            {
                        change = true;
                             }
                    }
                 }
                else if (selectedItem.GetType() == typeof(Splitter))
                {
                    if (((Splitter)selectedItem).adjustmentPercentage != flow)
                    {
                        ((Splitter)selectedItem).adjustmentPercentage = Convert.ToInt32(flow);
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
        private void removeOnlyComponent(Item i)
        {
            if (i.GetType() == typeof(Pipeline))
            {

                items.Remove(i);
              
                i = null;
                if (DrawItemsEvent != null)
                {
                    DrawItemsEvent();
                }
            }
            if (DrawItemsEvent != null)
            {

                items.Remove(i);
                i = null;
                DrawItemsEvent();
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
        private bool pathClear(Component c1, Component c2)
        {
   
           for(int p = 0; p < items.Count; p++)
            {
                if (items[p] is Component)
                {
                    if (items[p] != c1 && items[p] != c2)
                    {
                       
                            if (PipelineIntersectComponent(((Component)items[p]).getPosition(),c1.getPosition(),c2.getPosition()))
                            {
                                if (NetworkErrorEvent != null)
                                {
                                    NetworkErrorEvent("Cannot make pipeline connection there is a component in the way");
                                }
                                return false;
                            }
                        
                    }
                }
                else
                {
                    if ((((Pipeline)items[p]).getNextComponent().getPosition() != c2.getPosition()) && (((Pipeline)items[p]).getInput().getPosition() != c1.getPosition())  && PipelinesIntersect(((Pipeline)items[p]), c1.getPosition(), c2.getPosition()))
                    {
                        if (NetworkErrorEvent != null)
                        {
                            NetworkErrorEvent("Cannot Make a Pipeline Connection it would interspect with another pipeline");
                        }
                        return false;
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
                            NetworkErrorEvent("Cannot Place a Component here, Remove the " + i.GetType().Name + " or choose another place");
                        }
                        return false;
                    }

                }
                else
                {
                    //the item is a pipeline
                    if (PipelineIntersectComponent(pos,((Pipeline)i).getInput().getPosition(), ((Pipeline)i).getNextComponent().getPosition()))
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
        public bool ReplaceComponent(string Component, decimal value, decimal flow, Component replace, Point p)
        {
           
            if (!CanPlaceItem(p))
            {
                removeOnlyComponent(replace);
                if (Component == "Sink")
                {
                    replace = new Sink(p);

                }
                else if (Component == "Merger")
                {
                    replace = new Merger(p);
                }
                else if (Component == "Splitter")
                {
                    replace = new Splitter(p, Convert.ToInt32(value));
                }
                else if (Component == "Pump")
                {
                    if(replace is Pump)
                    { replace = new Pump(p, value, flow); }
                    
                }
                if (DrawItemsEvent != null && replace != null)
                {
                    replace.direction = "East";
                    items.Add(replace);
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
        public bool ReplaceComponent(string Component, decimal value, Component replace, Point p)
        {
           
            if (!CanPlaceItem(p))
            {
                removeOnlyComponent(replace);
                
                if (Component == "Sink")
                {
                    replace = new Sink(p);

                }
                else if (Component == "Merger")
                {
                    replace = new Merger(p);
                }
                else if (Component == "Splitter")
                {
                    replace = new Splitter(p, Convert.ToInt32(value));
                }
                if (DrawItemsEvent != null && replace != null)
                {
                    replace.direction = "East";
                    items.Add(replace);
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
        public bool ReplaceComponent(string Component, Component replace, Point p)
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
            Point A = pos;
            Point B = pos;
            Point C = pos;
            Point D = pos;

            A.X -= (ComponentSize / 2);
            A.Y -= (ComponentSize / 2);

            B.X += (ComponentSize / 2);
            B.Y -= (ComponentSize / 2);

            C.X -= (ComponentSize / 2);
            C.Y += (ComponentSize / 2);

            D.X += (ComponentSize / 2);
            D.Y += (ComponentSize / 2);

            if ((pos.X >= topLeft.X  && pos.Y  >= topLeft.Y && pos.X <= bottomRight.X && pos.Y <= bottomRight.Y ) || (A.X >= topLeft.X && A.Y >= topLeft.Y && A.X <= bottomRight.X && A.Y <= bottomRight.Y))
            {
                return true;
            }
            else if((B.X >= topLeft.X && B.Y >= topLeft.Y && B.X <= bottomRight.X && B.Y <= bottomRight.Y) || (C.X >= topLeft.X && C.Y >= topLeft.Y && C.X <= bottomRight.X && C.Y <= bottomRight.Y))
            {
                return true;
            }
            else if((D.X >= topLeft.X && D.Y >= topLeft.Y && D.X <= bottomRight.X && D.Y <= bottomRight.Y))
            {
                return true;
            }
            return false;
        }

        private bool PipelineInPosition(Pipeline p ,Point pos)
        {
            //First need to get the four points of the pipeline
            Component a = p.getInput();
            Component b = p.getNextComponent();
            Point A = new Point();
            Point B = new Point();
            Point C = new Point();
            Point D = new Point();
            decimal distance, DX, DY;
            double dx, dy;
            dx = a.getPosition().X - b.getPosition().X;
            dy = a.getPosition().Y - b.getPosition().Y;
            distance = Convert.ToDecimal(Math.Sqrt((dx * dx) + (dy * dy)));

            DX = (decimal)dx / distance;
            DY = (decimal)dy / distance;

            A.X = a.getPosition().X + Convert.ToInt32((pipelineWidth / 2) * DY);
            A.Y = a.getPosition().Y - Convert.ToInt32((pipelineWidth / 2) * DX);
            B.X = a.getPosition().X - Convert.ToInt32((pipelineWidth / 2) * DY);
            B.Y = a.getPosition().Y + Convert.ToInt32((pipelineWidth / 2) * DX);

            dx = b.getPosition().X - a.getPosition().X;
            dy = b.getPosition().Y - a.getPosition().Y;
            distance = Convert.ToDecimal(Math.Sqrt((dx * dx) + (dy * dy)));

            DX = (decimal)dx / distance;
            DY = (decimal)dy / distance;

            C.X = b.getPosition().X + Convert.ToInt32((pipelineWidth / 2) * DY);
            C.Y = b.getPosition().Y - Convert.ToInt32((pipelineWidth / 2) * DX);
            D.X = b.getPosition().X - Convert.ToInt32((pipelineWidth / 2) * DY);
            D.Y = b.getPosition().Y + Convert.ToInt32((pipelineWidth / 2) * DX);


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
            else
            {
                return false;
            }
        }
    

        private bool PipelineInPosition(Component a , Component b, Point pos)
        {
            //First need to get the four points of the pipeline

            Point A = new Point();
            Point B = new Point();
            Point C = new Point();
            Point D = new Point();
            decimal distance,DX,DY;
            double dx, dy;
            dx = a.getPosition().X - b.getPosition().X;
            dy = a.getPosition().Y - b.getPosition().Y;
            distance = Convert.ToDecimal(Math.Sqrt((dx * dx) + (dy * dy)));

            DX = (decimal)dx/distance;
            DY = (decimal)dy/distance;

            A.X = a.getPosition().X + Convert.ToInt32((pipelineWidth / 2) * DY);
            A.Y = a.getPosition().Y - Convert.ToInt32((pipelineWidth / 2) * DX);
            B.X = a.getPosition().X - Convert.ToInt32((pipelineWidth / 2) * DY);
            B.Y = a.getPosition().Y + Convert.ToInt32((pipelineWidth / 2) * DX);

            dx = b.getPosition().X - a.getPosition().X;
            dy = b.getPosition().Y - a.getPosition().Y;
            distance = Convert.ToDecimal(Math.Sqrt((dx * dx) + (dy * dy)));

            DX = (decimal)dx/distance;
            DY = (decimal)dy/distance;


            C.X = b.getPosition().X + Convert.ToInt32((pipelineWidth / 2) * DY);
            C.Y = b.getPosition().Y - Convert.ToInt32((pipelineWidth / 2) * DX);
            D.X = b.getPosition().X - Convert.ToInt32((pipelineWidth / 2) * DY);
            D.Y = b.getPosition().Y + Convert.ToInt32((pipelineWidth / 2) * DX);

   

           
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
                else
                {
                return false;
                }

            
           
        }
        private bool PipelineInPosition(Point a, Point b, Point pos)
        {
            //First need to get the four points of the pipeline

            Point A = new Point();
            Point B = new Point();
            Point C = new Point();
            Point D = new Point();
            decimal distance, DX, DY;
            double dx, dy;
            dx = a.X - b.X;
            dy = a.Y - b.Y;
            distance = Convert.ToDecimal(Math.Sqrt((dx * dx) + (dy * dy)));

            DX = (decimal)dx / distance;
            DY = (decimal)dy / distance;

            A.X = a.X + Convert.ToInt32((pipelineWidth / 2) * DY);
            A.Y = a.Y - Convert.ToInt32((pipelineWidth / 2) * DX);
            B.X = a.X - Convert.ToInt32((pipelineWidth / 2) * DY);
            B.Y = a.Y + Convert.ToInt32((pipelineWidth / 2) * DX);

            dx = b.X - a.X;
            dy = b.Y - a.Y;
            distance = Convert.ToDecimal(Math.Sqrt((dx * dx) + (dy * dy)));

            DX = (decimal)dx / distance;
            DY = (decimal)dy / distance;

            C.X = b.X + Convert.ToInt32((pipelineWidth / 2) * DY);
            C.Y = b.Y - Convert.ToInt32((pipelineWidth / 2) * DX);
            D.X = b.X - Convert.ToInt32((pipelineWidth / 2) * DY);
            D.Y = b.Y + Convert.ToInt32((pipelineWidth / 2) * DX);


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
            else
            {
                return false;
            }



        }

        private double getArea(Point A, Point B, Point C)
        {
            double area = ((A.X * (B.Y - C.Y)) + (B.X * (C.Y - A.Y)) + (C.X * (A.Y - B.Y))) / 2;
            area = Math.Abs(area);
            return area;
        }
        private double getArea(Point A,Point B, Point C,Point D)
        {
            double area = (  ((A.X * B.Y) - (A.Y * B.X)) + ((B.X * C.Y) - (B.Y * C.X)) + ((C.X * D.Y) - (C.Y * D.X)) + ((D.X * A.Y) - (D.Y * A.X)))  / 2;
            return area;
        }
        private bool PipelinesIntersect(Pipeline a , Point pipelineBInput, Point pipelineBOutput)
        {
            Point aInput = a.getInput().getPosition();
            Point aOutput = a.getNextComponent().getPosition();


            int o1 = orientation(aInput, aOutput, pipelineBInput);
            int o2 = orientation(aInput, aOutput, pipelineBOutput);
            int o3 = orientation(pipelineBInput, pipelineBOutput, aInput);
            int o4 = orientation(pipelineBInput, pipelineBOutput, aInput);

            if (o1 != o2 && o3 != o4)
            {
                return true;
            }
            else if (o1 == 0 && o2 == 0 && o3 == 0 && o4 == 0)
            {
                
                decimal pipelineALength = Convert.ToDecimal(Math.Sqrt((Math.Pow(aInput.X - aOutput.X, 2)) + (Math.Pow(aInput.Y - aOutput.Y, 2))));
                decimal pipelineBLength = Convert.ToDecimal(Math.Sqrt((Math.Pow(pipelineBInput.X - pipelineBOutput.X, 2)) + (Math.Pow(pipelineBInput.Y - pipelineBOutput.Y, 2))));
                Point AB = new Point();
                Point CD = new Point();
                Point Input = new Point();
                Point Output = new Point();
                
                if(pipelineALength > pipelineBLength)
                {
                    AB = pipelineBInput;
                    CD = pipelineBOutput;
                    Input = aInput;
                    Output = aOutput;
                }
                else if (pipelineALength <= pipelineBLength)
                {
                    AB = aInput;
                    CD = aOutput;
                    Input = pipelineBInput;
                    Output = pipelineBOutput;
                }
                Point A = new Point();
                Point B = new Point();
                Point C = new Point();
                Point D = new Point();
                decimal distance, DX, DY;
                double dx, dy;
                dx = AB.X - CD.X;
                dy = AB.Y - CD.Y;
                distance = Convert.ToDecimal(Math.Sqrt((dx * dx) + (dy * dy)));

                DX = (decimal)dx / distance;
                DY = (decimal)dy / distance;

                A.X = AB.X + Convert.ToInt32((pipelineWidth / 2) * DY);
                A.Y = AB.Y - Convert.ToInt32((pipelineWidth / 2) * DX);
                B.X = AB.X - Convert.ToInt32((pipelineWidth / 2) * DY);
                B.Y = AB.Y + Convert.ToInt32((pipelineWidth / 2) * DX);

                dx = CD.X - AB.X;
                dy = CD.Y - AB.Y;
                distance = Convert.ToDecimal(Math.Sqrt((dx * dx) + (dy * dy)));

                DX = (decimal)dx / distance;
                DY = (decimal)dy / distance;

                C.X = CD.X + Convert.ToInt32((pipelineWidth / 2) * DY);
                C.Y = CD.Y - Convert.ToInt32((pipelineWidth / 2) * DX);
                D.X = CD.X - Convert.ToInt32((pipelineWidth / 2) * DY);
                D.Y = CD.Y + Convert.ToInt32((pipelineWidth / 2) * DX);
                if(PipelineInPosition(Input,Output,A) || PipelineInPosition(Input, Output, B) || PipelineInPosition(Input, Output, C) || PipelineInPosition(Input, Output, D))
                {
                    return true;
                }


            }
            return false;

        }
        private bool PipelineIntersectComponent(Point ComponentPosition, Point InputComponent, Point OutputComponent)
        {
            Point ComponentPointA = ComponentPosition;
            Point ComponentPointB = ComponentPosition;

            for(int i = 0; i < 2; i++)
            {
                if(i == 0)
                {
                    ComponentPointA.X -= (ComponentSize / 2);
                    ComponentPointA.Y -= (ComponentSize / 2);
                    ComponentPointB.X += (ComponentSize / 2);
                    ComponentPointB.Y += (ComponentSize / 2);
                }
                else
                {
                    ComponentPointA.X += (ComponentSize / 2);
                    ComponentPointA.Y -= (ComponentSize / 2);
                    ComponentPointB.X -= (ComponentSize / 2);
                    ComponentPointB.Y += (ComponentSize / 2);
                }
                int o1 = orientation(ComponentPointA, ComponentPointB, InputComponent);
                int o2 = orientation(ComponentPointA, ComponentPointB, OutputComponent);
                int o3 = orientation(InputComponent, OutputComponent, ComponentPointA);
                int o4 = orientation(InputComponent, OutputComponent, ComponentPointA);
                if (o1 != o2 && o3 != o4)
                {
                    return true;
                }
                    
            }
            return false;

        }

        private int orientation(Point p, Point q, Point r)
        {    
            int val = (q.Y - p.Y) * (r.X - q.X) -
                      (q.X - p.X) * (r.Y - q.Y);

            if (val == 0)
            {
                return 0;
            }
            else
            {
                return (val > 0) ? 1 : 2;
            }
            
        }
        private void setDrawDirection(Component component)
        {
            if(component.getNextComponent() != null && component.GetType() != typeof(Pump) && component.GetType() != typeof(Sink))
            {
                Point center = component.getPosition();
                Point end = component.getNextComponent().getPosition();
                decimal eY = end.Y;
                decimal eX = end.X;
                decimal cX = center.X;
                decimal cY = center.Y;
                decimal slope = (eY - cY) / (eX / cX);
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
