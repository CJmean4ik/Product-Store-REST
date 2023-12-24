﻿using UsersRestApi.Database.Entities;

namespace ProductAPI.Database.Entities
{
    public class FavoritesEntity
    {
        public int FavouriteId { get; set; }

        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }

        public int BuyerId { get; set; }
        public BuyerEntity Buyer { get; set; }
    }
}
