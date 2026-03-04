using Website_Progress.Models;
using Website_Progress.ModelsDTO;


namespace Website_Progress.Helpers
{
    public static class Mapping
    {
        #region News
        public static List<NewsViewModel> ToNewsViewModels(this List<News> newsDb)
        {
            var newsViewModel = new List<NewsViewModel>();

            foreach (var newDb in newsDb)
            {
                newsViewModel.Add(newDb.ToNewsViewModel());
            }

            return newsViewModel;
        }

        public static NewsViewModel ToNewsViewModel(this News newsDb)
        {
            return new NewsViewModel()
            {
                Id = newsDb.Id,
                Title = newsDb.Title,
                Description = newsDb.ShortText,
                ImagePath = newsDb.ImageUrl,
                Date = newsDb.Date,
                IsOnMainPage = newsDb.IsOnMainPage,
            };
        }

        public static News ToNewsDb(this NewsViewModel product)
        {
            return new News()
            {
                Id = product.Id,
                Title = product.Title,
                ShortText = product.Description,
                ImageUrl = product.ImagePath,
                Date = product.Date,
                IsOnMainPage = product.IsOnMainPage
            };
        }

        #endregion


        #region Product
        public static List<ProductViewModel> ToProductViewModels(this List<Product> productsDb)
        {
            var productsViewModel = new List<ProductViewModel>();

            foreach (var productDb in productsDb)
            {
                productsViewModel.Add(productDb.ToProductViewModel());
            }

            return productsViewModel;
        }

        public static ProductViewModel ToProductViewModel(this Product productDb)
        {
            return new ProductViewModel()
            {
                Id = productDb.Id,
                Name = productDb.Name,
                Cost = productDb.Cost,
                Description = productDb.Description,
                PhotoPath = productDb.PhotoPath,
                PresentationPath = productDb.PresentationPath,
                FirmwarePath = productDb.FirmwarePath,
                IsOnMainPage = productDb.IsOnMainPage

            };
        }

        public static Product ToProductDb(this ProductViewModel product)
        {
            return new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                PhotoPath = product.PhotoPath,
                PresentationPath = product.PresentationPath,
                FirmwarePath = product.FirmwarePath,
                IsOnMainPage = product.IsOnMainPage

            };
        }
        #endregion

        #region Cart
        public static List<CartItemViewModel> ToCartItemViewModels(this List<CartItem> cartDbItems)
        {
            var cartItemsViewModel = new List<CartItemViewModel>();

            foreach (var cartDbItem in cartDbItems)
            {
                cartItemsViewModel.Add(cartDbItem.ToCartItemViewModel());
            }

            return cartItemsViewModel;
        }

        public static CartItemViewModel ToCartItemViewModel(this CartItem cartDbItem)
        {
            return new CartItemViewModel()
            {
                Id = cartDbItem.Id,
                Product = cartDbItem.Product.ToProductViewModel(),
                Quantity = cartDbItem.Quantity,
            };
        }

        public static CartViewModel? ToCartViewModel(this Cart? cartDb)
        {
            return cartDb == null
                ? null
                : new CartViewModel()
                {
                    Id = cartDb.Id,
                    UserId = cartDb.UserId,
                    Items = cartDb.Items.ToCartItemViewModels(),
                };
        }
        #endregion

        #region Order
        public static List<OrderViewModel> ToOrderViewModels(this List<Order> ordersDb)
        {
            var ordersViewModel = new List<OrderViewModel>();

            foreach (var orderDb in ordersDb)
            {
                ordersViewModel.Add(orderDb.ToOrderViewModel());
            }

            return ordersViewModel;
        }

        public static OrderViewModel ToOrderViewModel(this Order orderDb)
        {
            return new OrderViewModel()
            {
                Id = orderDb.Id,
                UserId = orderDb.UserId,
                Items = orderDb.Items.ToCartItemViewModels(),
                DeliveryUser = orderDb.DeliveryUser.ToDeliveryUserViewModel(),
                CreationDateTime = orderDb.CreationDateTime,
                Status = (OrderStatusViewModel)orderDb.Status
            };
        }

        public static DeliveryUserViewModel ToDeliveryUserViewModel(this DeliveryUser deliveryUserDb)
        {
            return new DeliveryUserViewModel()
            {
                Id = deliveryUserDb.Id,
                Name = deliveryUserDb.Name,
                Address = deliveryUserDb.Address,
                Phone = deliveryUserDb.Phone,
                Email = deliveryUserDb.Email,
                Telegram = deliveryUserDb.Telegram,
                Date = deliveryUserDb.Date,
                Comment = deliveryUserDb.Comment
            };
        }

        public static DeliveryUser ToDeliveryUserDb(this DeliveryUserViewModel deliveryUser)
        {
            return new DeliveryUser()
            {
                Id = deliveryUser.Id,
                Name = deliveryUser.Name,
                Address = deliveryUser.Address,
                Phone = deliveryUser.Phone,
                Email = deliveryUser.Email,
                Telegram = deliveryUser.Telegram,
                Date = deliveryUser.Date,
                Comment = deliveryUser.Comment
            };
        }
        #endregion
    }
}
