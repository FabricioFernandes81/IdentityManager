using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Models
{
    public class NavigationMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(ParentNavigationMenu))]
        public Guid? ParentMenuId { get; set; }

        public virtual NavigationMenu? ParentNavigationMenu { get; set; }

        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }

        [NotMapped]
        public bool Permitted { get; set; }
    }
}
