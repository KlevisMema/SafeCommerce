using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Moderation;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Pages.Moderator;

public partial class ModerationHistory
{
    [Inject] private IClientService_Moderation ModerationService { get; set; } = null!;

    private ClientDto_SplittedModerationHistory SplittedModerationHistory { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await ModerationService.GetModerationHistoryForModerator();

        if (result.Succsess)
            SplittedModerationHistory = result.Value!;
    }
}