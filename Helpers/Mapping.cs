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
                Description = newsDb.Description,
                ImagePath = newsDb.ImageUrl,
                Date = newsDb.Date,
                IsOnMainPage = newsDb.IsOnMainPage,
            };
        }

        public static News ToNewsDb(this NewsViewModel news)
        {
            return new News()
            {
                Id = news.Id,
                Title = news.Title,
                Description = news?.Description,
                ImageUrl = news.ImagePath,
                Date = news.Date,
                IsOnMainPage = news.IsOnMainPage
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
                ShortDescription = productDb.ShortDescription,
                PhotoPath = productDb.PhotoPath,
                PresentationPath = productDb.PresentationPath,
                FirmwarePath = productDb.FirmwarePath,
                IsOnMainPage = productDb.IsOnMainPage,
                Status = (ProductStatusViewModel)productDb.Status
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
                ShortDescription = product.ShortDescription,
                PhotoPath = product.PhotoPath,
                PresentationPath = product.PresentationPath,
                FirmwarePath = product.FirmwarePath,
                IsOnMainPage = product.IsOnMainPage,
                Status = (ProductStatus)product.Status

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
                PriceAtPurchase = cartDbItem.PriceAtPurchase
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
                    Items = cartDb.Items.ToCartItemViewModels()
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


        public static List<string> GetPhotoPaths(this string? photoPath) =>
    string.IsNullOrWhiteSpace(photoPath)
        ? []
        : photoPath.Split(';', StringSplitOptions.RemoveEmptyEntries)
                   .Select(p => p.Trim())
                   .Where(p => !string.IsNullOrEmpty(p))
                   .ToList();

        public static string? GetCoverPhoto(this string? photoPath) =>
            photoPath.GetPhotoPaths().FirstOrDefault();
    }
}
