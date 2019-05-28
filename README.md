# chatbot-net-core

The Chatbot software is powered by a [language understanding api](https://www.luis.ai/home) to handle human questions.

**Chatbot Understands**

*   Greetings
*   Weather
*   Stocks
*   Gibberish (not really)

**Developers**  
<small class="text-black-50">Setup</small>  
**Chatbot's brain** - Chatbot requires some free-ish apis to answer questions.  
<small class="text-black-50">*NOTE: You must create your own accounts to get these endpoints and api keys.</small>

*   Language Understanding (LUIS) - <small class="text-black-50">LUIS enables you to integrate natural language understanding into your chatbot or other application without having to create the complex part of machine learning models.[](https://www.luis.ai/welcome)</small>
    *   LUIS endpoint (ex. https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/)
    *   LUIS App Key
    *   LUIS Api Key
    *   Create 3 starter intents
        1.  Greeting
        2.  CheckWeather
        3.  CheckStock
*   Google Geocode - <small class="text-black-50">The Geocoding API is a service that provides geocoding and reverse geocoding of addresses.[](https://developers.google.com/maps/documentation/geocoding/start)</small>
    *   Google geocode endpoint (ex. https://maps.googleapis.com/maps/api/geocode/json?address=)
    *   Google Api Key
*   DarkSky - <small class="text-black-50">The Dark Sky API allows you to look up the weather anywhere on the globe.[](https://darksky.net/dev/docs)</small>
    *   DarkSky endpoint (ex. https://api.darksky.net/forecast/)
    *   DarkSky Api Key
*   Aplha Vantage - <small class="text-black-50">This suite of APIs provide realtime and historical global equity data in 4 different temporal resolutions: (1) daily, (2) weekly, (3) monthly, and (4) intraday. Daily, weekly, and monthly time series contain 20+ years of historical data.[](https://www.alphavantage.co/documentation/)</small>
    *   Aplha Advantage endpoint (ex. https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY)
    *   Aplha Advantage Api Key
*   Bootswatch - <small class="text-black-50">You can use the API to integrate the themes with your platform.[](https://bootswatch.com/help/#api)</small>
    *   Bootswatch endpoint (ex. https://bootswatch.com/api/4.json)
*   Amazon Web Services <small class="text-black-50">(optional)</small>- <small class="text-black-50">Amazon Web Services offers reliable, scalable, and inexpensive cloud computing services. Free to join, pay only for what you use.[](https://aws.amazon.com/)</small>
    *   AWS public endpoint
    *   S3 Bucket

<span>You should place these keys in the templated **appsettings.json** file.</span>`  
```
"Configurations": {  
  "AWSCDN": "",  
  "S3BucketName": "",  
  "ThemesEndpoint": "https://bootswatch.com/api/4.json",  
  "LanguageUnderstandingEndpoint": "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/",  
  "LanguageUnderstandingAppKey": "YOUR KEY HERE",  
  "LanguageUnderstandingApiKey": "YOUR KEY HERE",  
  "GoogleAddressLookupEndpoint": "https://maps.googleapis.com/maps/api/geocode/json?address=",  
  "GoogleApiKey": "YOUR KEY HERE",  
  "DarkSkyEndpoint": "https://api.darksky.net/forecast/",  
  "DarkSkySecretKey": "YOUR KEY HERE",  
  "AplhaAdvantageApiKey": "YOUR KEY HERE",  
  "AplhaAdvantageApiEndPoint": "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY"  
}`
```
