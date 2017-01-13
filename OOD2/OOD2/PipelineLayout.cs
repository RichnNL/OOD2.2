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
        public PipelineLayout()
        {
            componentsize = 50;
            InitializeComponent();
            network = new PipelineNetwork(componentsize, 5);
            followBox.Size = new Size(50, 50);
            network.drawComponentEvent += new PipelineNetwork.DrawComponentHandler(DrawComponent);
            network.drawPipelineEvent += new PipelineNetwork.DrawPipelinehandler(DrawPipeline);
            network.NetworkErrorEvent += new PipelineNetwork.NetWorkErrorHandler(Error);
            network.SelectedItem1Event += new PipelineNetwork.SelectedItem1ChangedHandler(ComponentSelected);
        }

        private void panel_pipeline_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button_Pump_Click(object sender, EventArgs e)
        {
            panelComponetSelect("Pump");
        }
        private void panelComponetSelect(string component)
        {
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
                    
                  

                    break;
                case "Sink":
                    selected_component_picture.BackgroundImage = Properties.Resources.sink;
                    followBox.BackgroundImage = Properties.Resources.sink;
                   
                    break;
                case "Merger":
                    selected_component_picture.BackgroundImage = Properties.Resources.MergerEast;
                    followBox.BackgroundImage = Properties.Resources.MergerEast;
                    
                    break;
                case "Splitter":
                    selected_component_picture.BackgroundImage  = Properties.Resources.WestSpiltter;
                    followBox.BackgroundImage = Properties.Resources.WestSpiltter;
                    nud_splitter_output.Visible = true;
                    label_splitter_output.Visible = true;
                    label_splitter_output.Text = "Splitter Output";
                    

                    break;
                case "Eraser":
                    selected_component_picture.BackgroundImage = Properties.Resources.eraser;
                    followBox.BackgroundImage = Properties.Resources.eraser;
                    break;
                case "Mouse":
                    selected_component_picture.BackgroundImage  = null;
                    followBox.BackgroundImage = null;
                   

                    break;
                case "Pipeline":
                    selected_component_picture.BackgroundImage = Properties.Resources.greenPipe;
                    followBox.BackgroundImage = null;
                   label_safety_limit.Visible = true;
                    nud_safety_limit.Visible = true;
                    label_safety_limit.Text = " Safety Limit";

                    break;
            }
            network.disselect();
        }
        private void panelComponetSelect(Item item)
        {
            if (item != null)
            {


                nud_capacity.Visible = false;
                label_set_capacity.Visible = false;
                nud_splitter_output.Visible = false;
                label_splitter_output.Visible = true;
                label_safety_limit.Visible = false;
                nud_safety_limit.Visible = true;
                label_safety_limit.Text = "Flow";
                textBox_show_flow.Visible = false;
                textBox_show_flow.Text = item.getFlow().ToString();

                switch (item.GetType().Name)
                {
                    case "Pump":
                        selected_component_picture.BackgroundImage = Properties.Resources.pump;
                        followBox.BackgroundImage = Properties.Resources.pump;
                        label_set_capacity.Visible = true;
                        textBox_show_flow.Visible = false;
                        nud_splitter_output.Visible = true;
                        label_splitter_output.Visible = true;
                        nud_safety_limit.Visible = false;
                        label_safety_limit.Text = "Safety Limit";
                        nud_splitter_output.Value = item.getFlow();
                        nud_capacity.Value = ((Pump)item).capacity;


                        break;
                    case "Sink":
                        selected_component_picture.BackgroundImage = Properties.Resources.sink;
                        followBox.BackgroundImage = Properties.Resources.sink;
                        break;
                    case "Merger":
                        selected_component_picture.BackgroundImage = Properties.Resources.MergerEast;
                        followBox.BackgroundImage = Properties.Resources.MergerEast;

                        break;
                    case "Splitter":
                        selected_component_picture.BackgroundImage = Properties.Resources.WestSpiltter;
                        followBox.BackgroundImage = Properties.Resources.WestSpiltter;
                        label_set_capacity.Visible = false;
                        nud_splitter_output.Visible = true;
                        label_splitter_output.Visible = true;
                        label_splitter_output.Text = "Splitter Output";
                        nud_splitter_output.Value = ((Splitter)item).adjustmentPercentage;
                        break;

                    case "Pipeline":
                        selected_component_picture.BackgroundImage = Properties.Resources.greenPipe;
                        followBox.BackgroundImage = null;
                        nud_capacity.Visible = false;
                        label_set_capacity.Visible = false;
                        nud_splitter_output.Visible = false;
                        label_splitter_output.Visible = false;
                        label_safety_limit.Visible = true;
                        nud_safety_limit.Visible = true;
                        nud_safety_limit.Visible = false;
                        label_safety_limit.Text = "Safety Limit";

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
            
             
            
            if(followBox.BackgroundImage != null)
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
                followBox.Location = new Point(e.X - (componentsize/2),  e.Y - (componentsize / 2));
              
                
            }
        }
        private void DrawComponent(bool Draw, string drawDirection, string component, Point Position, decimal flow, int HeightWidth)
        {
            Graphics g = layout.CreateGraphics();
            Image image = null;
            Position.X = Position.X - (componentsize / 2);
            Position.Y = Position.Y - (componentsize / 2);
            if (Draw)
            {
                if(component == "Pump")
                {
                    if(drawDirection == "North")
                    {
                        
                    }
                    else if(drawDirection == "East")
                    {
                        image = Properties.Resources.pump;
                    }
                    else if(drawDirection == "South")
                    {

                    }
                    else if(drawDirection == "West")
                    {

                    }
                }
                g.DrawImage(image, Position.X,Position.Y, HeightWidth, HeightWidth);
                drawComponentFlowNumebr(flow, Position);
            }
        }
        private void drawComponentFlowNumebr(decimal flow, Point ComponentPosition)
        {
            if(flow != -1)
            {
                Graphics g = layout.CreateGraphics();
                int size = componentsize / 4;
                ComponentPosition.X = ComponentPosition.X + (componentsize / 2);
                ComponentPosition.X = ComponentPosition.X - size;
                
                //g.DrawString(flow.ToString(), Font.Bold,Brushes.Green,ComponentPosition,)
            }
          

        }
        private void DrawPipeline(bool Draw, bool Green, Point Component1Position, Point Component2Position, int height)
        {

        }
        private void Error(string error)
        {
            MessageBox.Show(error, "Warning", MessageBoxButtons.OK);
        }
        private void ComponentSelected(Item item)
        {
            panelComponetSelect(item);
        }

        private void layout_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(selected_component_picture.BackgroundImage == Properties.Resources.pump)
            {
                network.NetworkDoubleClicked("Pump", e.Location, nud_capacity.Value, nud_safety_limit.Value);
            }
           
        }

        private void layout_MouseClick(object sender, MouseEventArgs e)
        {
            network.NetworkClicked(e.Location);
        }

        private void nud_capacity_ValueChanged(object sender, EventArgs e)
        {
            if (network.ComponentIsSelected())
            {
                network.ChangeSelectedItemValues(nud_capacity.Value, nud_safety_limit.Value);
            }
            
        }
    }
}
