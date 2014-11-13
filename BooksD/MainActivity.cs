using System;
using System.Net.Http;
using System.Net.Mime;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AndroidHUD;
using Java.Net;
using Debug = System.Diagnostics.Debug;

namespace BooksD
{
    [Activity(Label = "BooksD", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var label = new TextView(this);
            label.Text = "Search for....";
            layout.AddView(label);

            var textBox = new EditText(this);
            textBox.Text = "Brian Cox";
            layout.AddView(textBox);

            var button = new Button(this);
            button.Text = "Go";
            layout.AddView(button);

            var listView = new ListView(this);
            layout.AddView(listView);
            var adapter = new MyAdapter(this, Android.Resource.Layout.SimpleListItem1);
            listView.Adapter = adapter;


            button.Click += async (sender, args) =>
            {
                AndHUD.Shared.Show(this);
                var root = await Books.Query(textBox.Text);
                adapter.Clear();
                foreach (var item in root.items)
                {
                    adapter.Add(item.volumeInfo);
                }
                adapter.NotifyDataSetChanged();
                AndHUD.Shared.Dismiss(this);
            };

            this.SetContentView(layout);
        }
    }

    public class MyAdapter : ArrayAdapter<Volumeinfo>
    {
        public MyAdapter(Context context, int ignoredResourceId = -1)
            : base(context, ignoredResourceId)
        {
        }

        public override View GetView(int position, View convertView,
            ViewGroup parent)
        {
            // not production code!
            var linearLayout = new LinearLayout(Context);
            linearLayout.Orientation = Orientation.Horizontal;

            var imageView = new ImageView(Context);
            linearLayout.AddView(imageView);
            imageView.LayoutParameters = new LinearLayout.LayoutParams(100, 100);

            var label = new TextView(Context);
            linearLayout.AddView(label);

            var volumeInfo = GetItem(position);
            label.Text = volumeInfo.title;

            DownloadAndShow(imageView, volumeInfo.imageLinks.smallThumbnail);

            return linearLayout;
        }

        private async void DownloadAndShow(ImageView imageView, string url)
        {
            // not production code!
            try
            {
                var httpClient = new HttpClient();
                var bytes = await httpClient.GetByteArrayAsync(url);
                var bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                imageView.SetImageBitmap(bitmap);
            }
            catch (Exception pokemon)
            {
                Debug.WriteLine("Problem getting {0} - {1} - {2}", url, pokemon.GetType().Name,
                    pokemon.Message);
            }
        }
    }
}

