echo "Starting project..."
dotnet run --project llm-credit-score-api/llm-credit-score-api.csproj &
python3 mock-llm-server/main.py &
sudo socat TCP-LISTEN:443,fork TCP:localhost:5000 &
