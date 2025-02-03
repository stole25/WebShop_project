namespace Webshop_Frontend.Services;

using System;
using Microsoft.AspNetCore.Components;

public class AppState
{
    public event Action OnChange;
    public string CurrentError { get; private set; }

    public void ReportError(string message)
    {
        CurrentError = message;
        NotifyStateChanged();
    }

    public void ClearErrors()
    {
        CurrentError = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}