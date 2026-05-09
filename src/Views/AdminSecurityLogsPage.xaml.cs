using System.Collections.ObjectModel;
using System.Dynamic;
using StoreApp.src.Services;

namespace StoreApp.Views;

public partial class AdminSecurityLogsPage : ContentPage
{
    private ObservableCollection<dynamic> _securityLogs;

    public AdminSecurityLogsPage()
    {
        InitializeComponent();
        LoadStubLogs();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Admin-only access guard
        if (UserSession.CurrentUser?.UserType != "Admin")
        {
            await DisplayAlert(
                "Access Denied",
                "You are not authorized to view this page.",
                "OK"
            );
            await Navigation.PopAsync();
        }
    }

    private void LoadStubLogs()
    {
        _securityLogs = new ObservableCollection<dynamic>
        {
            CreateLog(
                "Unauthorized Admin Access",
                "user1@test.com",
                "2026-04-15 14:22"
            ),
            CreateLog(
                "Failed Login Attempt",
                "unknown@test.com",
                "2026-04-16 09:10"
            ),
            CreateLog(
                "Multiple Failed Logins",
                "seller2@test.com",
                "2026-04-16 18:45"
            )
        };

        LogList.ItemsSource = _securityLogs;
    }

    private dynamic CreateLog(string type, string email, string time)
    {
        dynamic log = new ExpandoObject();
        log.Type = type;
        log.Email = email;
        log.Time = time;
        return log;
    }

    private async void OnViewDetailsClicked(object sender, EventArgs e)
    {
        dynamic log = ((Button)sender).CommandParameter;

        await DisplayAlert(
            "Security Event",
            $"Type: {log.Type}\nEmail: {log.Email}\nTime: {log.Time}",
            "OK"
        );
    }

    private async void OnClearLogsClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Clear Logs",
            "Clear all security logs?",
            "Clear",
            "Cancel"
        );

        if (confirm)
        {
            _securityLogs.Clear();
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}