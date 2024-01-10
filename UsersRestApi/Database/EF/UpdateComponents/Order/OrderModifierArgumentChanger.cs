using ProductAPI.Database.EF.UpdateComponents.Arguments;
using ProductAPI.Database.Entities;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.EF.UpdateComponents.Arguments;

namespace ProductAPI.Database.EF.UpdateComponents.Order
{
    public class OrderModifierArgumentChanger : IOrderModifireArgumentChanger
    {
        public Dictionary<Func<OrderEntity, OrderEntity, bool>, ModifierArgumentsBase<OrderEntity, DatabaseContext>> Tracker { get; set; }
       
        public OrderModifierArgumentChanger()
        {
            InitializeTracker();
        }
       
        public void ChangeFoundModifieArguments(OrderEntity oldEntity, DatabaseContext db)
        {
            foreach (var track in Tracker)
            {
                if (track.Value.IsModified)
                {
                    track.Value.Attacher.Invoke(oldEntity, db);
                }
            }
        }
        public async Task SaveChangesAsync(DatabaseContext context)
        {
            await context.SaveChangesAsync();
        }

        public void SearchModifieArguments(OrderEntity oldEntity, OrderEntity newEntity)
        {
            foreach (var track in Tracker)
            {
                if (track.Key.Invoke(oldEntity, newEntity))
                {
                    track.Value.ValueChanger.Invoke(oldEntity, newEntity);
                    track.Value.IsModified = true;
                }
            }
        }


        public void InitializeTracker()
        {
            Tracker = new Dictionary<Func<OrderEntity, OrderEntity, bool>, ModifierArgumentsBase<OrderEntity, DatabaseContext>>
            {
                [(oldOrder, newOrder) => newOrder.TotalPrice != 0 && oldOrder.TotalPrice != newOrder.TotalPrice] = new OrderModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldOrder, newOrder) => oldOrder.TotalPrice = newOrder.TotalPrice,
                    Attacher = (oldOrder, db) => db.Entry(oldOrder).Property(p => p.TotalPrice).IsModified = true
                },
                [(oldOrder, newOrder) => newOrder.City != "" && oldOrder.City != newOrder.City] = new OrderModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldOrder, newOrder) => oldOrder.City = newOrder.City,
                    Attacher = (oldOrder, db) => db.Entry(oldOrder).Property(p => p.City).IsModified = true
                },
                [(oldOrder, newOrder) => newOrder.DeliveryAddress != "" && oldOrder.DeliveryAddress != newOrder.DeliveryAddress] = new OrderModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldOrder, newOrder) => oldOrder.DeliveryAddress = newOrder.DeliveryAddress,
                    Attacher = (oldOrder, db) => db.Entry(oldOrder).Property(p => p.DeliveryAddress).IsModified = true
                },
                [(oldOrder, newOrder) => newOrder.PaymentType != "" && oldOrder.PaymentType != newOrder.PaymentType] = new OrderModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldOrder, newOrder) => oldOrder.PaymentType = newOrder.PaymentType,
                    Attacher = (oldOrder, db) => db.Entry(oldOrder).Property(p => p.PaymentType).IsModified = true
                },
                [(oldOrder, newOrder) => newOrder.DeliveryType != "" && oldOrder.DeliveryType != newOrder.DeliveryType] = new OrderModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldOrder, newOrder) => oldOrder.DeliveryType = newOrder.DeliveryType,
                    Attacher = (oldOrder, db) => db.Entry(oldOrder).Property(p => p.DeliveryType).IsModified = true
                },
                [(oldOrder, newOrder) => newOrder.OrderStatus != "" && oldOrder.OrderStatus != newOrder.OrderStatus] = new OrderModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldOrder, newOrder) => oldOrder.OrderStatus = newOrder.OrderStatus,
                    Attacher = (oldOrder, db) => db.Entry(oldOrder).Property(p => p.OrderStatus).IsModified = true
                },
                 [(oldOrder, newOrder) => newOrder.Buyer.PhoneNumber != "" && oldOrder.Buyer.PhoneNumber != newOrder.Buyer.PhoneNumber] = new OrderModifierArguments
                 {
                     IsModified = false,
                     ValueChanger = (oldOrder, newOrder) => oldOrder.Buyer.PhoneNumber = newOrder.Buyer.PhoneNumber,
                     Attacher = (oldOrder, db) => db.Entry(oldOrder.Buyer).Property(p => p.PhoneNumber).IsModified = true
                 }
            };
        }
    }
}
