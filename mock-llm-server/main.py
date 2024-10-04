from http.server import *
import json
  
class Handler(BaseHTTPRequestHandler): 
    def do_POST(self): 
        
        self.send_response(200) 
        self.send_header('Content-type', 'application/json')
        self.end_headers() 

        response_data = {
          "id": "chatcmpl-123",
          "object": "chat.completion",
          "created": 1677652288,
          "model": "gpt-4o-mini",
          "system_fingerprint": "fp_44709d6fcb",
          "choices": [{
            "index": 0,
            "message": {
              "role": "assistant",
              "content": "",
            },
            "logprobs": None,
            "finish_reason": "stop"
          }],
          "usage": {
            "prompt_tokens": 9,
            "completion_tokens": 12,
            "total_tokens": 21,
            "completion_tokens_details": {
              "reasoning_tokens": 0
            }
          }
        }

        with open('credit_report.txt', 'r') as file:
            response_data["choices"][0]["message"]["content"] = file.read()

        json_response = json.dumps(response_data)
        self.wfile.write(json_response.encode('utf-8'))

address = ('', 5001)
server = HTTPServer(address, Handler)
print("Starting server...")
server.serve_forever()
