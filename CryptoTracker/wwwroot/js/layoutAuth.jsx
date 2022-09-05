fetch("http://localhost:5001/api/LayoutApi")
    .then(res => res.text())
    .then(res => {
        ReactDOM.render(
            <LayoutAuth username={res}/>,
            document.getElementById("auth")
        );
    });

class LayoutAuth extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <header>
                <nav className="navbar navbar-expand-sm navbar-toggleable-sm box-shadow mb-3">
                    <div className="container">
                        <a className="navbar-brand nav-text-light" asp-area="" href="http://localhost:5001/">CryptoTracker</a>
                        <button className="navbar-toggler" type="button" data-toggle="collapse"
                                data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                                aria-expanded="false" aria-label="Toggle navigation">
                            <span className="navbar-toggler-icon"></span>
                        </button>
                        <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                            <ul className="navbar-nav flex-grow-1 justify-content-between">
                                <div className="d-flex flex-row">
                                    <li className="nav-item">
                                        <a className="nav-link nav-text-light" href="http://localhost:5001/">Home</a>
                                    </li>
                                    <li className="nav-item">
                                        <a className="nav-link nav-text-light" href={"http://localhost:5001/deal/getdeals?name="+this.props.username}>Deals</a>
                                    </li>
                                    <li className="nav-item">
                                        <a className="nav-link nav-text-light" href={"http://localhost:5001/transaction/gettransactions?name="+this.props.username}>Transactions</a>
                                    </li>
                                </div>
                                <div className="d-flex flex-row">
                                    <li className="nav-item">
                                        <div className="nav-link nav-text-light">{this.props.username}</div>
                                    </li>
                                    <li className="nav-item">
                                        <a className="nav-link nav-text-light" href={"http://localhost:5001/account/logout/"}>Logout</a>
                                    </li>
                                </div>
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>
        );
    }
}