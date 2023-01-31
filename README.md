# Record Database using EntityFramework and SQLite

## Install packages

```bash
    Install-Package Microsoft.EntityFrameworkCore.Sqlite
```

Then.

```bash
    Install-Package Microsoft.EntityFrameworkCore.Tools
```

Now you can scaffold the DbContext.

```bash
Scaffold-DbContext -provider Microsoft.EntityFrameworkCore.Sqlite "DataSource=D:\Sandbox\SQLite\RecordDBEFSQLiteApp\RecordDBEFSQLite\bin\Debug\net7.0\RecordDB.db" -OutputDir D:\Sandbox\SQLite\RecordDBEFSQLiteApp\RecordDBEFSQLite\Data -context RecordDBContext -force
```
