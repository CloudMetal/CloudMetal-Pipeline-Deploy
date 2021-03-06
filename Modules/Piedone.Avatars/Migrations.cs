using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using Piedone.Avatars.Models;
using Piedone.Avatars.Services;

namespace Piedone.Avatars.Migrations
{
    [OrchardFeature("Piedone.Avatars")]
    public class Migrations : DataMigrationImpl
    {
        private readonly IAvatarsService _avatarsService;

        public Migrations(IAvatarsService avatarsService)
        {
            _avatarsService = avatarsService;
        }

        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(AvatarsSettingsPartRecord).Name, 
                table => table
                    .ContentPartRecord()
                    .Column<string>("AllowedFileTypeWhitelist")
                    .Column<int>("MaxFileSize")
            );

            SchemaBuilder.CreateTable(typeof(AvatarProfilePartRecord).Name, 
                table => table
                    .ContentPartRecord()
                    .Column<string>("FileExtension")
            );

            ContentDefinitionManager.AlterTypeDefinition("User",
                cfg => cfg
                    .WithPart(typeof(AvatarProfilePart).Name)
                );

            ContentDefinitionManager.AlterPartDefinition(typeof(AvatarPart).Name,
                builder => builder.Attachable());


            _avatarsService.CreateAvatarsFolder();


            return 2;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("User",
                cfg => cfg
                    .WithPart(typeof(AvatarProfilePart).Name)
                );

            return 2;
        }
    }
}