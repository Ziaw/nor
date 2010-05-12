@echo off

if not exist lib\nrails\NRails.Console.exe goto MAKE_BOOT_WARN

lib\nrails\NRails.Console.exe %*
goto EXIT

:MAKE_BOOT_WARN
echo Error: lib\nrails\NRails.Console.exe not found, run make_boot.cmd from nrails solution folder first

:EXIT

