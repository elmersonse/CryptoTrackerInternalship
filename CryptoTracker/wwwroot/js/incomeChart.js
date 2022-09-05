let chart = new Chart();
function countMyData(rates, dealsGrouped, chartDays, currency) {
    let groupKeys = Object.keys(dealsGrouped);
    let tempMy = 0;
    let chartMyData = [];
    
    groupKeys.forEach(key => {
        if(key < rates[0][0]) {
            dealsGrouped[key].forEach(item => {
                if(item.currency === currency) {
                    if(item.dealType === "Sell") {
                        tempMy -= item.amount*item.rate;
                    }
                    else {
                        tempMy += item.amount*item.rate;
                    }
                }
            })
        }
    });

    for(let i = 0; i < chartDays.length; i++) {
        if(dealsGrouped[rates[i][0]] === undefined) {
            chartMyData.push(tempMy);
        }
        else {
            let tempSum = tempMy;
            dealsGrouped[rates[i][0]].forEach(item => {
                if(item.currency === currency) {
                    if(item.dealType === "Sell") {
                        tempSum -= item.amount*item.rate;
                    }
                    else {
                        tempSum += item.amount*item.rate;
                    }
                }
            });
            chartMyData.push(Math.round(tempSum*100)/100);
            tempMy = Math.round(tempSum*100)/100;
        }
    }
    return chartMyData;
}


function countLiveData(rates, allRates, dealsGrouped, chartDays, currency) {
    let groupKeys = Object.keys(dealsGrouped);
    let tempMy = 0;
    let tempMyCurr = 0;
    let chartLiveData = [];

    groupKeys.forEach(key => {
        if(key < rates[0][0]) {
            dealsGrouped[key].forEach(item => {
                if(item.currency === currency) {
                    if(item.dealType === "Sell") {
                        tempMy -= item.amount;
                    }
                    else {
                        tempMy += item.amount;
                    }
                }
            })
        }
    });

    for(let i = 0; i < chartDays.length; i++) {
        if(dealsGrouped[rates[i][0]] === undefined) {
            chartLiveData.push(Math.round(tempMy*rates[i][1]*100)/100);
        }
        else {
            let tempSum = tempMy;
            dealsGrouped[rates[i][0]].forEach(item => {
                if(item.currency === currency) {
                    if(item.dealType === "Sell") {
                        tempSum -= item.amount;
                    }
                    else {
                        tempSum += item.amount;
                    }
                }
            });
            chartLiveData.push(Math.round(tempSum*rates[i][1]*100)/100);
            tempMy = tempSum;
        }
    }
    return chartLiveData;
}

function createIncomeChart(fullName, shortName) {
    document.getElementById("incChart").hidden = false;
    fetch("http://localhost:5001/api/DealApi")
        .then(res => res.json())
        .then(deals => {
            const days = 7862400;
            const now = Math.round(new Date().getTime()/1000);
            fetch("https://api.coingecko.com/api/v3/coins/"+fullName+"/market_chart/range?vs_currency=usd&from="+(now-days)+"&to="+now)
                .then(btc => btc.json())
                .then(btc => {
                    let rates = structuredClone(btc.prices.slice(-30));
                    let chartDays = [];
                    let chartBtcRates = [];
                    let dealsGrouped = groupBy(deals);
                    rates.forEach(item => {
                        chartDays.push(new Date(parseInt(item[0])).customFormat("#D#.#MM#"));
                        chartBtcRates.push(item[1]);
                    });

                    let chartMyData = countMyData(rates, dealsGrouped, chartDays, shortName);
                    let chartLiveData = countLiveData(rates, btc.prices, dealsGrouped, chartDays, shortName);

                    let lineColor = randomColor();
                    let lineColor1 = randomColor();
                    const data = {
                        labels: chartDays,
                        datasets: [
                            {
                                label: "MyData",
                                data: chartMyData,
                                borderColor: lineColor,
                                backgroundColor: lineColor,
                            },
                            {
                                label: "LiveData",
                                data: chartLiveData,
                                borderColor: lineColor1,
                                backgroundColor: lineColor1
                            }
                        ]
                    }
                    const config = {
                        type: 'line',
                        data: data,
                        options: {
                            responsive: true,
                            plugins: {
                                legend: {
                                    position: 'top',
                                },
                                title: {
                                    display: true,
                                    text: 'Last assets'
                                }
                            }
                        },
                    };

                    chart = new Chart(ctx2, config);
                });

        });
}

const canvas = document.getElementById("incomeChart");
const ctx2 = canvas.getContext('2d');

function drawIncomeBtc() {
    chart.destroy();
    createIncomeChart("bitcoin", "BTC");
}
function drawIncomeEth() {
    chart.destroy();
    createIncomeChart("ethereum", "ETH");
}
function drawIncomeUsdt() {
    chart.destroy();
    createIncomeChart("tether", "USDT");
}
function drawIncomeXrp() {
    chart.destroy();
    createIncomeChart("ripple", "XRP");
}
function drawIncomeBnb() {
    chart.destroy();
    createIncomeChart("binancecoin", "BNB");
}

document.getElementById("btc").addEventListener("click", drawIncomeBtc);
document.getElementById("eth").addEventListener("click", drawIncomeEth);
document.getElementById("usdt").addEventListener("click", drawIncomeUsdt);
document.getElementById("xrp").addEventListener("click", drawIncomeXrp);
document.getElementById("bnb").addEventListener("click", drawIncomeBnb);

drawIncomeBtc();