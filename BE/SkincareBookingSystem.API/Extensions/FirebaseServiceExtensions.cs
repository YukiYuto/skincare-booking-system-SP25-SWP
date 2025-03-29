using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.API.Extensions;

public static class FirebaseServiceExtensions
{
    public static IServiceCollection AddFirebaseServices(this IServiceCollection services)
    {
        var credentialPath = Path.Combine(Directory.GetCurrentDirectory(),
            StaticConnectionString.FirebaseConfigurationFile);
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(credentialPath)
        });
        services.AddSingleton(StorageClient.Create(GoogleCredential.FromFile(credentialPath)));
        return services;
    }
}