using OWTournamentsHistory.Common.Settings;

namespace OWTournamentsHistory.Api.DI
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.RegisterConfiguration<OWTournamentsHistoryDatabaseSettings>();
            builder.RegisterConfiguration<DropboxApiSettings>();
            builder.RegisterConfiguration<TwitchApiSettings>();
            builder.RegisterConfiguration<ApplicationAuthenticationSettings>();
        }

        public static void RegisterConfiguration<T>(this WebApplicationBuilder builder, string? configSectionName = default)
            where T : class =>
            builder.Services.Configure<T>(builder.Configuration.GetRequiredSection(configSectionName ?? nameof(T)));
    }
}
