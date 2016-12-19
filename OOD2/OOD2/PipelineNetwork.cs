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
        SaveLoad fileHandler;
        public PipelineNetwork()
        {
            items = new List<Item>();
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
        private Point calEnterPoint(Item i, Item i2)
        {

        }
        private Point calExitPoint(Item i, Item i2)
        {

        }
    }
}
