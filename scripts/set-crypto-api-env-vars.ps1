# Set the API key/secret combinations for various crypto exchanges
# API key and secret should be in a single string separated by vertical bar ('|').
# If no secret is required (as with BITTREX), leave "(secret)" as the secret since an empty secret causes an error.
# Examples:
#    SetEnvironmentVariable('BITFINEX_KEY','ZXacbKmno8qvz2uKmno8qvz2uKmno8qvz2uKmno8qvz|AQD1e7jyu6vhjke3evb9mn2D1e7jyu6vhjke3evb9mn',[System.EnvironmentVariableTarget]::User)
#    SetEnvironmentVariable('BITTREX_KEY','1e7jyu6vhjke3evb9mn2D1e7jyu6vhjk|(secret)',[System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable('BINANCE_KEY','(key)|(secret)',[System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable('BITTREX_KEY','(key)|(secret)',[System.EnvironmentVariableTarget]::User)
[System.Environment]::SetEnvironmentVariable('BITFINEX_KEY','(key)|(secret)',[System.EnvironmentVariableTarget]::User)
