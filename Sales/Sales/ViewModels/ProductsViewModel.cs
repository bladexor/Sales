


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
    using System.Threading.Tasks;

    public class ProductsViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private readonly DataService dataService;
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
            this.dataService = new DataService();
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
            if (connection.IsSuccess)
            {
                var answer = await this.LoadProductsFromAPI();
                if (answer)
                {
                    await this.SaveProductsToDB();
                }
            }
            else
            {
                await this.LoadProductsFromDB();
            }
            if (this.MyProducts==null || this.MyProducts.Count == 0)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoProductsMessage, Languages.Accept);

               return;
            }

            this.RefreshList();
            this.IsRefreshing = false;
        }

        private async Task LoadProductsFromDB()
        {
            this.MyProducts = await this.dataService.GetAllProducts();
        }

        private async Task SaveProductsToDB()
        {
            await this.dataService.DeleteAllProducts();
            await this.dataService.Insert(this.MyProducts);
        }

        private async Task<bool> LoadProductsFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.GetList<Product>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyProducts = (List<Product>)response.Result;
            return true;
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
