echo "Starting project..."
dotnet run --project llm-credit-score-api/llm-credit-score-api.csproj &
python3 mock-llm-server/main.py &
wait
