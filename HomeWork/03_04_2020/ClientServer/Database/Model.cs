namespace Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class Model : DbContext
    {       
        public Model()
            : base("name=Model")
        {
            Database.SetInitializer(new Initializer());
        }

        public virtual DbSet<Index_Data> IndexData { get; set; }
    }

    public class Index_Data
    {
        public int Id { get; set; }
        [Required]
        public string Index { get; set; }
        [Required]
        public string Data { get; set; }
    }

    class Initializer : DropCreateDatabaseAlways<Model>
    {
        protected override void Seed(Model context)
        {
            base.Seed(context);

            context.IndexData.AddRange(new List<Index_Data>
            {
                new Index_Data() {Index = "1", Data = "Street1,Street2,Street3,Street4"},
                new Index_Data() {Index = "2", Data = "Street11,Street22,Street33,Street44"},
                new Index_Data() {Index = "3", Data = "Street111,Street222,Street333,Street444"},
                new Index_Data() {Index = "4", Data = "Street1111,Street2222,Street3333,Street4444"}
            });

            context.SaveChanges();
        }
    }
}