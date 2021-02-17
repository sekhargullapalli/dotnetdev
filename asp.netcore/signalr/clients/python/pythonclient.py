import ssl
import websocket
import json

ws = None


def ws_on_error(ws, error):
    print(error)

def ws_on_close(ws):
    print('#### Disconnected from SignalR server##')

def ws_on_message(ws, message:str):
    ignore_list = ['{"type":6}', '{}']
    for msg in message.split(chr(0x1E)):
        if msg and msg not in ignore_list:
            print(f"From server: {msg}")                  

def encode_json(obj):    
    return json.dumps(obj) + chr(0x1E)

def ws_on_open(ws):
    print("### Connected to SignalR Server via WebSocket ###")
    
    # Do a handshake request
    print("### Performing handshake request ###")
    ws.send(encode_json({
        "protocol": "json",
        "version": 1
    }))


    # Handshake completed
    print("### Handshake request completed ###") 

    #Sending a sample message
    #https://github.com/dotnet/aspnetcore/blob/main/src/SignalR/docs/specs/HubProtocol.md
    ws.send(encode_json({
        "type":1,  
        "invocationId ":"1234",      
        "target" : "SendMessage",
        "arguments" : ["PythonClient", "HelloWorld!", True ] 
        }))

if __name__ == "__main__":     
    websocket.enableTrace(True)
    url = 'wss://localhost:5001/generichub'
    ws = websocket.WebSocketApp(url,
                              on_message = ws_on_message,
                              on_error = ws_on_error,
                              on_close = ws_on_close)
    ws.on_open = ws_on_open
    ws.run_forever(sslopt={"cert_reqs": ssl.CERT_NONE})
    
