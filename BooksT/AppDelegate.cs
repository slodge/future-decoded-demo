using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading;
using BigTed;
using BooksD;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BooksT
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            // If you have defined a view, add it here:
            window.RootViewController  = new MyViewController();

            // make the window visible
            window.MakeKeyAndVisible();

            return true;
        }
    }

    public class MyViewController : UIViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var label = new UILabel(new RectangleF(10, 70, 300, 30));
            label.Text = "Books Search:";
            Add(label);

            var textBox = new UITextField(new RectangleF(10, 100, 300, 30));
            textBox.Placeholder = "Searching for?";
            textBox.Text = "Brian Cox";
            Add(textBox);

            var button = new UIButton(new RectangleF(10, 130, 300, 30));
            button.SetTitle("Go", UIControlState.Normal);
            button.SetTitleColor(UIColor.Red, UIControlState.Normal);
            Add(button);

            var table = new UITableView(new RectangleF(10, 160, 300, 300));
            Add(table);
            var source = new MySource();
            table.Source = source;

            button.TouchUpInside += async (sender, args) =>
            {
                BTProgressHUD.Show();
                var result = await Books.Query(textBox.Text);
                source.Volumes = result.items.Select(r => r.volumeInfo).ToList();
                table.ReloadData();
                BTProgressHUD.Dismiss();
            };

            UIView.Animate(2000, () =>
            {
                
            });

        }
    }

    public class MySource : UITableViewSource
    {
        public List<Volumeinfo> Volumes { get; set; }

        public MySource()
        {
            Volumes = new List<Volumeinfo>();
        }

        public override UITableViewCell GetCell(UITableView tableView,
            NSIndexPath indexPath)
        {
            var cell = new UITableViewCell(UITableViewCellStyle.Default, "MyCell");
            cell.TextLabel.Text = Volumes[indexPath.Row].title;
            LoadImage(cell, cell.ImageView, Volumes[indexPath.Row].imageLinks.smallThumbnail);
            return cell;
        }

        private async void LoadImage(UITableViewCell cell, UIImageView holder,
            string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var stream = await client.GetStreamAsync(url);
                    var data = NSData.FromStream(stream);
                    var image = UIImage.LoadFromData(data);
                    holder.Image = image;
                    holder.Frame = new RectangleF(0, 0, 50, 50);
                    cell.SetNeedsLayout();
                }
            }
            catch (Exception pokemon)
            {
                Debug.WriteLine("Problem getting {0} - {1} - {2}", url, pokemon.GetType().Name,
                    pokemon.Message);
            }
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return Volumes.Count;
        }
    }

}