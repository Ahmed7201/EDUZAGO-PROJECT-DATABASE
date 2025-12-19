using System.ComponentModel.DataAnnotations;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Admin : User
    {
        // Admin specific fields if any. Schema says "Other fields inherited from USER".
        // PK is Admin_ID (inherited USER_ID).
    }
}
