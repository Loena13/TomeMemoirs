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
using Microsoft.EntityFrameworkCore.Update;
using System.Threading.Tasks;

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

        private async void BAddBook_Click(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();

            // Valideer elk invoerveld afzonderlijk
            if (!await ValidateTitle() || !await ValidateAuthor() ||
                !await ValidateReleaseDate() || !await ValidateRating())
            {
                return; // Stop de methode als een invoer ongeldig is
            }

            var bookUser = new BookUser();
            if (int.TryParse(tbRating.Text, out int rating))
            {
                bookUser.UserId = User.LoggedInUser.Id;
                bookUser.Rating = rating;
            }
            else
            {
                // Behandel het mislukken van het parsen voor de beoordeling
                await cdRatingDialog.ShowAsync();
                return;
            }

            var newBook = new Book
            {
                Id = GenerateRandomId(), // Genereer een willekeurig ID
                Title = tbTitle.Text,
                Author = tbAuthor.Text,
                PublicationDate = dbPublicationDate.SelectedDate.Value.DateTime,
                BookUser = bookUser // Koppel de boekgebruiker aan de klasse
            };

            db.Books.Add(newBook);

            db.SaveChanges();
            this.Close();
        }

        private async Task<bool> ValidateTitle()
        {
            // Valideer invoervelden
            if (string.IsNullOrWhiteSpace(tbTitle.Text))
            {
                await cdTitleDialog.ShowAsync();
                return false; // Ongeldige invoer
            }

            return true; // Invoer is geldig
        }

        private async Task<bool> ValidateAuthor()
        {
            // Valideer invoervelden
            if (string.IsNullOrWhiteSpace(tbAuthor.Text))
            {
                await cdAuthorDialog.ShowAsync();
                return false; // Ongeldige invoer
            }

            return true; // Invoer is geldig
        }

        private async Task<bool> ValidateReleaseDate()
        {
            // Valideer invoervelden
            if (dbPublicationDate.SelectedDate == null)
            {
                await cdPublicationDateDialog.ShowAsync();
                return false; // Ongeldige invoer
            }

            return true; // Invoer is geldig
        }

        private async Task<bool> ValidateRating()
        {
            // Valideer invoervelden
            if (!int.TryParse(tbRating.Text, out _))
            {
                await cdRatingDialog.ShowAsync();
                return false; // Ongeldige invoer
            }

            return true; // Invoer is geldig
        }

        private int GenerateRandomId()
        {
            // Genereer een willekeurige integer ID
            Random random = new Random();
            return random.Next();
        }
    }
}
