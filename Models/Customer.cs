using System;
using System.Collections.Generic;

namespace GeneralStoreAPI.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
