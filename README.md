# SimpleSniffer

SimpleSniffer is a very simple C# sniffer program. It logs a minimal amount of information for a "sniffer" and currently acts as more of a packet header logger.

## Description

Sample output:
{
Year,Month,Day,Hour,Minute,Second,ms,ReceiverIP,Protocol,SourceIP,Port,DestinationIP,Port,Size
2013,03,16,18,30,740,172.20.10.2,TCP,172.20.10.2,50999,2.20.1.32,443,443
2013,03,16,18,30,993,172.20.10.2,TCP,2.20.1.32,52912,222.2.14.32,443
2013,03,16,18,31,031,172.20.10.2,TCP,222.2.14.32,443,172.20.10.2,52912
2013,03,16,18,31,037,172.20.10.2,TCP,2.20.1.32,443,172.20.10.2,50999
2013,03,16,18,31,370,172.20.10.2,UDP,172.20.10.2,52391,10.0.0.3,785
2013,03,16,18,31,371,172.20.10.2,UDP,172.20.10.2,52391,10.0.0.4,7859
2013,03,16,18,31,371,172.20.10.2,UDP,172.20.10.2,52391,10.0.0.7,78
}

The syntax is very simply running the executable at a command line like so:
SimpleSniffer.exe

You can pipe the data to an output file. The format is CSV by default. You can do this like so:
SimpleSniffer.exe > output.csv

The program will bind to all available IPv4 addresses on the local machine.
It will process all packets that hit those sockets.
 
Any other questions, please let me know.