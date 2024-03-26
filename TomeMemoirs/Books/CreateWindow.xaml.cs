using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using TomeMemoirs.Data;
using System.Xml.Linq;
using TomeMemoirs.Model;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TomeMemoirs.Books
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateWindow : Window
    {
        public CreateWindow()
        {
            this.InitializeComponent();
        }

        private void BAddBook_Click(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();

            // Maak een nieuwe BookUser
            var bookUser = new BookUser
            {
                Rating = int.Parse(tbRating.Text),
            };

            var newBook = new Book
            {
                Title = tbTitle.Text,
                Author = tbAuthor.Text,
                PublicationDate = dbPublicationDate.SelectedDate.Value.DateTime,
                BookUser = bookUser // Associeert de boekuser met de klasse
            };

            db.Books.Add(newBook);

            db.SaveChanges();
            this.Close();
        }
    }
}
