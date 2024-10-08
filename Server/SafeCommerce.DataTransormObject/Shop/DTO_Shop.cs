﻿using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.DataTransormObject.Shop;

public class DTO_Shop
{
    public Guid ShopId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerPublicKey { get; set; } = string.Empty;
    public string EncryptedKey { get; set; } = string.Empty;
    public string EncryptedKeyNonce { get; set; } = string.Empty;
    public string DataNonce { get; set; } = string.Empty;
    public bool MakePublic { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int MemberCount { get; set; }
    public int ItemCount { get; set; }
    public ICollection<DTO_ShopShare>? ShopShares { get; set; }
    public ICollection<DTO_Item>? Items { get; set; }
}