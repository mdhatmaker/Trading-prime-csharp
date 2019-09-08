#SentToProwl

 A simple .NET C# implementation of the Prowl notification service.
 
 Included is a basic command line application to send a notification.

###Class Usage

```
  var notification = new ProwlNotification()
  {
	  ApiKey = "xxxxxxxxxxxx",
	  Application = "Send To Prowl",
	  Event = "Test Notification",
	  Description = "Just testing notifications",
	  Priority = 1;
  };
  
  var client = new ProwlClient();
  client.SendNotification(notification);
```

###Command Line Usage

```
SendToProwl [OPTIONS]
   OPTIONS (required):
   -k key               The API key
   -a application       Application name
   -e event             Event (e.g. "Build Complete")
   -d description       A description, generally terse
   
   OPTIONS (optional)
   -u url               The Url to display
   -p priority          The notification priority 
                        (-2, -1, 0, 1, 2 (very low, moderate, normal, high, emergency))
```

###Command Line Example
```
SendToProwl.exe -k xxxxxxxxxxxx -a "Send To Prowl" -e "Test Notification" -d "Just testing notifications."
```
