/*
* Defines the ApplicationUser class that represents registered users within the system.
* This file contains definitions for the ApplicationUser's properties and their relationships.
*/


/*
* Defines the ApplicationUser class that represents registered users within the system.
* This file contains definitions for the ApplicationUser's properties and their relationships.
*/

using SafeCommerce.DataAccess.BaseModels;
using SafeShare.DataAccessLayer.Models;

namespace SafeCommerce.DataAccess.Models;

/// <summary>
/// Represents a registered user in the system.
/// Inherits from IdentityUser with Guid as the primary key.
/// </summary>
public class ApplicationUser : BaseIdentity
{
    public byte[]? ImageData { get; set; }
    public virtual ICollection<Shop>? Shops { get; set; }
    public virtual ICollection<Item>? Items { get; set; }

    public virtual ICollection<ShopInvitation>? SentInvitations { get; set; }
    public virtual ICollection<ShopInvitation>? ReceivedInvitations { get; set; }
}
