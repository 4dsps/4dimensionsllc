

namespace _4DimenssionLLC.ViewModels {
    /// <summary>
    /// ProductViewModel, LoginViewModel, MediaContentModel
    /// </summary>
    public class ContactsStatsViewModel
	{
        public int id { get; set; } // Will hold the value for the selected reaction, e.g., "like" or "love"
        public int likecount { get; set; } // Will hold the value for the selected reaction, e.g., "like" or "love"
        public int lovecount { get; set; } // Will hold the value for the selected reaction, e.g., "like" or "love"
        public string MonthName { get; set; } 
        public int Months { get; set; } 

    }
   
}
