using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DALClass
    {
        private Model _ctx;

        public DALClass()
        {
            _ctx = new Model();
        }

        public string GetResponse(string index)
        {
            Console.WriteLine(index);
            var index_Data = _ctx.IndexData.FirstOrDefault(p => p.Index == index);
            if (index_Data == null) return "I dont understand";
            return index_Data.Data;
        }

        public void AddIndexData(string ind, string data)
        {
            _ctx.IndexData.Add(new Index_Data() { Index = ind, Data = data });
            _ctx.SaveChanges();
        }
    }
}
