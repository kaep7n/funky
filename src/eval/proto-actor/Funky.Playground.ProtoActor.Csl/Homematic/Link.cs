namespace Funky.Playground.ProtoActor
{
    public record Link(string Rel, string Href, string Title)
    {
        private const string ParentHref = "..";

        public bool IsParentRef => this.Href == ParentHref;
    }
}
