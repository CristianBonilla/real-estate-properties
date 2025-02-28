BEGIN TRANSACTION;
DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[PropertyTrace]') AND [c].[name] = N'Value');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [dbo].[PropertyTrace] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [dbo].[PropertyTrace] ALTER COLUMN [Value] decimal(14,2) NOT NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[PropertyTrace]') AND [c].[name] = N'Tax');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[PropertyTrace] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [dbo].[PropertyTrace] ALTER COLUMN [Tax] decimal(14,2) NOT NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[dbo].[Property]') AND [c].[name] = N'Price');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [dbo].[Property] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [dbo].[Property] ALTER COLUMN [Price] decimal(14,2) NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250301232129_ChangedPropertiesPrecision', N'9.0.1');

COMMIT;
GO
