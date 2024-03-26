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
using TomeMemoirs.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;
using TomeMemoirs.Data;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TomeMemoirs.Books
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditWindow : Window
    {
        private Book _clickedBook;
        public EditWindow(Book clickedBook)
        {
            this.InitializeComponent();

            _clickedBook = clickedBook;

            using var db = new AppDbContext();
            // Ervan uitgaande dat BookUser altijd geassocieerd is met het geselecteerde boek
            var bookUser = db.BookUsers.FirstOrDefault(bu => bu.BookId == clickedBook.Id);

            if (bookUser != null)
            {
                tbTitle.Text = clickedBook.Title;
                tbAuthor.Text = clickedBook.Author;
                tbPublicationDate.Text = clickedBook.PublicationDate.ToString();
                tbRating.Text = bookUser.Rating.ToString();
            }
        }

        private async void bEditGame_Click_1(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();

            var clickedBook = db.Books.Find(_clickedBook.Id);

            // Valideer elk invoerveld afzonderlijk
            if (!await ValidateTitle() || !await ValidateAuthor() ||
                !await ValidateReleaseDate() || !await ValidateRating())
            {
                return; // Stop de methode als een invoer ongeldig is
            }

            // Zoeken naar de bijbehorende BookUser
            var bookUser = db.BookUsers.FirstOrDefault(bu => bu.BookId == clickedBook.Id);

            // Als BookUser niet bestaat, maken we een nieuwe aan
            if (bookUser == null)
            {
                bookUser = new BookUser();
                db.BookUsers.Add(bookUser);
            }
            else
            {

            }

            // Boekgebruiker bijwerken met de nieuwe beoordeling
            bookUser.Rating = int.Parse(tbRating.Text);

            // Boekgebruiker koppelen aan het boek
            clickedBook.BookUser = bookUser;

            clickedBook.Title = tbTitle.Text;
            clickedBook.Author = tbAuthor.Text;
            clickedBook.PublicationDate = DateTime.Parse(tbPublicationDate.Text);

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
            if (string.IsNullOrWhiteSpace(tbPublicationDate.Text))
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
    }
}
