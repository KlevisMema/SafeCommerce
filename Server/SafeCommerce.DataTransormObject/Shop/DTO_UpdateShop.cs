﻿using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Shop;
public class DTO_UpdateShop
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [StringLength(400)]
    public string Description { get; set; } = string.Empty;

    public string? EncryptedSymmetricKey { get; set; }
    public string? DataNonce { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string? SignatureOfKey { get; set; }
    public string? SigningPublicKey { get; set; }
}