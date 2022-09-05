const days = 7862400;
const now = Math.round(new Date().getTime()/1000);
const canvas1 = document.getElementById("ratesChart");
const ctx3 = canvas1.getContext('2d');
let chart1 = new Chart();
function createChart(fullName) {
    fetch("https://api.coingecko.com/api/v3/coins/"+fullName+"/market_chart/range?vs_currency=usd&from="+(now-days)+"&to="+now)
        .then(btc => btc.json())
        .then(btc => {
            let name;
            switch (fullName) {
                case "bitcoin":
                    name = "Bitcoin";
                    break;
                case "ethereum":
                    name = "Ethereum";
                    break;
                case "tether":
                    name = "Tether";
                    break;
                case "ripple":
                    name = "Ripple";
                    break;
                case "binancecoin":
                    name = "Binance Coin";
                    break;
            }
            let rates = structuredClone(btc.prices.slice(-30));
            let chartLabels = [];
            let chartData = [];
            rates.forEach(item => {
                chartLabels.push(new Date(parseInt(item[0])).customFormat("#D#.#MM#"));
                chartData.push(item[1]);
            });
            
            let lineColor = randomColor();
            const data = {
                labels: chartLabels,
                datasets: [
                    {
                        label: name,
                        data: chartData,
                        borderColor: lineColor,
                        backgroundColor: lineColor,
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

            chart1 = new Chart(ctx3, config);
        });
}

function drawRateBtc() {
    chart1.destroy();
    createChart("bitcoin");
}
function drawRateEth() {
    chart1.destroy();
    createChart("ethereum");
}
function drawRateUsdt() {
    chart1.destroy();
    createChart("tether");
}
function drawRateXrp() {
    chart1.destroy();
    createChart("ripple");
}
function drawRateBnb() {
    chart1.destroy();
    createChart("binancecoin");
}

document.getElementById("btcRate").addEventListener("click", drawRateBtc);
document.getElementById("ethRate").addEventListener("click", drawRateEth);
document.getElementById("usdtRate").addEventListener("click", drawRateUsdt);
document.getElementById("xrpRate").addEventListener("click", drawRateXrp);
document.getElementById("bnbRate").addEventListener("click", drawRateBnb);

drawRateBtc();