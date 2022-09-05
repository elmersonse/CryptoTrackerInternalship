fetch("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin%2Cethereum%2Ctether%2Cripple%2Cbinancecoin&vs_currencies=usd")
    .then(res => res.json())
    .then(res => {
        let table = <tbody>
            <tr>
                <td>Bitcoin</td>
                <td>{Math.round(res.bitcoin.usd*100)/100}$</td>
            </tr>
            <tr>
                <td>Ethereum</td>
                <td>{Math.round(res.ethereum.usd*100)/100}$</td>
            </tr>
            <tr>
                <td>Tether</td>
                <td>{Math.round(res.tether.usd*100)/100}$</td>
            </tr>
            <tr>
                <td>Ripple</td>
                <td>{Math.round(res.ripple.usd*100)/100}$</td>
            </tr>
            <tr>
                <td>Binance Coin</td>
                <td>{Math.round(res.binancecoin.usd*100)/100}$</td>
            </tr>
        </tbody>
        
        ReactDOM.render(
            <Rates table={table} />,
            document.getElementById("rates")
        );
    });

class Rates extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className={"d-flex flex-column align-items-center"}>
                <h2>RATES</h2>
                <table className="table">
                    <thead>
                    <tr>
                        <th>Name</th>
                        <th>Price</th>
                    </tr>
                    </thead>
                        {this.props.table}
                </table>
            </div>
        );
    }
}