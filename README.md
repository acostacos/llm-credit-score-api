# LLM Credit Score API
This was an API for the Asian Institute of Digital Finance Backend Technical Interview.
It's primary function is to generate a credit rating report using an LLM and display the results. For this impelemntation,
we are just simulating an API call to the LLM by running a mock server that returns a dummy report.
The generation of a report is initiated through the `api/report/generate` endpoint. This will return a task that can be monitored
through the `api/task/get` endpoint. Once the task has a status of "Done", we can query the report associated with this task through
the `api/report/get` endpoint.

# Running the Program
### System Requirements:
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) = Version 8.0.400
- [Python](https://www.python.org/downloads/) = Version 3.12.6

### Windows
Run the *run.bat* script located in the base directory. 

### Linux and Mac
Run the *run.sh* script located in the base directory. 

# Endpoints

### Report Endpoint
<table>
  <tr>
    <th>Endpoint</th>
    <th>HTTP Method</th>
    <th>Description</th>
    <th>Request</th>
    <th>Response</th>
  </tr>
  <tr>
    <td>api/report/get</td>
    <td>GET</td>
  </tr>
  <tr>
    <td>api/report/get/{id}</td>
    <td>GET</td>
  </tr>
  <tr>
    <td>api/report/generate</td>
    <td>POST</td>
  </tr>
</table>

# Architecture

# Tech Stack

# File Structure

