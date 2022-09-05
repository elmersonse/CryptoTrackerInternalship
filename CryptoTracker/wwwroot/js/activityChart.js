const ctx1 = document.getElementById("activityChart").getContext('2d');

fetch("./api/DealApi")
    .then(res => res.json())
    .then(deals => {
        let dealsUsd = structuredClone(deals);
        dealsUsd.forEach(item => {
            item.amount *= item.rate;
            item.amount = Math.round(item.amount * 100000) / 100000;
        });
        let dealsGrouped = groupBy(dealsUsd);
        let keys = Object.keys(dealsGrouped);
        let amount = 0;
        let chartData = []
        keys.forEach(key => {
            dealsGrouped[key].forEach(item => {
                if(item.dealType === "Sell") {
                    amount -= item.amount;
                }
                else {
                    amount += item.amount;
                }
            });
            chartData.push(Math.round(amount * 100) / 100);
        });
        let chartLabels = keys.map(item => new Date(parseInt(item)).customFormat("#D#.#MM#"));
        if(chartLabels.length === 0) {
            document.getElementById("actChart").hidden = true;
        }
        let lineColor = randomColor();
        const data = {
            labels: chartLabels,
            datasets: [
                {
                    label: "USD",
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

        const chart2 = new Chart(ctx1, config);
    });