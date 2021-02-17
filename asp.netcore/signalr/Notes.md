# Overview


## Transports
SignalR automatically chooses the transport methods that is within the capabilities of the server and the client in the order of graceful fallback (1. Web Sockets, 2. Server Sent Events and 3. Long Polling)

 
  https://localhost:5001/generichub/negotiate?negotiateVersion=1

curl -x POST -F "negotiateVersion=1" https://localhost:5001/generichub/negotiate


curl -X POST -F "negotiateVersion=1" https://localhost:5001/generichub/negotiate
{"negotiateVersion":0,"connectionId":"T8Oj4zt49XPxNoTz-oT1vA","availableTransports":[{"transport":"WebSockets","transferFormats":["Text","Binary"]},{"transport":"ServerSentEvents","transferFormats":["Text"]},{"transport":"LongPolling","transferFormats":["Text","Binary"]}]}

## Reference
1. https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-5.0

