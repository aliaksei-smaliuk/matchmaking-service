namespace MatchmakingService.DataAccess.Redis.Configurations;

public record RedisOptions
{
    public string ConnectionString { get; set; } = null!;
    public string ActivityPlayerPoolPath { get; set; } = null!;
    public string ScorePlayerPoolPath { get; set; } = null!;
    public string MatchMakingPlayerDataPath { get; set; } = null!;
}