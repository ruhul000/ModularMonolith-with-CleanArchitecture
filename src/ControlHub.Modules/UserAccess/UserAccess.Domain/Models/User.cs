namespace UserAccess.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string? VerificationToken { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
