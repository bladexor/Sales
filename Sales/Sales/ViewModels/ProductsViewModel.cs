


namespace Sales.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using Helpers;
    using Services;
    using System.Linq;
    using System;

    public class ProductsViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private ObservableCollection<ProductItemViewModel> products;

        private string filter;

        private bool isRefreshing;

        public List<Product> MyProducts { get; set; }

        public string Filter
        {
            get {  return this.filter;  }
            set {
                this.filter = value;
                this.RefreshList();
                }
        }
        
        
        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        #region Singleton
                private static ProductsViewModel instance;

                public static ProductsViewModel GetInstance()
                {
                    if (instance == null)
                    {
                        return new ProductsViewModel();
                    }

                    return instance;
                }
        #endregion

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.GetList<Product>(url, prefix, controller,Settings.TokenType,Settings.AccessToken);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

           this.MyProducts = (List<Product>)response.Result;
           this.RefreshList();
           this.IsRefreshing = false;

            //await Application.Current.MainPage.DisplayAlert("Data", list[0].Description, "Accept");
        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                });

                this.Products = new ObservableCollection<ProductItemViewModel>(
                    myListProductItemViewModel.OrderBy(p => p.Description));
            }
            else
            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                }).Where(p=>p.Description.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Products = new ObservableCollection<ProductItemViewModel>(
                    myListProductItemViewModel.OrderBy(p => p.Description));
            }
           
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ICommand RefreshCommand
        {
            get { return new RelayCommand(LoadProducts); }
        }

        public ICommand SearchCommand
        {
            get { return new RelayCommand(RefreshList); }
        }
    }
}
