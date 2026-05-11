using StoreApp.src.Models;
using StoreApp.src.Services;
using System.Collections.ObjectModel;
using System.Dynamic;

namespace StoreApp.Views;

public partial class AdminSecurityLogsPage : ContentPage
{
    private ObservableCollection<dynamic> _securityLogs = new();
    private readonly DatabaseService _db;
    public AdminSecurityLogsPage(DatabaseService db)
    {
        
        InitializeComponent();
        _db = db;
        LogList.ItemsSource = _securityLogs;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Admin-only access guard
        if (UserSession.CurrentUser?.UserType != "Admin")
        {
            await _db.LogSecurityEventAsync(
            "Unauthorized Admin Access",
            UserSession.CurrentUser?.Email ?? "Unknown",
            $"Attempted to access {GetType().Name}",
            UserSession.CurrentUser?.Id);

            await DisplayAlert(
                "Access Denied",
                "You are not authorized to view this page.",
                "OK"
            );
            await Navigation.PopAsync();
            return;
        }
        await LoadLogsAsync();
    }



    private async Task LoadLogsAsync()
    {
        var logs = await _db.GetSecurityLogsAsync();
        _securityLogs.Clear();
        foreach (var log in logs)
            _securityLogs.Add(log);

        // Update unread count label
        UnreadLabel.Text = $"{logs.Count(l => !l.IsRead)} unread events";
    }

    private async void OnViewDetailsClicked(object sender, EventArgs e)
    {
        var log = (SecurityLog)((Button)sender).CommandParameter;

        await DisplayAlert("Security Event",
            $"Type: {log.Type}\n" +
            $"Email: {log.Email}\n" +
            $"Time: {log.OccurredAt:MM/dd/yyyy HH:mm}\n" +
            $"Details: {log.Details}", "OK");

        // Mark as read
        if (!log.IsRead)
        {
            await _db.MarkLogReadAsync(log);
            await LoadLogsAsync();
        }
    }

    private async void OnClearLogsClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Clear Logs",
            "Permanently delete all security logs?", "Clear", "Cancel");
        if (!confirm) return;

        await _db.ClearSecurityLogsAsync();
        _securityLogs.Clear();
        UnreadLabel.Text = "0 unread events";
    }

    private async void OnBackClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}
