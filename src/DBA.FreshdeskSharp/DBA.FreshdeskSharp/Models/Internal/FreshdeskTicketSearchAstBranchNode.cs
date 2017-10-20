namespace DBA.FreshdeskSharp.Models.Internal
{
    internal class FreshdeskTicketSearchAstBranchNode : FreshdeskTicketSearchAstNode
    {
        public FreshdeskTicketSearchAstNode Left { get; set; }
        public FreshdeskTicketSearchAstNode Right { get; set; }
    }
}