import subprocess, sys, os
p = subprocess.Popen(["powershell.exe",
       "cd {0} ; ./dotnet-install.ps1 -RunTime dotnet -InstallDir ./dotnetRuntime ; ./dotnetRuntime/dotnet.exe run --project ./Advent2021".format(os.getcwd())],
       stdout=sys.stdout)
p.communicate()

