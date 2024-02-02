using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserAccess.Infrastructure.Dtos
{
    [Table("RefreshToken")]
    public class RefreshTokenDto
    {
        [Key]
        public string UserName { get; set; }
        public string TokenID { get; set; }
        public string RefreshToken { get; set; }
        public bool IsActive { get; set; }
    }
}
