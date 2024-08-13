

namespace _4DimenssionLLC.ViewModels {
    /// <summary>
    /// ProductViewModel, LoginViewModel, MediaContentModel
    /// </summary>
    public class ContactFormViewModel {

        public int Id { get; set; } // Integer ID
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; } // Date type in C#
        public string Contact1 { get; set; }
        public string? Contact2 { get; set; } // Nullable in case the second contact is not provided
        public string Email { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public int likereaction { get; set; } // Will hold the value for the selected reaction, e.g., "like" or "love"
        public int lovereaction { get; set; } // Will hold the value for the selected reaction, e.g., "like" or "love"
        public int month { get; set; } 

    }
   
}
