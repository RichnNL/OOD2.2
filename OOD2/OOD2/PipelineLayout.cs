using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOD2
{
    public partial class PipelineLayout : Form
    {
        PipelineNetwork network;
        string selectedPanelComponent;
        readonly int componentsize;
        readonly int pipelineSize;
        private Timer doubleClickTimer;
        private int milliseconds;
        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        SaveLoad saveload;

        public PipelineLayout()
        {
            doubleClickTimer = new Timer();
            componentsize = 50;
            pipelineSize = 3;
            InitializeComponent();
            network = new PipelineNetwork(componentsize, 5);
            followBox.Size = new Size(componentsize, componentsize);
            network.DrawItemsEvent += new PipelineNetwork.DrawItemsHandler(DrawLayout);
            network.NetworkErrorEvent += new PipelineNetwork.NetWorkErrorHandler(Error);
            network.SelectedItem1Event += new PipelineNetwork.SelectedItem1ChangedHandler(ComponentSelected);
            layout_panel.Paint += new PaintEventHandler(LayoutPaintEventHandler);
            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);
            saveload = new SaveLoad();
        }
        private void DrawLayout()
        {
            layout_panel.Refresh();
        }

        private void panel_pipeline_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button_Pump_Click(object sender, EventArgs e)
        {
            panelComponetSelect("Pump");
        }
        private void disselectControls()
        {
            nud_capacity.Enabled = false;
            label_set_capacity.Enabled = false;
            nud_splitter_output.Enabled = false;
            label_splitter_output.Enabled = false;
            label_safety_limit.Enabled = false;
            nud_safety_limit.Enabled = false;
            textBox_show_flow.Enabled = false;
            nud_capacity.Enabled = true;
            label_set_capacity.Enabled = true;
            nud_splitter_output.Enabled = true;
            label_splitter_output.Enabled = true;
            label_safety_limit.Enabled = true;
            nud_safety_limit.Enabled = true;
            textBox_show_flow.Enabled = true;
        }
        private void panelComponetSelect(string component)
        {
            disselectControls();
            nud_capacity.Visible = false;
            label_set_capacity.Visible = false;
            nud_splitter_output.Visible = false;
            label_splitter_output.Visible = false;
            label_safety_limit.Visible = false;
            nud_safety_limit.Visible = false;
            textBox_show_flow.Visible = false;
            switch (component)
            {
                case "Pump":
                    selected_component_picture.BackgroundImage = Properties.Resources.pump;
                    followBox.BackgroundImage = Properties.Resources.pump;
                    nud_capacity.Visible = true;
                    label_set_capacity.Visible = true;
                    nud_splitter_output.Visible = true;
                    label_splitter_output.Visible = true;
                    label_splitter_output.Text = "Set Flow";
                    selectedPanelComponent = "Pump";
                    nud_splitter_output.Maximum = 10000;



                    break;
                case "Sink":
                    selected_component_picture.BackgroundImage = Properties.Resources.sink;
                    followBox.BackgroundImage = Properties.Resources.sink;
                    selectedPanelComponent = "Sink";

                    break;
                case "Merger":
                    selected_component_picture.BackgroundImage = Properties.Resources.MergerEast;
                    followBox.BackgroundImage = Properties.Resources.MergerEast;
                    selectedPanelComponent = "Merger";

                    break;
                case "Splitter":
                    selected_component_picture.BackgroundImage = Properties.Resources.WestSpiltter;
                    followBox.BackgroundImage = Properties.Resources.EastSpiltter;
                    nud_splitter_output.Visible = true;
                    nud_splitter_output.Value = 50;
                    nud_splitter_output.Maximum = 100;
                    
                    label_splitter_output.Visible = true;
                    label_splitter_output.Text = "Splitter Output";
                    selectedPanelComponent = "Splitter";


                    break;
                case "Eraser":
                    selected_component_picture.BackgroundImage = Properties.Resources.eraser;
                    followBox.BackgroundImage = Properties.Resources.eraser;
                    selectedPanelComponent = "Eraser";
                    break;
                case "Mouse":
                    selected_component_picture.BackgroundImage = null;
                    followBox.BackgroundImage = null;
                    selectedPanelComponent = "";
                    label_safety_limit.Visible = true;
                    nud_safety_limit.Visible = true;
                    label_safety_limit.Text = " Safety Limit";
                    selectedPanelComponent = "Pipeline";
                    break;

            }
            network.disselect();
        }
        private void panelComponetSelect(Item item)
        {
            if (item != null)
            {
                followBox.BackgroundImage = null;

                nud_capacity.Visible = false;
                label_set_capacity.Visible = false;
                nud_splitter_output.Visible = false;
                label_splitter_output.Visible = true;
                label_safety_limit.Visible = false;
                nud_safety_limit.Visible = true;
                label_safety_limit.Text = "Flow";
                textBox_show_flow.Visible = false;
                textBox_show_flow.Text = item.getFlow().ToString();
                selectedPanelComponent = "Mouse";
                nud_safety_limit.Visible = false;
                label_splitter_output.Visible = false;
                switch (item.GetType().Name)
                {
                    case "Pump":
                        selected_component_picture.BackgroundImage = Properties.Resources.pump;
                       
                        label_set_capacity.Visible = true;
                        textBox_show_flow.Visible = false;
                        nud_splitter_output.Visible = true;
                        label_splitter_output.Visible = true;
                        nud_safety_limit.Visible = false;
                        label_safety_limit.Text = "Safety Limit";
                        nud_splitter_output.Value = item.getFlow();
                        nud_capacity.Value = ((Pump)item).capacity;
                        nud_capacity.Visible = true;
                        nud_splitter_output.Maximum = 10000;


                        break;
                    case "Sink":
                        selected_component_picture.BackgroundImage = Properties.Resources.sink;
                     
                      
                        break;
                    case "Merger":
                        selected_component_picture.BackgroundImage = Properties.Resources.MergerEast;
                       
                      
                        break;

                    case "Splitter":
                        selected_component_picture.BackgroundImage = Properties.Resources.WestSpiltter;
                        
                        label_set_capacity.Visible = false;
                        nud_splitter_output.Visible = true;
                        label_splitter_output.Visible = true;
                        label_splitter_output.Text = "Splitter Output";
                        nud_splitter_output.Value = ((Splitter)item).adjustmentPercentage;
                        nud_splitter_output.Maximum = 100;
                        nud_safety_limit.Visible = false;
                        break;

                    case "Pipeline":
                        selected_component_picture.BackgroundImage = Properties.Resources.greenPipe;
                        
                        nud_capacity.Visible = false;
                        label_set_capacity.Visible = false;
                        nud_splitter_output.Visible = false;
                        label_splitter_output.Visible = false;
                        label_safety_limit.Visible = true;
                        nud_safety_limit.Visible = true;
                        nud_safety_limit.Value = ((Pipeline)item).safetyLimit;
                        label_safety_limit.Text = "Safety Limit";
                        selectedPanelComponent = "Mouse";
                        break;
                }
            }
        }

        private void button_sink_Click(object sender, EventArgs e)
        {
            panelComponetSelect("Sink");
        }

        private void button_splitter_Click(object sender, EventArgs e)
        {
            panelComponetSelect("Splitter");
        }

        private void button_merger_Click(object sender, EventArgs e)
        {
            panelComponetSelect("Merger");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelComponetSelect("Eraser");
        }

        private void button_mouse_Click(object sender, EventArgs e)
        {
            panelComponetSelect("Mouse");
        }

        private void layout_MouseEnter(object sender, EventArgs e)
        {



            if (followBox.BackgroundImage != null)
            {
                if (!followBox.Visible)
                {
                    followBox.Visible = true;
                }

            }
        }

        private void layout_MouseLeave(object sender, EventArgs e)
        {
            if (followBox.Visible)
            {

                followBox.Visible = false;
            }

        }

        private void layout_MouseMove(object sender, MouseEventArgs e)
        {
            if (followBox.Visible)
            {
                followBox.Location = new Point(e.X - (componentsize / 2), e.Y - (componentsize / 2));


            }
        }
     
        private void drawComponentFlowNumebr(decimal flow, Point ComponentPosition, object sender,PaintEventArgs args)
        {
            if (flow != -1)
            {
               
                int size = componentsize / 4;
                Font font = new Font("Arial", size/2, FontStyle.Bold, GraphicsUnit.Point);

                ComponentPosition.X = ComponentPosition.X + componentsize;
                ComponentPosition.X = ComponentPosition.X - size;

                Rectangle rectangle = new Rectangle(ComponentPosition.X, ComponentPosition.Y, size, size);
                args.Graphics.DrawString(flow.ToString(), font, Brushes.Green, rectangle);
                args.Graphics.DrawRectangle(new Pen(Color.Black, 1), Rectangle.Round(rectangle));


            }


        }
     
        private void Error(string error)
        {
            Notification_Bar.Items.Add("error");
        }
        private void ComponentSelected(Item item)
        {
            panelComponetSelect(item);
            layout_panel.Refresh();
        }

        private void layout_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            Point position = new Point();
            position = e.Location;
            if (NotOutSizeLayout(position) && selectedPanelComponent != "Mouse")
            {
                if (selectedPanelComponent == "Pump")
                {
                    network.NetworkDoubleClicked("Pump", position, nud_capacity.Value, nud_splitter_output.Value);
                }
                else if (selectedPanelComponent == "Splitter")
                {
                    network.NetworkDoubleClicked("Splitter", position, nud_splitter_output.Value);
                }
                else if (selectedPanelComponent == "Sink")
                {
                    network.NetworkDoubleClicked("Sink", position, nud_splitter_output.Value);
                }
                else if(selectedPanelComponent=="Merger")
                {
                    network.NetworkDoubleClicked("Merger", position, nud_splitter_output.Value);
                }
                else if (selectedPanelComponent == "Eraser")
                {
                    network.NetworkDoubleClicked("Eraser", position);
                }

            }
            else if(selectedPanelComponent == "Mouse")
            {
                network.NetworkDoubleClicked("Pipeline", position, nud_safety_limit.Value);
            }
            


        }

        private void layout_MouseClick(object sender, MouseEventArgs e)
        {
           
                doubleClickTimer.Start();
             
            

           

        }

        private void nud_capacity_ValueChanged(object sender, EventArgs e)
        {

            if (network.ComponentIsSelected())
            {
                network.ChangeSelectedItemValues(nud_capacity.Value, nud_splitter_output.Value);
                disselectControls();
            }

        }
        private bool NotOutSizeLayout(Point position)
        {

            int width = layout_panel.Size.Width;
            int height = layout_panel.Size.Height;
            position.X += componentsize / 2;
            position.Y += componentsize / 2;
            if (position.X > width || position.Y > height)
            {
                return false;
            }
            position.X -= componentsize;
            position.Y -= componentsize;
            if (position.X < 0 || position.Y < 0)
            {
                return false;
            }
            return true;
        }
        private void LayoutPaintEventHandler(object sender, PaintEventArgs args)
        {
            List<Item> item = network.getitems();
            // Draw all Pipelines
            foreach(Item pipe in item)
            {
                if(pipe.GetType() == typeof(Pipeline))
                {
                    drawPipelines((Pipeline)pipe, sender, args);
                }
                
            }
            foreach(Item component in item)
            {
                if(component is Component)
                {
                    drawComponent((Component)component, sender, args);
                }
                
            }
        }
        private void drawPipelines(Pipeline pipe, object sender, PaintEventArgs args)
        {
            Pen pen = new Pen(Color.Green);
            pen.Width = pipelineSize;
            if(pipe.safetyLimit < pipe.getFlow())
            {
                pen.Color = Color.Red;
            }
            args.Graphics.DrawLine(pen, pipe.getInput().getPosition(), pipe.getNextComponent().getPosition());      
        }
        private void drawComponent(Component component, object sender, PaintEventArgs args)
        {
            Point Position = component.getPosition();
            Bitmap image = null;
            Position.X = Position.X - (componentsize / 2);
            Position.Y = Position.Y - (componentsize / 2);
            if (component.GetType() == typeof(Pump))
            {

                if (component.selected == true)
                {
                    image = Properties.Resources.pump_selected;
                }
                else
                {
                    image = Properties.Resources.pump;
                }

            }
          else  if (component.GetType() == typeof(Sink))
            {

                if (component.selected == true)
                {
                    image = Properties.Resources.sink_selected;
                }
                else
                {
                    image = Properties.Resources.sink;
                }

            }

            else if (component.GetType()==typeof(Merger))
            {
                if (component.direction == "East")
                {
                    if (component.selected == true)
                    {
                        image = Properties.Resources.MergerEast_selected;
                    }
                    else
                    {
                        image = Properties.Resources.MergerEast;
                    }
                }
                else if  (component.direction == "West")
                {
                    if (component.selected == true)
                    {
                        image = Properties.Resources.MergerWest_selected;
                    }
                    else
                    {
                        image = Properties.Resources.MergerWest;
                    }
                }
                else if (component.direction=="South")
                {
                    if(component.selected==true)
                    {
                        image = Properties.Resources.MergerSouth_selected;
                    }
                    else
                    {
                        image = Properties.Resources.MergerSouth;
                    }
                }
            }

            else if (component.GetType() == typeof(Splitter))
            {
                if (component.direction == "North")
                {
                    if (component.selected == true)
                    {
                        image = Properties.Resources.NorthSpiltter_selected;
                    }
                    else
                    {
                        image = Properties.Resources.NorthSpiltter;
                    }
                }
                else if (component.direction == "East")
                {
                    if (component.selected == true)
                    {
                        image = Properties.Resources.EastSpiltter_selected;
                    }
                    else
                    {
                        image = Properties.Resources.EastSpiltter;
                    }

                }
                else if (component.direction == "South")
                {
                    if (component.selected == true)
                    {
                        image = Properties.Resources.SouthSpiltter_selected;
                    }
                    else
                    {
                        image = Properties.Resources.SouthSpiltter;
                    }
                }
                else if (component.direction == "West")
                {
                    if (component.selected == true)
                    {
                        image = Properties.Resources.WestSpiltter_selected;
                    }
                    else
                    {
                        image = Properties.Resources.WestSpiltter;
                    }
                }
              

            }
            args.Graphics.DrawImage(image, Position.X, Position.Y, componentsize, componentsize);
            drawComponentFlowNumebr(component.getFlow(), Position, sender, args);
        }
        void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            milliseconds += 100;

            if (milliseconds >= SystemInformation.DoubleClickTime)
            {
                doubleClickTimer.Stop();
                if (followBox.BackgroundImage == null)
                {
                    network.NetworkClicked(layout_panel.PointToClient(MousePosition));
                }
              
                milliseconds = 0;
            }

               
              
               
            }


        private void nud_safety_limit_ValueChanged(object sender, EventArgs e)
        {
            if (network.ComponentIsSelected())
            {
                network.ChangeSelectedItemValues(nud_capacity.Value, nud_safety_limit.Value);
                disselectControls();
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Pipeline Network save file|*.pnt";
            savefile.Title = "Save a Pipeline Network file";
            savefile.ShowDialog();

            if (savefile.FileName != "")
            {
                saveload.save(savefile.FileName, network.getitems());
            }
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pipeline Network save file|*.pnt";
            ofd.Title = "Load a Pipeline Network file";
            ofd.ShowDialog();

            network.setitems(saveload.load(ofd.FileName));

        }

        private void nud_splitter_output_ValueChanged(object sender, EventArgs e)
        {
            if (network.ComponentIsSelected())
            {
                network.ChangeSelectedItemValues(nud_capacity.Value, nud_splitter_output.Value);
                disselectControls();
            }
        }

        private void btnmouse_Click(object sender, EventArgs e)
        {
            panelComponetSelect("Mouse");
        }
    }
    }
    

