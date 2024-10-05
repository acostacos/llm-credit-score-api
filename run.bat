@echo off

echo "Starting project..."
start dotnet run --project llm-credit-score-api/llm-credit-score-api.csproj
start python mock-llm-server/main.py
