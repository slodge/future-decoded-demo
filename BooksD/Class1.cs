using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace BooksD
{
    public class Books
    {
        public static async Task<Rootobject> Query(string term)
        {
            using (var http = new HttpClient())
            {
                var url = string.Format("https://www.googleapis.com/books/v1/volumes?q={0}",
                    Uri.EscapeUriString(term));

                var result = await http.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Rootobject>(result);
            }
        }
    }


    public class Rootobject
    {
        public string kind { get; set; }
        public int totalItems { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public Volumeinfo volumeInfo { get; set; }
        public Saleinfo saleInfo { get; set; }
        public Accessinfo accessInfo { get; set; }
        public Searchinfo searchInfo { get; set; }
    }

    public class Volumeinfo
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string[] authors { get; set; }
        public string publisher { get; set; }
        public string publishedDate { get; set; }
        public string description { get; set; }
        public Industryidentifier[] industryIdentifiers { get; set; }
        public Readingmodes readingModes { get; set; }
        public int pageCount { get; set; }
        public string printType { get; set; }
        public string[] categories { get; set; }
        public float averageRating { get; set; }
        public int ratingsCount { get; set; }
        public string contentVersion { get; set; }
        public Imagelinks imageLinks { get; set; }
        public string language { get; set; }
        public string previewLink { get; set; }
        public string infoLink { get; set; }
        public string canonicalVolumeLink { get; set; }
    }

    public class Readingmodes
    {
        public bool text { get; set; }
        public bool image { get; set; }
    }

    public class Imagelinks
    {
        public string smallThumbnail { get; set; }
        public string thumbnail { get; set; }
    }

    public class Industryidentifier
    {
        public string type { get; set; }
        public string identifier { get; set; }
    }

    public class Saleinfo
    {
        public string country { get; set; }
        public string saleability { get; set; }
        public bool isEbook { get; set; }
        public Listprice listPrice { get; set; }
        public Retailprice retailPrice { get; set; }
        public string buyLink { get; set; }
        public Offer[] offers { get; set; }
    }

    public class Listprice
    {
        public float amount { get; set; }
        public string currencyCode { get; set; }
    }

    public class Retailprice
    {
        public float amount { get; set; }
        public string currencyCode { get; set; }
    }

    public class Offer
    {
        public int finskyOfferType { get; set; }
        public Listprice1 listPrice { get; set; }
        public Retailprice1 retailPrice { get; set; }
    }

    public class Listprice1
    {
        public float amountInMicros { get; set; }
        public string currencyCode { get; set; }
    }

    public class Retailprice1
    {
        public float amountInMicros { get; set; }
        public string currencyCode { get; set; }
    }

    public class Accessinfo
    {
        public string country { get; set; }
        public string viewability { get; set; }
        public bool embeddable { get; set; }
        public bool publicDomain { get; set; }
        public string textToSpeechPermission { get; set; }
        public Epub epub { get; set; }
        public Pdf pdf { get; set; }
        public string webReaderLink { get; set; }
        public string accessViewStatus { get; set; }
        public bool quoteSharingAllowed { get; set; }
    }

    public class Epub
    {
        public bool isAvailable { get; set; }
        public string acsTokenLink { get; set; }
    }

    public class Pdf
    {
        public bool isAvailable { get; set; }
        public string acsTokenLink { get; set; }
    }

    public class Searchinfo
    {
        public string textSnippet { get; set; }
    }

}