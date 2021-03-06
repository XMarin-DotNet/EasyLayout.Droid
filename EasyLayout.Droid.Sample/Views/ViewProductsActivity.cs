using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using EasyLayout.Droid.Sample.Models;

namespace EasyLayout.Droid.Sample
{
    [Activity]
    public class ViewProductsActivity : Activity
    {
        private RelativeLayout _relativeLayout;
        private ListView _listView;
        private ProductAdapter _adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetPageProperties();
            AddViews();
            ConstrainLayout();
            SetContentView(_relativeLayout);
        }

        private void SetPageProperties()
        {
            Title = "Products ListView";
        }

        private void AddViews()
        {
            _relativeLayout = ViewUtils.AddRelativeLayout(this);
            _listView = _relativeLayout.Add<ListView>();
            List<Product> items = Data.GetProducts();
            _adapter = new ProductAdapter(this, items);
            _listView.Adapter = _adapter;
        }

        private void ConstrainLayout()
        {
            _relativeLayout.ConstrainLayout(() =>
                _listView.Top == _relativeLayout.Top &&
                _listView.Right == _relativeLayout.Right &&
                _listView.Left == _relativeLayout.Left &&
                _listView.Bottom == _relativeLayout.Bottom
                );
        }
    }

    internal class ProductAdapter : BaseAdapter<Product>
    {
        private readonly Context _context;
        private readonly List<Product> _items;

        public ProductAdapter(Context context, List<Product> items)
        {
            _context = context;
            _items = items;
        }

        public override long GetItemId(int position) => _items[position].Id;

        public override int Count => _items.Count;

        public override Product this[int position] => _items[position];

        public override View GetView(int position, View view, ViewGroup parent)
        {
            var productRowView = view as ProductRowView ?? 
                new ProductRowView(_context);
            var product = _items[position];
            productRowView.Update(product);
            return productRowView;
        }

    }

    public class ProductRowView : RelativeLayout
    {
        private TextView _titleText;
        private TextView _dollarText;
        private TextView _amountText;

        public ProductRowView(Context context) : base(context)
        {
            SetViewProperties();
            AddViews();
            ConstrainLayout(this);
        }

        private void SetViewProperties()
        {
            var height = ViewUtils.DpToPx(Context, 40);
            var width = ViewGroup.LayoutParams.MatchParent;
            LayoutParameters = new ViewGroup.LayoutParams(width, height);
        }

        private void AddViews()
        {
            _titleText = this.Add<TextView>();
            _titleText.SetTextColor(Colors.YellowGreen);

            _dollarText = AddDollarText(this);
            _amountText = this.Add<TextView>();
            _amountText.SetTextColor(Colors.Green);
        }

        private static TextView AddDollarText(ViewGroup parent)
        {
            var dollarText = parent.Add<TextView>();
            dollarText.Text = "$";
            dollarText.SetTextColor(Colors.Green);
            dollarText.TextSize = 8;
            return dollarText;
        }

        private void ConstrainLayout(RelativeLayout relativeLayout)
        {
            relativeLayout.ConstrainLayout(() =>
                _titleText.Left == Left + 20
                && _titleText.Top == Top + 10

                && _amountText.Right == Right - 20
                && _amountText.Top == Top + 10

                && _dollarText.Right == _amountText.Left - 2
                && _dollarText.Top == _amountText.Top
            );
        }

        public void Update(Product product)
        {
            _titleText.Text = product.Title;
            _amountText.Text = product.Amount.ToString("0.00");
        }
    }
}