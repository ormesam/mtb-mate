sqlcmd -E -S localhost\SQLSERVER17 -i ..\..\Database\Create\DropAndCreate.sql
sqlcmd -E -S localhost\SQLSERVER17 -d MtbMateDev -i ..\..\Database\Create\Tables\User.sql

cd /d "..\..\DataAccess"
dotnet-ef dbcontext scaffold "Server=localhost\SQLSERVER17;Database=MtbMateDev;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models --context-dir Models -c ModelDataContext --force

pause