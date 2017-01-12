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
        public PipelineLayout()
        {
            InitializeComponent();
            network = new PipelineNetwork(50, 5);
           
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
            switch (component)
            {
                case "Pump":
                    selected_component_picture.BackgroundImage = Properties.Resources.pump;
                    
                    break;
                case "Sink":
                    selected_component_picture.BackgroundImage = Properties.Resources.sink;
                   
                    break;
                case "Merger":
                    selected_component_picture.BackgroundImage = Properties.Resources.MergerEast;
                   
                    break;
                case "Splitter":
                    selected_component_picture.BackgroundImage  = Properties.Resources.WestSpiltter;
                    
                    break;
                case "Eraser":
                    selected_component_picture.BackgroundImage = Properties.Resources.eraser;

                    break;
                case "Mouse":
                    selected_component_picture.BackgroundImage  = null;
                    break;
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
    }
}
