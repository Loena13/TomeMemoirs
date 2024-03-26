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
using System.Threading.Tasks;
using TomeMemoirs.Books;
using TomeMemoirs.Data;
using TomeMemoirs.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TomeMemoirs.Validation
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            this.InitializeComponent();
        }
        private async void BLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = pbPassword.Password;

            User user;
            using (var dbContext = new AppDbContext())
            {
                user = dbContext.Users.FirstOrDefault(u => u.Name == username);
            }

            if (user != null)
            {
                bool isValidUser = SecureHasher.Verify(password, user.HashedPassWord);
                if (isValidUser)
                {
                    HomeWindow home = new HomeWindow();
                    home.LoggedInUser = user;
                    home.Activate();
                    this.Close();
                }
                else
                {
                    await ShowErrorMessage("Password incorrect. Please try again.");
                }
            }
            else
            {
                await ShowErrorMessage("User not found. Check if the username is correct.");
            }
        }

        private async Task ShowErrorMessage(string message)
        {
            await cdLoginDialog.ShowAsync();
        }

        public User GetUserByUsername(string username)
        {
            using (var dbContext = new AppDbContext())
            {
                return dbContext.Users.FirstOrDefault(u => u.Name == username);
            }
        }
    }
}
