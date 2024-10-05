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
    <th colSpan="2">Request</th>
    <th colSpan="2">Response</th>
  </tr>
  <tr>
    <td rowSpan="3">api/report/get</td>
    <td rowSpan="3">GET</td>
    <td rowSpan="3">Retrieve a list of reports</td>
    <tr>
      <td>pageSize</td>
      <td>int; Size of query page</td>
      <td>reports</td>
      <td>Report array; List of retrieved reports</td>
    </tr>
    <tr>
      <td>pageNum</td>
      <td>int; Page number of query</td>
      <td>error</td>
      <td>string; Error encountered in endpoint</td>
    </tr>
  </tr>
  <tr>
    <td rowSpan="3">api/report/get/{id}</td>
    <td rowSpan="3">GET</td>
    <td rowSpan="3">Retrieve a specific report</td>
    <td rowSpan="3">id</td>
    <td rowSpan="3">int; ID of Report to query</td>
    <tr>
      <td>report</td>
      <td>Report; Report with specified id</td>
    </tr>
    <tr>
      <td>error</td>
      <td>string; Error encountered in endpoint</td>
    </tr>
  </tr>
  <tr>
    <td rowSpan="4">api/report/generate</td>
    <td rowSpan="4">POST</td>
    <td rowSpan="4">Generate report for a specified company</td>
    <td rowSpan="4">companyId</td>
    <td rowSpan="4">int; ID of Company to generate report for</td>
    <tr>
      <td>task</td>
      <td>Task; Created generate report task</td>
    </tr>
    <tr>
      <td>company</td>
      <td>Company; Associated Company for generate report task</td>
    </tr>
    <tr>
      <td>error</td>
      <td>string; Error encountered in endpoint</td>
    </tr>
  </tr>
</table>

# Architecture

# Tech Stack

# File Structure

