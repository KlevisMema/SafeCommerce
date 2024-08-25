﻿using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_InvitationItemRequestActions
{
    [Required]
    public Guid InvitingUserId { get; set; }

    [Required]
    public Guid ItemId { get; set; }

    [Required]
    public Guid InvitationId { get; set; }

    [Required]
    public Guid InvitedUserId { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public string UserWhoAcceptedTheInvitation { get; set; } = string.Empty;
}