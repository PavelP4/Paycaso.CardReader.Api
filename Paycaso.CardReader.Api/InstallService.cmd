sc create Paycaso.CardReader.Api binpath=c:\Paycaso.CardReader.Api\Paycaso.CardReader.Api.exe start=auto
sc description Paycaso.CardReader.Api "Paycaso cardreader integraion service."

rem sc.exe failure Paycaso.CardReader.Api reset=0 actions=restart/5000/restart/10000/run/1000

net start Paycaso.CardReader.Api

pause