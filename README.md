# Record Database using EntityFramework and SQLite

Source code: ``Projects\RecordDBEFSQLiteApp``.

Add the ``RecordDB.db`` database into the ``Environment.CurrentDirectory`` folder.

## Install packages

```bash
    Install-Package Microsoft.EntityFrameworkCore.Sqlite
```

Then.

```bash
    Install-Package Microsoft.EntityFrameworkCore.Tools
```

Then.

```bash
    Install-Package System.Configuration.ConfigurationManager
```

## Scaffold the DbContext

```bash
Scaffold-DbContext -provider Microsoft.EntityFrameworkCore.Sqlite "DataSource=D:\Projects\RecordDBEFSQLiteApp\RecordDBEFSQLite\bin\Debug\net7.0\RecordDB.db" -OutputDir D:\Projects\RecordDBEFSQLiteApp\RecordDBEFSQLite\Data -context RecordDbContext -force
```

This will produce the connection string in the ``OnConfiguring`` service.

```csharp
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        => optionsBuilder.UseSqlite("DataSource=D:\\Projects\\RecordDBEFSQLiteApp\\RecordDBEFSQLite\\bin\\Debug\\net7.0\\RecordDB.db");
    }
```

This also gives a warning message that the connection string should be abstracted out of here.

We can do this by moving the connection string into the **App.Config** file.

### App.Config

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <connectionStrings>
        <add name="RecordDB" connectionString="Data Source=D:\Projects\RecordDBEFSQLiteApp\RecordDBEFSQLite\bin\Debug\net7.  0\RecordDB.db;Version=3;" providerName="System.Data.SqlClient"/>
    </connectionStrings>
</configuration>
```

Now we can replace the connection string in the ``OnConfiguring`` service.

```csharp
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["RecordDB"].ConnectionString;
        optionsBuilder.UseSqlite(constr);
    }
```

**Note:** the RecordDB context that is created is nowhere near as sophisticated as a SQL Server context but you should be able to add more to the context model to make it more usable.
