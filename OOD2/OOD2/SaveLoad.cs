using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD2
{
    class SaveLoad
    {
        public bool save(string filename, List<Item> items)
        {
            itemsToBytes(items);
            return true;
        }
        private byte[] itemsToBytes(List<Item> items)
        {

        }
        private List<Item> ByteToItems(byte[] bytes)
        {
            
        }
        public List<Item> load(string filename)
        {
            byte[] bytes;
            return ByteToItems(bytes);
        }
    }
}
