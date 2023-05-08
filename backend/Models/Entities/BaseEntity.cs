using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }    
    }
}
