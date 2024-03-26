using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TomeMemoirs.Data;
using TomeMemoirs.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TomeMemoirs.Books
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeWindow : Window
    {
        public User LoggedInUser;
        public HomeWindow()
        {
            this.InitializeComponent();
            using var db = new AppDbContext();
            var books = db.Books
                .Include(b => b.BookUser)
                .ToList();
            lvBook.ItemsSource = books;
        }

        private void bAddBook_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateWindow();
            createWindow.Activate();
            createWindow.Closed += CreateWindow_Closed;
        }
        private void CreateWindow_Closed(object sender, WindowEventArgs args)
        {
            using var db = new AppDbContext();
            lvBook.ItemsSource = db.Books;
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvBook.SelectedItem is Book selectedBook)
            {
                using var db = new AppDbContext();
                db.Books.Remove(selectedBook);
                db.SaveChanges();

                lvBook.ItemsSource = db.Books.ToList();
            }
        }

        private void lvBook_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is Book clickedBook)
            {
                var editWindow = new EditWindow(clickedBook);
                editWindow.Activate();
                editWindow.Closed += EditGame_Closed;
            }
        }

        private void EditGame_Closed(object sender, WindowEventArgs args)
        {
            using var db = new AppDbContext();
            lvBook.ItemsSource = db.Books;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchInput = tbSearch.Text;
            using var db = new AppDbContext();
            var books = db.Books
                .Where(t => t.Title == searchInput)
                .Include(a => a.Author == searchInput);
            lvBook.ItemsSource = books;
        }

        private void bBack_Click(object sender, RoutedEventArgs e)
        {
            var backToPreviouspage = new MainWindow();
            backToPreviouspage.Activate();
            this.Close();
        }

        private void bAuthor_Click(object sender, RoutedEventArgs e)
        {
            //TODO for next sprint
        }
    }
}
