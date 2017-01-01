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
            return null;
        }
        private List<Item> ByteToItems(byte[] bytes)
        {
            return null;
        }
        public List<Item> load(string filename)
        {
            byte[] bytes=null;
            return ByteToItems(bytes);
        }
    }
}
