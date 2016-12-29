# FakeSmtpServer
SmtpServer for development environments

Installation: run with Administrator privileges FakeSmtpServer.exe install , it will install as windows service, but not run automatically


FakeSmtpServer.exe.config
```xml
<appSettings>
    <add key="SmtpPort" value="25" />   
    <!--Tokens:
    {TOs} - string joined by ,
    {From}
    {ReceviedDate:dd.MM.YYYY}
    {Subject} -->
    <add key="PathTemplate" value="d:\temp\mails\{ReceivedDate:yyyyMMdd}\{ReceivedDate:yyyyMMdd-HHmmssFFFFFFF}-{Subject}-{TOs}.eml" /> <!-- file template -->
  </appSettings>
```

Logging configuration: NLog.config
