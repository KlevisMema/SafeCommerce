using MudBlazor;

namespace SafeCommerce.Client.Internal.Helpers;
public static class DialogHelper
{
    public static DialogOptions
    SimpleDialogOptions()
    {
        return new()
        {
            CloseOnEscapeKey = true,
            BackdropClick = true,
            CloseButton = true,
            Position = DialogPosition.Center,
        };
    }

    public static DialogOptions
    BigDialog()
    {
        return new()
        {
            CloseOnEscapeKey = true,
            BackdropClick = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true,
            MaxWidth = MaxWidth.ExtraLarge,
        };
    }

    public static DialogOptions
    DialogOptions()
    {
        return new()
        {
            BackgroundClass = "my-custom-class",
            CloseOnEscapeKey = false,
            BackdropClick = true,
            CloseButton = true,
            Position = DialogPosition.Center,
        };
    }

    public static DialogOptions
    DialogOptionsNoCloseButton()
    {
        return new()
        {
            BackgroundClass = "my-custom-class",
            CloseOnEscapeKey = false,
            BackdropClick = false,
            CloseButton = false,
            Position = DialogPosition.TopCenter,
        };
    }
}