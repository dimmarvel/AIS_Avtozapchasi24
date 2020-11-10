using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoShop
{
    public class DataBase
    {
        public int      _id;
        public string   _zapchast;
        public string   _mark;
        public string   _izgotovitel;
        public int      _price_per_one;
        public int      _count;

        public void Add_data(int id, string zap, string mark, string izg, int price, int count)
        {
            _id             = id;
            _zapchast       = zap;
            _mark           = mark;
            _izgotovitel    = izg;
            _price_per_one  = price;
            _count          = count;
        }
    }
}
