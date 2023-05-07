namespace Models.Entities
{
    public class GuildTeamEntity : BaseEntity
    {
        public ulong GuildId { get; set; }
        public int TeamId { get; set; }
    }
}
