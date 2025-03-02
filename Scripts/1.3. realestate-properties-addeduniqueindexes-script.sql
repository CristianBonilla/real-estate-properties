BEGIN TRANSACTION;
CREATE UNIQUE INDEX [IX_PropertyTrace_Name] ON [dbo].[PropertyTrace] ([Name]);

CREATE UNIQUE INDEX [IX_PropertyImage_ImageName] ON [dbo].[PropertyImage] ([ImageName]);

CREATE UNIQUE INDEX [IX_Property_Name_CodeInternal] ON [dbo].[Property] ([Name], [CodeInternal]);

CREATE UNIQUE INDEX [IX_Owner_Name] ON [dbo].[Owner] ([Name]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250302013726_AddedUniqueIndexes', N'9.0.2');

COMMIT;
GO
