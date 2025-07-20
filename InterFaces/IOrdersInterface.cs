using System;
using WebApi.Models;

namespace WepApi.InterFaces
{
    public interface IOrdersInterface
    {
        ICollection<Orders> GetOrders();
        Orders GetOrders(int id);
        bool CreateOrder(Orders order);
        bool Save();

        bool CostumerExist(int customerId);
        bool ProductExist(int productID);
        

    }
}
