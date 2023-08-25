# Welcome to TuiFly.Turnover

This is a basic asp.net core console project for manage passengers , families a display turnover 

## PS : For test case you can change values from `passengers.csv in : TuiFly/Turnover/Application/StaticFiles`

## Step for building and run app

with docker cli run commands

### `build docker image`

```
docker build -t tuifly_turnover_app .
```

### `run console app`

```
docker run -d -p 7022:80 tuifly_turnover_app
```

## Launch and test app in docker console

`open the swagger of application TuiFly.BookingApi`

Open [http://localhost:7022/swagger/index.html](http://localhost:7022/swagger/index.html) to view it in your browser.
