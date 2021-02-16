# Coinbase Pro Return Calculator

**First version for UK users who have bought in Euros (using SEPA bank transfer). It will calculate the return from Crypto ordered on Coinbase Pro, with key-free API calls to get the current Crypto price and also the current GBP-Euro price for displaying prices in GBP**

Console output from dummy fills.csv file:

| Name | Amount     | Total Cost        | Current Value     | Gain/Loss       | %       | Crypto's Live Price     | Your Average Price (Weighted) |
| ---- | ---------- | ----------------- | ----------------- | --------------- | ------- | ----------------------- | ----------------------------- |
| ETH  | 0.04077848 | £34.93 (€40.00)   | £51.65 (€59.14)   | £16.71 (€19.14) | 47.84 % | £1,266.48 (€1,450.17)   | £852.37 (€976.00)             |
| BTC  | 0.00655106 | £178.70 (€204.62) | £230.71 (€264.18) | £52.01 (€59.56) | 29.11 % | £35,217.66 (€40,325.72) | £27,142.62 (€31,079.45)       |

Visit https://pro.coinbase.com/orders/filled to download your statement in CSV format

<img src="https://i.imgur.com/x9rwwv5.jpg" width="500">

<img src="https://i.imgur.com/yUau34c.jpg" width="500">

The statement will be sent to your email address. You can download the CSV file from here.

Place 'fills.csv' in the 'Data' folder, and if necessary Copy to output directory