using System.ComponentModel.DataAnnotations;

namespace Info.Models
{
    public class ApplicationType
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }
    }
}
