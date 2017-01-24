using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace OOD2
{
    class SaveLoad
    {
        public bool save(string filename, List<Item> items)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, items);
            fs.Close();
            return true;
        }
        private byte[] itemsToBytes(List<Item> items)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(memoryStream, items);
                    return memoryStream.ToArray();


                }


                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
            }
        }
        private List<Item> ByteToItems(byte[] bytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var obj = bf.Deserialize(memoryStream);
                    return (List<Item>)obj;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
            }
        }


        public List<Item> load(string filename)
        {
            List<Item> loadlist = new List<Item>();
            FileStream fs = new FileStream(filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            loadlist = (List<Item>)bf.Deserialize(fs);
            fs.Close();

            return loadlist;
        }
    }
}
