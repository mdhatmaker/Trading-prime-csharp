# Metaco itBit Rest API client for .NET [![Build Status](https://travis-ci.org/MetacoSA/metaco-itbit-client.svg?branch=master)](https://travis-ci.org/MetacoSA/metaco-itbit-client)

[Metaco](https://metaco.com) [itBit](https://www.itbit.com) REST API client provides every single functionality of the itBit API.

Installation
----------------------------------------------

### Using NuGet

With nuget :
> **Install-Package Metaco.ItBit** 

Go on the [nuget website](https://www.nuget.org/packages/Metaco.ItBit/) for more information.


Testing
----------------------------------------------
You need a [itBit](https://exchange.itbit.com/signup) account for setting the Trading unit tests.

* Go to the TradeTest.cs file, open it and set your credential:

    ```
    TradeClient client = new TradeClient("your-client-key-here", "your-secret-key-here");
    ```

* Onces done, just run the test.

Contributing
----------------------------------------------
1. Fork this repository and make your changes in your fork
2. Add or Update the tests and run them to make sure they pass
3. Commit and push your changes to your fork `git push origin master`
4. Submit a pull request and we will handle the rest :)

Known Issues / Gotcha
----------------------------------------------
- There are three missing methods
 - [New Wallet Transfer](https://api.itbit.com/docs#trading-new-wallet-transfer),
 - [Get Trades](https://api.itbit.com/docs#trading-get-trades) and,
 - [New Wallet](https://api.itbit.com/docs#trading-new-wallet)


License
----------------------------------------------
GPLv3 (See LICENSE file).
