"%ProgramFiles%\Microsoft SQL Server\100\Tools\Binn\SQLCMD.EXE" -E -S .\sqlexpress -Q "ALTER DATABASE [NRailsDemo] SET SINGLE_USER WITH ROLLBACK IMMEDIATE"
"%ProgramFiles%\Microsoft SQL Server\100\Tools\Binn\SQLCMD.EXE" -E -S .\sqlexpress -Q "DROP DATABASE [NRailsDemo]"
