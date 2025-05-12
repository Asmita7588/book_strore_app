using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class OrderRepo : IOrderRepo
    {
        private readonly BookStoreContext context;
        public OrderRepo(BookStoreContext context) {
            this.context = context;
        }
    }
}
