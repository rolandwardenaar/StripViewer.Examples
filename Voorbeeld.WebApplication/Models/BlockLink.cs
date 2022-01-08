namespace Voorbeeld.WebApplication.Models
{
    public class BlockLink
    {
        public int BlockId { get; set; }
        public BlockLinkText[] BlockLinkTexts { get; set; }
        public BlockLinkAttribute[] BlockLinkAttributes { get; set; }
    }
}