namespace IdentityServer.Models
{
    public class MenuViewModel
    {
  

        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid? ParentMenuId { get; set; }

        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }

        public bool Permitted { get; set; }
        public Guid? GroupMenu { get; set; }
        public bool isParent { get; set; }

        

    }
    
}
