const ctx = document.getElementById("pieChart").getContext('2d');
let rates = [];
fetch('https://api.coingecko.com/api/v3/simple/price?ids=bitcoin%2Cethereum%2Ctether%2Cripple%2Cbinancecoin&vs_currencies=usd')
    .then(res => res.json())
    .then(res => rates = [res.bitcoin.usd, res.ethereum.usd, res.tether.usd, res.ripple.usd, res.binancecoin.usd])
    .then(res => {
        fetch("http://localhost:5001/api/MainPage")
            .then(res => res.json())
            .then(res => {
        let lable = [];
        let value = [];
        let color = [];
        let i = 0;
        res.map(item => {
            if(item.value>0) {
                lable.push(item.shortName);
                value.push(item.value * rates[i++]);
                color.push(randomColor());
            }
        });
        if(lable.length == 0) {
            document.getElementById("chart").hidden = true;
        }
        else {
            document.getElementById("chart").hidden = false;
        }
        const chart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: lable,
                datasets: [{
                    label: 'currencies',
                    data: value,
                    backgroundColor: color,
                    hoverOffset: 4
                }]
            },
            options: {
                responsive: true
            }
        })
    });
});




