namespace AutoLotConsoleApplication.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        [Key]
        public int Orderld { get; set; }

        public int Custld { get; set; }

        public int Carld { get; set; }

        public virtual Customers Customers { get; set; }

        public virtual Car Car { get; set; }
    }
}
