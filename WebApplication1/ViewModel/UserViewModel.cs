using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;

namespace OnePixelBE.ViewModel
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public List<UserPermissionViewModel> Permissions { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string SecondEmail { get; set; }
        public bool IsConfirmed { get; set; }
        public UserType Type { get; set; }
        public UserRole Role { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string LocalNumber { get; set; }
        public string Token { get; set; }
    }
}
