namespace _4DimenssionLLC.ViewModels
{
    public class BlazorResponseViewModel 
    {
        
        public bool status { get; set; } = false;
        public string keyValue { get; set; } = string.Empty;
        public Int16 effectedRows { get; set; } = 0;
        public string keyElement { get; set; } = string.Empty;
        public string returnURL { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public object data { get; set; } 
        public DateTime updateTime { get; set; } = DateTime.Now;
        
    }
}
