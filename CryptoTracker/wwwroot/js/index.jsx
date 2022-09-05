fetch('/api/MainPage')
    .then(result => result.json())
    .then(result => {
        fetch('https://api.coingecko.com/api/v3/simple/price?ids=bitcoin%2Cethereum%2Ctether%2Cripple%2Cbinancecoin&vs_currencies=usd')
            .then(res => res.json())
            .then(res => {
                let rates = [res.bitcoin.usd, res.ethereum.usd, res.tether.usd, res.ripple.usd, res.binancecoin.usd];
                let valueBought = 0;
                let valueLive = 0;
                let i = 0;
                let table = result.map((item) => {
                    valueBought += item.boughtUsd;
                    valueLive += item.value*rates[i];
                    return <tr key={item.shortName}>
                        <td>{item.name}</td>
                        <td>{item.shortName}</td>
                        <td>{Math.round(item.value * 100000) / 100000}</td>
                        <td>{Math.round(item.value*rates[i++] * 100) / 100}$</td>
                    </tr>
                });
                valueBought = Math.round(valueBought * 100) / 100
                valueLive = Math.round(valueLive * 100) / 100
                let profit = Math.round((valueLive - valueBought) * 100) / 100;
                let profitPercent = Math.round((profit*100)/valueBought * 100) / 100;
                if(isNaN(profitPercent)) {
                    profitPercent = 0;
                }
                let profitString = profit + "$ ("+profitPercent+"%)";
                
                fetch("/api/MainPage", {
                    method: "POST",
                    headers: {
                        "Accept": "application/json", "Content-Type": "application/json"
                    },
                    body: JSON.stringify(profit),
                });
                
                ReactDOM.render(
                    <JsonData table={table} rates={rates} bought={valueBought} live={valueLive} profit={profitString}/>,
                    document.getElementById("app")
                )
            });
    });

class JsonData extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return(
            <div>
                <table className="table">
                    <thead>
                    <tr>
                        <th>Name</th>
                        <th>Short Name</th>
                        <th>Value</th>
                        <th>USD</th>
                    </tr>
                    </thead>
                    <tbody>
                    {this.props.table}
                    </tbody>
                </table>
                <table className="table">
                    <thead>
                        <tr>
                            <th>Bought</th>
                            <th>Now</th>
                            <th>Profit</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>{this.props.bought}$</td>
                            <td>{this.props.live}$</td>
                            <td>{this.props.profit}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        )
    }
}

